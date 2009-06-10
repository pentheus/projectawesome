using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using AwesomeEngine.Items;
using AwesomeEngine.Enemies;

namespace AwesomeEngine
{
    public class Octree 
    {
        public const int OBJECT_LIMIT = 2;

        Node root;
        float treeSize;
        //
        public Octree(float treeSize)
        {
            //Compute octree min and max values
            Vector3 min = new Vector3(-treeSize/2);
            Vector3 max = new Vector3(treeSize/2);
            //Assign values
            root = new Node(treeSize, min, max, Vector3.Zero);
            this.treeSize = treeSize;
        }

        public float TreeSize
        {
            get { return treeSize; }
        }

        public Node createChild(int nodeIndex, float parentSize, Vector3 parentCenter)
        {
            //Compute the distance
            float nodeSize = parentSize / 2;
            //Determine how to compute the min, max, and center based on the index of the node
            Vector3 min;
            Vector3 max;
            Vector3 center;
            switch (nodeIndex)
            {
                case 0:
                    min = new Vector3(parentCenter.X - nodeSize, parentCenter.Y, parentCenter.Z - nodeSize);
                    max = new Vector3(parentCenter.X, parentCenter.Y + nodeSize, parentCenter.Z);
                    center = new Vector3(parentCenter.X - (nodeSize / 2), parentCenter.Y + (nodeSize / 2), parentCenter.Z - (nodeSize / 2));
                    break;

                case 1:
                    min = new Vector3(parentCenter.X, parentCenter.Y, parentCenter.Z - nodeSize);
                    max = new Vector3(parentCenter.X + nodeSize, parentCenter.Y + nodeSize, parentCenter.Z);
                    center = new Vector3(parentCenter.X + (nodeSize / 2), parentCenter.Y + (nodeSize / 2), parentCenter.Z - (nodeSize / 2));
                    break;

                case 2:
                    min = new Vector3(parentCenter.X - nodeSize, parentCenter.Y - nodeSize, parentCenter.Z - nodeSize);
                    max = new Vector3(parentCenter.X, parentCenter.Y, parentCenter.Z);
                    center = new Vector3(parentCenter.X - (nodeSize / 2), parentCenter.Y - (nodeSize / 2), parentCenter.Z - (nodeSize / 2));
                    break;

                case 3:
                    min = new Vector3(parentCenter.X, parentCenter.Y - nodeSize, parentCenter.Z - nodeSize);
                    max = new Vector3(parentCenter.X + nodeSize, parentCenter.Y, parentCenter.Z);
                    center = new Vector3(parentCenter.X + (nodeSize / 2), parentCenter.Y - (nodeSize / 2), parentCenter.Z - (nodeSize / 2));
                    break;

                case 4:
                    min = new Vector3(parentCenter.X - nodeSize, parentCenter.Y, parentCenter.Z);
                    max = new Vector3(parentCenter.X, parentCenter.Y + nodeSize, parentCenter.Z + nodeSize);
                    center = new Vector3(parentCenter.X - (nodeSize / 2), parentCenter.Y + (nodeSize / 2), parentCenter.Z + (nodeSize / 2));
                    break;

                case 5:
                    min = new Vector3(parentCenter.X, parentCenter.Y, parentCenter.Z);
                    max = new Vector3(parentCenter.X + nodeSize, parentCenter.Y + nodeSize, parentCenter.Z + nodeSize);
                    center = new Vector3(parentCenter.X + (nodeSize / 2), parentCenter.Y + (nodeSize / 2), parentCenter.Z + (nodeSize / 2));
                    break;

                case 6:
                    min = new Vector3(parentCenter.X - nodeSize, parentCenter.Y - nodeSize, parentCenter.Z);
                    max = new Vector3(parentCenter.X, parentCenter.Y, parentCenter.Z + nodeSize);
                    center = new Vector3(parentCenter.X - (nodeSize / 2), parentCenter.Y - (nodeSize / 2), parentCenter.Z + (nodeSize / 2));
                    break;

                case 7:
                    min = new Vector3(parentCenter.X, parentCenter.Y - nodeSize, parentCenter.Z);
                    max = new Vector3(parentCenter.X + nodeSize, parentCenter.Y, parentCenter.Z + nodeSize);
                    center = new Vector3(parentCenter.X + (nodeSize / 2), parentCenter.Y - (nodeSize / 2), parentCenter.Z + (nodeSize / 2));
                    break;

                default:
                    min = new Vector3(0);
                    max = new Vector3(2);
                    center = new Vector3(1);
                    break;
            }
            //Return the node
            return new Node(nodeSize, min, max, center);
        }

        public void SplitNode(Node n)
        {
            for(int i = 0; i < 8; i++)
                n.Children[i] = createChild(i, n.Size, n.Center);
        }

        //Top level function for removing intersecting nodes
        public void remove(BoundingBox subspace)
        {
            removeNodes(subspace, root, null);
        }

        //Recursive function for removing nodes
        public void removeNodes(BoundingBox subspace, Node lookup, Node parent)
        {
            //If the node has children, recurse to them.
            if(lookup.HasChildren())
            {
                foreach(Node n in lookup.Children)
                    removeNodes(subspace, n, lookup);
            }
            //If the node has no children, determine if it's intersecting and needs to be deleted
            else
            {
                if (lookup.intersectsWith(subspace))
                {
                    parent.removeChild(lookup);
                }
            }
        }

        //Top level function to lookup nodes
        public List<Node> lookup(BoundingBox subspace)
        {
            return lookupNodes(subspace, root);
        }

        //Recurrsive function to lookup nodes
        public List<Node> lookupNodes(BoundingBox subspace, Node lookup)
        {
            //Create a new list to append the results to
            List<Node> resultnodes = new List<Node>();
            //If the node has children, recurse to them.
            if (lookup.HasChildren())
            {
                foreach (Node n in lookup.Children)
                    resultnodes.AddRange(lookupNodes(subspace, n));
            }
            //If the node has no children, determine if it's intersecting and needs to be returned
            else
            {
                if (lookup.intersectsWith(subspace))
                    resultnodes.Add(lookup);
            }
            return resultnodes;
        }



        //----------------------------Object Manipulation--------------------------------
        public void AddObject(ModelInfo obj)
        {
            
            //Add the object into the node
            AddRecursiveObject(obj, root);
        }

        //Recursively find lowest node and add the given object
        public void AddRecursiveObject(ModelInfo obj, Node node)
        {
            if (node.BoundingBox.Contains(obj.BoundingSphere) == ContainmentType.Contains)
            {
                node.DrawableObjects.Add(obj);

                if (node.DrawableObjects.Count > OBJECT_LIMIT)
                    DistributeObjects(node);
            }
        }

        public void AddItem(Item item)
        {
            Console.WriteLine("Added an item.");
            AddRecursiveItem(item, root);
        }

        public void AddRecursiveItem(Item item, Node node)
        {
            if (node.HasChildren())
            {
                foreach (Node n in node.Children)
                {
                    if (node.BoundingBox.Contains(item.Model.BoundingSphere) == ContainmentType.Contains)
                    {
                        AddRecursiveItem(item, n);
                        return;
                    }
                }
            }
            node.Items.Add(item);
        }

        public void AddEntity(LogicEntity entity)
        {
            AddRecursiveEntity(entity, root);
        }

        public void AddRecursiveEntity(LogicEntity entity, Node node)
        {
            if (node.HasChildren())
            {
                foreach (Node n in node.Children)
                {
                    if (node.BoundingBox.Contains(entity.BoundingSphere) == ContainmentType.Contains)
                    {
                        AddRecursiveEntity(entity, n);
                        return;
                    }
                }
            }
            node.Entities.Add(entity);
        }


        /// <summary>
        /// When the number of DrawableObjects exceed a certain threshold in
        /// the parent's list of DrawableObjects. This method should be called
        /// to distribute the nodes among the children of the parent node.
        /// </summary>
        /// <param name="parent">The node that you want to distribute DrawableObjects from</param>
        public void DistributeObjects(Node parent)
        {
            if(!parent.HasChildren())
                SplitNode(parent);

            /*foreach (Node child in parent.Children)
            {
                Stack<ModelInfo> tempStack = new Stack<ModelInfo>(parent.DrawableObjects);
                while (tempStack.Count != 0) 
                {
                    if (child.containsPos(tempStack.Peek().Position))
                    {
                        child.DrawableObjects.Add(tempStack.Peek());
                        parent.DrawableObjects.Remove(tempStack.Peek());
                    }

                    tempStack.Pop();
                }
                if (child.DrawableObjects.Count > OBJECT_LIMIT)
                    DistributeObjects(child);

            }*/

            for (int i = parent.DrawableObjects.Count-1; i > 0; i--)
            {
                foreach (Node child in parent.Children)
                {
                    if(child.BoundingBox.Contains(parent.DrawableObjects[i].BoundingSphere) == ContainmentType.Contains)
                    {
                        child.DrawableObjects.Add(parent.DrawableObjects[i]);
                        parent.DrawableObjects.Remove(parent.DrawableObjects[i]);
                        if(child.DrawableObjects.Count > OBJECT_LIMIT)
                            DistributeObjects(child);
                        break;
                    }
                }
            }
        }

        //Return a list of all objects contained in the tree
        public List<ModelInfo> getDrawableObjects()
        {
            List<ModelInfo> objects = new List<ModelInfo>();
            recurseObjects(objects, root);
            return objects;
        }

        public List<ModelInfo> recurseObjects(List<ModelInfo> objects, Node node)
        {
            objects.AddRange(node.DrawableObjects);
            if (node.HasChildren())
            {
                foreach (Node n in node.Children)
                    recurseObjects(objects, n);
            }
            return objects;
        }

        //Return a list of the enemies contained in the tree
        public List<Enemy> GetEnemies()
        {
            List<Enemy> enemies = new List<Enemy>();
            recurseEnemies(enemies, root);
            return enemies;
        }

        public List<Enemy> recurseEnemies(List<Enemy> enemies, Node node)
        {
            enemies.AddRange(node.Enemies);
            if (node.HasChildren())
            {
                foreach (Node n in node.Children)
                    recurseEnemies(enemies, n);
            }
            return enemies;
        }

        //Return a list of the items contained in the tree
        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();
            recurseItems(items, root);
            return items;
        }

        public List<Item> recurseItems(List<Item> items, Node node)
        {
            items.AddRange(node.Items);
            if (node.HasChildren())
            {
                foreach (Node n in node.Children)
                    recurseItems(items, n);
            }
            return items;
        }

        public List<LogicEntity> GetEntities()
        {
            List<LogicEntity> entities = new List<LogicEntity>();
            recurseEntities(entities, root);
            return entities;
        }

        public List<LogicEntity> recurseEntities(List<LogicEntity> entities, Node node)
        {
            entities.AddRange(node.Entities);
            if (node.HasChildren())
            {
                foreach (Node n in node.Children)
                    recurseEntities(entities, n);
            }
            return entities;
        }

        //Add world geometry to the root node of the octree
        public void addGeometry(ModelInfo geometry)
        {
            root.WGeometry = geometry;
        }

        //Retrieve the tree's world geometry
        public ModelInfo getGeometry()
        {
            return root.WGeometry;
        }

        public Node Root
        {
            get { return root; }
        }
    }
}

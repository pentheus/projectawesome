using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace AwesomeEngine
{
    public class Octree 
    {
        public const int OBJECT_LIMIT = 8;

        Node root;
        float treeSize;

        public Octree(float treeSize)
        {
            //Compute octree min and max values
            Vector3 min = new Vector3(-treeSize/2);
            Vector3 max = new Vector3(treeSize/2);
            //Assign values
            root = new Node(treeSize, min, max, new Vector3(0));
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
        public void addObject(Vector3 pos, ModelInfo obj)
        {
            
            //Add the object into the node
            AddRecursiveObject(pos, obj, root);
        }

        //Recursively find lowest node and add the given object
        public void AddRecursiveObject(Vector3 pos, ModelInfo obj, Node node)
        {
            if (node.HasChildren())
            {
                foreach (Node n in node.Children)
                {
                    if (n.intersectsWith(obj))
                        AddRecursiveObject(pos, obj, n);
                }
            }
            else
            {
                node.DrawableObjects.Add(obj);
                if(node.Children.Length > OBJECT_LIMIT)
                {
                    SplitNode(node);
                    DistributeObjects(node);
                }
            }
        }

        /// <summary>
        /// When the number of DrawableObjects exceed a certain threshold in
        /// the parent's list of DrawableObjects. This method should be called
        /// to distribute the nodes among the children of the parent node.
        /// </summary>
        /// <param name="parent">The node that you want to distribute DrawableObjects from</param>
        public void DistributeObjects(Node parent)
        {
            foreach (Node child in parent.Children)
            {
                foreach (ModelInfo model in parent.DrawableObjects)
                {
                    if (child.intersectsWith(model))
                    {
                        child.DrawableObjects.Add(model);
                        parent.DrawableObjects.Remove(model);
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

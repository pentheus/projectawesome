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
    #region Node

    public class Node
    {
        BoundingBox boundingBox;
        Node[] children;
        List<ModelInfo> objects;
        //World geometry is only contained by the root node of the octree
        ModelInfo worldgeometry;
        float size;
        Vector3 min;
        Vector3 max;
        Vector3 center;
        Boolean haschildren;
        //TODO:
        //Later we'll add an array list for lights once we create a class that stores lighting data
        //This will also mean we have to implement better lighting shaders that allow for more than 2
        //dynamic lights.
        //List<Lights> lights 

        /// <summary>
        /// A constructor for the Node object
        /// </summary>
        /// <param name="nodeIndex">This is the index for a particular node. For example the upper left hand child that is in the -z axis would be designated 0.</param>
        /// <param name="size">This is the length of the edge of its bounding volume</param>
        public Node(float size, Vector3 min, Vector3 max, Vector3 center)
        {
            this.size = size;
            this.min = min;
            this.max = max;
            this.center = center;
            children = new Node[8];
            objects = new List<ModelInfo>();
            boundingBox = new BoundingBox();//Need the add the proper calculations for creating a new bounding box
            haschildren = false;
        }

        public List<ModelInfo> DrawableObjects
        {
            get { return objects; }
        }

        //Add a child node to this node
        public void addChild(int nodeIndex, Node child)
        {
            children[nodeIndex] = child;
        }

        //Add world geometry to this node
        public void addGeometry(ModelInfo geometry)
        {
            worldgeometry = geometry;
        }

        //Remove the designated child
        public void removeChild(Node child)
        {
            for (int i = 0; i < 8; i++ )
            {
                if (children[i].Equals(child))
                {
                    children[i] = null;
                }
            }
        }

        
        //--------------Affecting objects contained by the node-----------------

        //Add an object to the node
        public void addObject(ModelInfo addobject)
        {
            objects.Add(addobject);
            addobject.Node = this;
        }

        //Returns whether this node's bounding box contains the given vector position
        public Boolean containsPos(Vector3 pos)
        {
            if(boundingBox.Intersects(new BoundingSphere(pos, 0)))
            {
                return true;
            }
            return false;
        }

        //Returns whether this node intersects with a given bounding box
        public Boolean intersectsWith(BoundingBox box)
        {
            if(boundingBox.Intersects(box))
                return true;
            else
                return false;
        }

        public float getSize()
        {
            return size;
        }

        public Vector3 getCenter()
        {
            return center;
        }

        public void setHasChildren(Boolean v)
        {
            haschildren = v;
        }

        public Boolean hasChildren()
        {
            return haschildren;
        }

        public Node[] getChildren()
        {
            return children;
        }
    }
    #endregion 

    #region Octree
    public class Octree
    {
        Node root;
        int treeSize;

        public Octree(int treeSize)
        {
            //Compute octree min and max values
            Vector3 min = new Vector3(-treeSize/2);
            Vector3 max = new Vector3(treeSize/2);
            //Assign values
            root = new Node(treeSize, min, max, new Vector3(0));
            this.treeSize = treeSize;
        }

        //Creates the tree with nodes  to the given depth
        public void createTree(int depth, Node buildon)
        {
            if(depth > 0)
            {
                for(int i = 0; i < 8; i++)
                {
                    Node added =  createChild(i, buildon.getSize(), buildon.getCenter());
                    buildon.addChild(i,added);
                    buildon.setHasChildren(true);
                    createTree(depth-1, added);
                }
            }
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

        //Top level function for removing intersecting nodes
        public void remove(BoundingBox subspace)
        {
            removeNodes(subspace, root, null);
        }

        //Recursive function for removing nodes
        public void removeNodes(BoundingBox subspace, Node lookup, Node parent)
        {
            //If the node has children, recurse to them.
            if(lookup.hasChildren())
            {
                foreach(Node n in lookup.getChildren())
                    removeNodes(subspace, n, lookup);
            }
            //If the node has no children, determine if it's intersecting and needs to be deleted
            else
            {
                if (lookup.intersectsWith(subspace))
                {
                    parent.removeChild(lookup);
                    if (parent.getChildren().Equals(null))
                        parent.setHasChildren(false);
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
            if (lookup.hasChildren())
            {
                foreach (Node n in lookup.getChildren())
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
        public void addObject(Vector3 pos, ModelInfo data)
        {
            Node lowlevel = root;
            //While the current node has children...
            while (lowlevel.hasChildren())
            {
                //...Find the one that contains the given point
                foreach (Node n in lowlevel.getChildren())
                {
                    if (n.containsPos(pos))
                    {
                        lowlevel = n;
                        break;
                    }
                }
            }
            //Add the object into the node
            lowlevel.addObject(data);
        }

        //Add world geometry to the root node of the octree
        public void addgeometry(ModelInfo geometry)
        {
            root.addGeometry(geometry);
        }

        public void removeObject(Vector3 pos, ModelInfo data)
        {
            //Need code here.
        }
        //TODO:
        //Need remove (remove node completed), lookup, buildTree(but that kind of goes along with add).
        //It is probably not necessary to have specific object lookups in this class
        //For good OO programming purposes, we'll have another class that handles dealing with objects.

    }
    #endregion
}

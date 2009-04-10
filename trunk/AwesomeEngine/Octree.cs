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
    #region ModelInfo

    /// <summary>
    /// This is wrapper for XNA's model class. It provides pertinent information about the
    /// model. For example, its position, rotation, scale, the model itself, the bounding volume,
    /// and a pointer to the node in which in resides.
    /// </summary>
    public class ModelInfo
    {
        Vector3 pos;
        Vector3 rotation;
        Vector3 scale;
        Model model;
        BoundingSphere boundingSphere;
        Node node;
        
        public ModelInfo(Vector3 pos, Vector3 rotation, Vector3 scale, Model model, BoundingSphere boundingSphere, Node node)
        {
            this.pos = pos;
            this.rotation = rotation;
            this.scale = scale;
            this.model = model;
            this.boundingSphere = boundingSphere;
            this.node = node;
        }
    }
    #endregion

    #region Node

    public class Node
    {
        BoundingBox boundingBox;
        Node[] children;
        List<ModelInfo> objects;
        float size;
        Vector3 min;
        Vector3 max;
        Vecotr3 center;
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
        }

        public void addChild(int nodeIndex, float parentSize, Vector3 parentCenter)
        {
            //Compute the distance
            float nodeSize = parentSize / 2;
            //Determine how to compute the min, max, and center based on the index of the node
            Vector3 min;
            Vector3 max;
            Vector3 center;
            switch(nodeIndex)
            {    
                case 0:
                    min = new Vector3(parentCenter.X-nodeSize, parentCenter.Y, parentCenter.Z-nodeSize);
                    max = new Vector3(parentCenter.X, parentCenter.Y+nodeSize, parentCenter.Z);
                    center = new Vector3(parentCenter.X-(nodeSize/2), parentCenter.Y+(nodeSize/2), parentCenter.Z-(nodeSize/2));
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
            //Create the new node and add it to the parent
            children[nodeIndex] = new Node(nodeSize, min, max, center);
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

        public void addNode(int nodeIndex, float parentSize, Vector3 parentCenter)
        {
            //Compute the distance
            float nodeSize = parentSize / 2;
            //Determine how to compute the min, max, and center based on the index of the node
            Vector3 min;
            Vector3 max;
            Vector3 center;
            switch(nodeIndex)
            {    
                case 0:
                    min = new Vector3(parentCenter.X-nodeSize, parentCenter.Y, parentCenter.Z-nodeSize);
                    max = new Vector3(parentCenter.X, parentCenter.Y+nodeSize, parentCenter.Z);
                    center = new Vector3(parentCenter.X-(nodeSize/2), parentCenter.Y+(nodeSize/2), parentCenter.Z-(nodeSize/2));
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
            //Create the new node
            root.addChild(
        }

        //TODO:
        //Need add(Nodes only, no models), remove, lookup, buildTree(but that kind of goes along with add).
        //It is probably not necessary to have specific object lookups in this class
        //For good OO programming purposes, we'll have another class that handles dealing with objects.

    }
    #endregion
}

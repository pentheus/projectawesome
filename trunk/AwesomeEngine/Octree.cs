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
        public Node(int nodeIndex, float size)
        {
            children = new Node[8];
            objects = new List<ModelInfo>();
            boundingBox = new BoundingBox(); //Need the add the proper calculations for creating a new bounding box

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
            root = new Node();
            this.treeSize = treeSize;
        }

        //TODO:
        //Need add(Nodes only, no models), remove, lookup, buildTree(but that kind of goes along with add).
        //It is probably not necessary to have specific object lookups in this class
        //For good OO programming purposes, we'll have another class that handles dealing with objects.

    }
    #endregion
}

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
    public class Node
    {
        BoundingBox boundingBox;
        Node[] children;
        List<ModelInfo> objects;
        List<Enemy> enemies;
        List<Item> items;
        List<LogicEntity> entities;
        ModelInfo worldgeometry; //null in all nodes except root
        float size;
        Vector3 min;
        Vector3 max;
        Vector3 center;

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
            enemies = new List<Enemy>();
            items = new List<Item>();
            boundingBox = new BoundingBox(min, max);
        }

        public List<ModelInfo> DrawableObjects
        {
            get { return objects; }
        }

        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        public List<Item> Items
        {
            get { return items; }
        }

        public Node[] Children
        {
            get { return children; }
            set { children = value; }
        }

        //Add world geometry to this node
        public ModelInfo WGeometry
        {
            get { return worldgeometry; }
            set { worldgeometry = value; }
        }

        //Remove the designated child
        public void removeChild(Node child)
        {
            for (int i = 0; i < 8; i++)
            {
                if (children[i].Equals(child))
                {
                    children[i] = null;
                }
            }
        }

        //Divides the node into eight child nodes


        //--------------Affecting objects contained by the node-----------------

        //Returns whether this node's bounding box contains the given vector position
        public Boolean containsPos(Vector3 pos)
        {
            if (boundingBox.Intersects(new BoundingSphere(pos, 0)))
            {
                return true;
            }
            return false;
        }

        //Returns whether this node intersects with a given bounding box
        public Boolean intersectsWith(BoundingBox box)
        {
            if (boundingBox.Intersects(box))
                return true;
            return false;
        }

        public Boolean intersectsWith(BoundingFrustum box)
        {
            if (boundingBox.Intersects(box))
                return true;
            return false;
        }

        public Boolean intersectsWith(ModelInfo model)
        {
            foreach (ModelMesh mesh in model.Model.Meshes)
            {
                if (mesh.BoundingSphere.Intersects(BoundingBox))
                    return true;
            }
            return false;
        }

        public float Size
        {
            get { return size; }
        }

        public Vector3 Center
        {
            get { return center; }
        }

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        public Boolean HasChildren()
        {
            foreach (Node child in this.children)
                if (child != null)
                    return true;
            return false;
        }
            
    }
}

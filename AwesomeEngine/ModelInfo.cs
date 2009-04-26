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

        public ModelInfo()
        {
        }
        
        public ModelInfo(Vector3 pos, Vector3 rotation, Vector3 scale, Model model, BoundingSphere boundingSphere, Node node)
        {
            this.pos = pos;
            this.rotation = rotation;
            this.scale = scale;
            this.model = model;
            this.boundingSphere = boundingSphere;
            this.node = node;
        }

        public Vector3 Position
        {
            get{ return pos; }
            set{ pos = value; }
        }

        public Vector3 Rotation
        {
            get{ return rotation; }
            set{ rotation = value; }
        }

        public Vector3 Scale
        {
            get{ return scale; }
            set{ scale = value; }
        }

        public Matrix WorldMatrix
        {
            get
            {
                return (Matrix.CreateTranslation(pos) * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X)) * 
                    Matrix.CreateRotationY(rotation.Y) *Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z)) * 
                    Matrix.CreateScale(scale.X, scale.Y, scale.Z));
            }
        }

        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        public BoundingSphere BoundingVolume
        {
            get { return boundingSphere; }
            set { boundingSphere = value; }
        }

        public Node Node
        {
            get { return node; }
            set { node = value; }
        }
    }

}

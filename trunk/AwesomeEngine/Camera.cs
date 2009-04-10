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
    abstract class Camera
    {
        protected Vector3 pos;
        protected Vector3 rotation;

        protected Matrix view; //camera lookAt
        protected Matrix projection; //projects geometry onto a view plane.

        protected float nearPlane;
        protected float farPlane;
        protected float aspectRatio; 

        public Vector3 Pos
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        public float NearPlane
        {
            get
            {
                return nearPlane;
            }
            set
            {
                nearPlane = value;
            }
        }

        public float FarPlane
        {
            get
            {
                return farPlane;
            }
            set
            {
                farPlane = value;
            }
        }

        public float AspectRatio
        {
            get
            {
                return aspectRatio;
            }
            set
            {
                aspectRatio = value;
            }
        }

        public abstract Matrix View
        {
            get;
        }

        public abstract Matrix Projection
        {
            get;
        }
    }
}

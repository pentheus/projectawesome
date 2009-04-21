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
    public class ChaseCamera : Camera
    {
        protected Vector3 lookAt;

        public ChaseCamera()
        {
            pos = Vector3.Zero;
            rotation = Vector3.Zero;
            aspectRatio = 4.0f / 3.0f;
            nearPlane = 0.1f;
            farPlane = 1000.0f;
            lookAt = Vector3.Zero;
        }

        /// <summary>
        /// A constructor for the camera
        /// </summary>
        /// <param name="pos">A vector containing the (x,y,z) coordinates of the camera</param>
        /// <param name="rotation">A vector which expresses how much the camera is rotated on the (x,y,z) axes</param>
        /// <param name="aspectRatio"></param>
        /// <param name="near">Distance of the near plane from the camera</param>
        /// <param name="far">Distance of the far plane from the camera</param>
        public ChaseCamera(Vector3 pos, Vector3 lookAt, float aspectRatio, float near, float far)
        {
            this.pos = pos;
            this.rotation = Vector3.Zero;
            this.aspectRatio = aspectRatio;
            this.nearPlane = near;
            this.farPlane = far;
            this.lookAt = lookAt;
        }

        public override Matrix View
        {
            get
            {
                view = Matrix.CreateLookAt(pos, lookAt, Vector3.UnitY);
                return view;
            }
        }

        public override Matrix Projection
        {
            get
            {
                projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, nearPlane, farPlane);
                return projection;
            }
        }
    }
}

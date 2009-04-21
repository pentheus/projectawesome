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
    public class RegCamera : Camera
    {
        public RegCamera()
        {
            pos = Vector3.Zero;
            rotation = Vector3.Zero;
            aspectRatio = 4.0f / 3.0f;
            nearPlane = 0.1f;
            farPlane = 1000.0f;
        }

        /// <summary>
        /// A constructor for the camera
        /// </summary>
        /// <param name="pos">A vector containing the (x,y,z) coordinates of the camera</param>
        /// <param name="rotation">A vector which expresses how much the camera is rotated on the (x,y,z) axes</param>
        /// <param name="aspectRatio"></param>
        /// <param name="near">Distance of the near plane from the camera</param>
        /// <param name="far">Distance of the far plane from the camera</param>
        public RegCamera(Vector3 pos, Vector3 rotation, float aspectRatio, float near, float far)
        {
            this.pos = pos;
            this.rotation = rotation;
            this.aspectRatio = aspectRatio;
            this.nearPlane = near;
            this.farPlane = far;
        }

        public override Matrix View
        {
            get
            {
                view = Matrix.CreateTranslation(-pos) * Matrix.CreateRotationX(rotation.X) *
                       Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z);
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

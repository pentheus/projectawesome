using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace AwesomeEngine.Camera
{
    public class OrthoCamera : Camera
    {
        float xmin;
        float xmax;
        float ymin;
        float ymax;

        public OrthoCamera()
        {
            pos = Vector3.Zero;
            rotation = Vector3.Zero;
            xmin = -400.0f;
            xmax = 400.0f;
            ymin = -300.0f;
            ymax = 300.0f;
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
        public OrthoCamera(Vector3 pos, Vector3 rotation, float width, float height, float near, float far)
        {
            this.pos = pos;
            this.rotation = rotation;
            this.xmin = -(width / 2.0f);
            this.xmax = width / 2.0f;
            this.ymin = -(height / 2.0f);
            this.ymax = height / 2.0f; 
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
                projection = Matrix.CreateOrthographicOffCenter(xmin, xmax, ymin, ymax, nearPlane, farPlane);
                return projection;
            }
        }
    }
}

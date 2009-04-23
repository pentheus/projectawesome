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
    public class Light
    {
        Vector3 pos;
        Vector3 lightTarget;
        float lightFar;
        
        public Light(Vector3 pos, Vector3 lightTarget, float lightFar)
        {
            this.pos = pos;
            this.lightTarget = lightTarget;
            this.lightFar = lightFar;
        }

        public Vector3 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public Vector3 LightTarget
        {
            get { return lightTarget; }
            set { lightTarget = value; }
        }

        public float LightFar
        {
            get { return lightFar; }
            set { lightFar = value; }
        }

        public Matrix ViewMatrix
        {
            get { return Matrix.CreateLookAt(pos, lightTarget, Vector3.Up); }
        }
    }
}

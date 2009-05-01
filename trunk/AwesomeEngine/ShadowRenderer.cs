//This ShadowRenderer class is heavily based off of
//Matt Pettineo's Deferred Shadow Rendering tutorial
//So everybody bow down to him for his mighty brain.


using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using AwesomeEngine.Camera;

namespace AwesomeEngine
{
    class ShadowRenderer
    {
        Effect shadowEffect;

        public ShadowRenderer(Effect effect)
        {
            shadowEffect = effect;
        }

        public void CreateShadowMap(ModelInfo model)
        {
        }
    }
}

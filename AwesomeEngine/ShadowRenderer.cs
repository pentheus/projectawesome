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
        DepthStencilBuffer dsBuffer;
        DepthStencilBuffer oldDS;
        RenderTarget2D shadowMap;
        Effect shadowMapEffect;
        RenderTarget2D shadowOcclusion;
        RenderTarget2D disabledShadowOcclusion;

        ContentManager contentManager;
        GraphicsDevice graphicsDevice;

        FullScreenQuad fullScreenQuad;

        Vector3[] frustCornersViewspace = new Vector3[8];
        Vector3[] frustCornersWorldspace = new Vector3[8];
        Vector3[] frustCornersLightspace = new Vector3[8];
        Vector3[] farFrustCornersViewspace = new Vector3[8];

        OrthoCamera lightCamera;

    }
}

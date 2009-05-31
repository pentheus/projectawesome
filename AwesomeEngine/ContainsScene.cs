﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AwesomeEngine.Camera;

namespace AwesomeEngine
{
    public interface ContainsScene
    {
        SceneManager GetScene();
        GraphicsDevice GetGraphics();
        ContentManager GetContent();
        ThirdPersonCamera GetCamera();
        Player GetPlayer();
    }
}
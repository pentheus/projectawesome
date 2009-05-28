using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeEngine
{
    interface ContainsScene
    {
        public SceneManager GetScene();

        public GraphicsDevice GetGraphics();

        public ContentManager GetContent();
    }
}

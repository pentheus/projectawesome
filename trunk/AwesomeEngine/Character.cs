using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine
{
    public class Character : GameComponent
    {
        ModelInfo model;
        SceneManager scene;

        public Character(Game game, SceneManager scene, ModelInfo model)
            : base(game)
        {
            this.scene = scene;
            this.model = model;
        }

        public ModelInfo Model
        {
            get { return model; }
            set { model = value; }
        }
    }
}
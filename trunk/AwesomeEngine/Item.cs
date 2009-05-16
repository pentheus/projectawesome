using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine
{
    class Item:GameComponent
    {
        ModelInfo model;

        public Item(Game game, ModelInfo model):base(game)
        {
            this.model = model;
        }

        public ModelInfo Model
        {
            get { return model; }
            set { model = value; }
        }

        public void runScript()
        {

        }
    }
}
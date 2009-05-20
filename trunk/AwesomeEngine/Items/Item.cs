using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine
{
    public class Item:GameComponent
    {
        ModelInfo model;
        bool movable, pickable, picked;

        public Item(Game game, ModelInfo model) // initializing movable, pickable, picked as false
            : base(game)
        {
            this.model = model;
            movable = false;
            pickable = false;
            picked = false;
        }

        public ModelInfo Model
        {
            get { return model; }
            set { model = value; }
        }

        public void setMovable(bool m)
        {
            movable = m;
        }

        public void setPickable(bool p)
        {
            pickable = p;
        }

        public void runScript()
        {
            //if bounding box of item intersects bounding box of character, pick up/move if movable is true
        }
    }
}

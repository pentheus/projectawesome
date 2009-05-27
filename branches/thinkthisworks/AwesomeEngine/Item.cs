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
        bool movable, picked; // movable means that an item can be moved/picked up; picked means that the item has been picked up

        public Item(Game game, ModelInfo model):base(game)
        {
            this.model = model;
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

        public void runScript()
        {
            //if bounding box of item intersects bounding box of character, pick up/move if movable is true
        }
    }
}

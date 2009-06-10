using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine.Items
{
    public class FuseItem:Item
    {
        public FuseItem(Game game, ModelInfo model) :
            base(game, model)
        {
            this.setPickable(true);
        }

        
        public override void runScript()
        {
            this.Player.Pickup(this);
            Game.Components.Remove(this);
            this.picked = true;
        }
    }
}

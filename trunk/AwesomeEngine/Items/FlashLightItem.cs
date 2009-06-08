using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine.Items
{
    public class FlashLightItem : Item
    {
        private int dps, battLife;

        public FlashLightItem(Game game, ModelInfo model) :
            base(game, model)
        {
            this.setPickable(true);
            dps = 3; // medium damage per second
            battLife = 10; // initialized flashlight battery life to 10
        }

        // scripts
        // if not picked up, if bounding spheres intersect, pick up
        // if picked up, turn on, turn off
        public override void runScript()
        {
            this.Player.Pickup(this);
            this.picked = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine.Items
{
    public class GlowStickItem:Item
    {
        private int dps;
        public GlowStickItem(Game game, ModelInfo model) :
            base(game, model)
        {
            this.setPickable(true);
            dps = 1; // very low damage per second
        }

        // scripts
        // 1) pick up if bounding spheres intersect
        // 2) throw
        // 
        public override void runScript()
        {
            if (picked)
            {
            }
            else // if not picked
            {
            }
        }
    }
}

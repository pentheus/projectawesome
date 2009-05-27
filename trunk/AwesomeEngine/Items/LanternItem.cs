using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine.Items
{
    public class LanternItem : Item
    {
        private int dps, battLife;

        public LanternItem(Game game, ModelInfo model) :
            base(game, model)
        {
            this.setPickable(true);
            dps = 6; // strong damage per second
            battLife = 10;
        }

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

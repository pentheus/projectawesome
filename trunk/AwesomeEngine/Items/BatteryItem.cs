using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine.Items
{
    public class BatteryItem:Item
    {
        private int battLife;

        public BatteryItem(Game game, ModelInfo model)
            : base(game, model)
        {
            this.setPickable(true);
            battLife = 10; //intialized battery life to 10
        }

        public int BatteryLife
        {
            get { return battLife; }
        }

        // scripts
        // pick up and add value to current item if not full
        public override void runScript()
        {
            if (this.picked == false)
            {
                this.Player.Pickup(this);
                this.picked = true;
            }
        }
    }
}

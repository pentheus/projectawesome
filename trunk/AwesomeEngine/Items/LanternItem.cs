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
        public LanternItem(Game game, ModelInfo model) :
            base(game, model)
        {
            this.setPickable(true);
        }

        public void runScript()
        {

        }
    }
}

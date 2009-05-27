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

        // two scripts
        // 1) if not picked up yet, if bounding sphere of character intersects with bounding sphere of fuse, pick it up
        // 2) if picked up already, if bounding sphere of character intersects with bounding sphere of fuse box, put fuse in
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

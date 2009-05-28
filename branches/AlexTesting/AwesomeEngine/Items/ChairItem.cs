using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine.Items
{
    public class ChairItem : Item
    {
        public ChairItem(Game game, ModelInfo model):base(game, model)
            // this class is for the chair the character needs to get to the attic access
        {
            this.setMovable(true);
        }
    }
}

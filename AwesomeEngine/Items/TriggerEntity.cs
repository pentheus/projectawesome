using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeEngine
{
    public class TriggerEntity : LogicEntity
    {
        public TriggerEntity(Game game, Model model, Vector3 position): 
            base(game, model,position)
        {
            
        }
    }
}

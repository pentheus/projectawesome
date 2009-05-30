using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeEngine
{
    public class SpawnEntity:LogicEntity
    {
        public SpawnEntity(Game game, Model model, Vector3 position, Vector3 scale) : 
            base(game, model,position,scale)
        {
            
        }
    }
}

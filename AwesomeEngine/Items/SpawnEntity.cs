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
        private const int cooldown = 3000;
        private int timer = cooldown;
        private bool isSpawnAlive = false;

        public SpawnEntity(Game game, Model model, Vector3 position) : 
            base(game, model, position)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if ((Game as ContainsScene).GetPlayer != null)
            {
                if (timer <= 3000 && !isSpawnAlive)
                    Spawn();
                timer += gameTime.ElapsedGameTime.Milliseconds;
            }

            
            base.Update(gameTime);
        }

        public void Spawn()
        {
            this.isSpawnAlive = true;
            this.timer -= cooldown;
        }

        public bool IsAlive
        {
            get { return isSpawnAlive; }
            set { isSpawnAlive = value; }
        }
    }
}

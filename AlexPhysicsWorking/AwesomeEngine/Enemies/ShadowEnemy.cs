using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine.Enemies
{
    public class ShadowEnemy:Enemy
    {
        Vector3 playerPOS;

        public ShadowEnemy(Game game, SceneManager scene, AnimModelInfo model): 
            base(game, scene, model)
        {

        }

        public override void ActIdle()
        {
            
        }
        public override void ActSeeking()
        {
            //if (this.enemyAOE.Intersects(playershadow enemy's second bounding sphere (or whatever mesh) is intersected by the player
            {
                this.MoveTowards(playerPOS);
            }
        }
        public override void ActAttacking()
        {

        }
        public override void ActDamaged()
        {
            // if shadow enemy's 
        }

    }
}

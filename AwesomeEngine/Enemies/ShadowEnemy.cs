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
        public ShadowEnemy(Game game, SceneManager scene, AnimModelInfo model): 
            base(game, scene, model)
        {
            this.updateSeekingSphere(30); // updated the seeking bounding sphere's radius to 30 units
            this.updateAttackingSphere(8); // updated the attacking bounding sphere's radius to 8 units
        }

        public override void ActIdle()
        {
            if (this.enemyAttackingSphere.Intersects(this.enemyAttackingSphere))
            {
                this.State = state.Attacking;
            }

            else if (this.enemySeekingSphere.Intersects(this.Player.BoundingSphere))
            {
                this.State = state.Seeking;
            }
        }
        public override void ActSeeking()
        {
            if (this.enemyAttackingSphere.Intersects(this.enemyAttackingSphere))
            {
                this.State = state.Attacking;
            }

            else if (this.enemySeekingSphere.Intersects(this.Player.BoundingSphere))
            {
                this.MoveTowards(this.Player.Position);
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

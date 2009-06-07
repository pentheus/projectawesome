using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;


namespace AwesomeEngine.Enemies
{
    public class FirstBossEnemy:Enemy
    {

        public FirstBossEnemy(Game game, SceneManager scene, AnimModelInfo model): 
            base(game, scene, model)
        {
            this.updateSeekingSphere(50);
            this.updateAttackingSphere(11);
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
            if (this.enemyAttackingSphere.Intersects(this.Player.BoundingSphere))
            {
                this.State = state.Attacking;
            }

            else if (this.enemySeekingSphere.Intersects(this.Player.BoundingSphere))
            {
                this.MoveTowards(this.Player.Position);
            }
        }
        public override void ActAttacking(GameTime gameTime)
        {
            if (this.enemyAttackingSphere.Intersects(this.Player.BoundingSphere))
            {
                Attack();
            }

            else if (this.enemySeekingSphere.Intersects(this.Player.BoundingSphere))
            {
                this.State = state.Seeking;
            }
        }
        public override void ActDamaged()
        {
            // if shadow enemy's 
        }

        public override void Attack()
        {
            this.Player.TakeBossDamage();
        }
    }
}

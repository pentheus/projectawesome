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
        private int accumulator, currentTime, cooldown;
        private int hp = 10;
        private SpawnEntity spawnPoint;
        private int damagetimer = 0;

        public ShadowEnemy(Game game, SceneManager scene, AnimModelInfo model):
        //public ShadowEnemy(Game game, SceneManager scene, AnimModelInfo model): 
            base(game, scene, model)
        {
            this.updateSeekingSphere(120); // updated the seeking bounding sphere's radius to 30 units
            this.updateAttackingSphere(20); // updated the attacking bounding sphere's radius to 8 units
            accumulator = 0;
            cooldown = 2000;
        }

        public override void ActIdle()
        {
            if (this.enemyAttackingSphere.Contains(this.Player.Position) == ContainmentType.Contains)
            {
                this.State = state.Attacking;
            }

            else if (this.enemySeekingSphere.Contains(this.Player.Position) == ContainmentType.Contains)
            {
                this.State = state.Seeking;
            }
        }
        public override void ActSeeking()
        {
            if (this.enemyAttackingSphere.Contains(this.Player.Position) == ContainmentType.Contains)
            {
                this.State = state.Attacking;
            }

            else if (this.enemySeekingSphere.Contains(this.Player.Position) == ContainmentType.Contains)
            {
                this.MoveTowards(this.Player.Position);
            }

            else // if it is not in either the attacking sphere or the seeking sphere
            {
                this.State = state.Idle;
            }
        }
        public override void ActAttacking(GameTime gameTime)
        {
            accumulator += gameTime.ElapsedGameTime.Milliseconds;
            Console.WriteLine(accumulator + " Accumulator");
            if (this.enemyAttackingSphere.Contains(this.Player.Position) == ContainmentType.Contains)
            {
                if (accumulator >= cooldown)
                {
                    Attack();
                    Model.animateModel("Attack");
                    accumulator -= cooldown;
                }
            }

            else if (this.enemySeekingSphere.Contains(this.Player.Position) == ContainmentType.Contains)
            {
                this.State = state.Seeking;
                Model.animateModel("Walk");
            }

            else
            {
                this.State = state.Idle;
            }
        }
        public override void ActDamaged()
        {
            if (damagetimer == 0)
            {
                SetState(state.Idle);
                Model.animateModel("Idle");
                return;
            }

            if (hp <= 0)
            {
                spawnPoint.IsAlive = false;
                Game.Components.Remove(this);
            }

            damagetimer--;
            
        }

        public override void Attack()
        {
            this.Player.TakeRegularDamage();
        }

        public new void TakeDamage(int damage)
        {
            damagetimer = 1000;
            SetState(state.Damaged);
            Model.animateModel("Damage");
            base.TakeDamage(damage);
        }

        public SpawnEntity SpawnPoint
        {
            get { return spawnPoint; }
            set { spawnPoint = value; }
        }

    }
}

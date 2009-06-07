using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine
{
    public abstract class Enemy:GameComponent
    {
        public enum state { Idle, Seeking, Attacking, Damaged };
        public BoundingSphere enemySeekingSphere, enemyAttackingSphere;
        private int seekRadius, attackRadius, health;

        ModelInfo model;
        //AnimModelInfo model;
        SceneManager scene;
        state currentstate;
        ContainsScene afgame;
        Player player;

        public Enemy(Game game, SceneManager scene, ModelInfo model): base(game)
        //public Enemy(Game game, SceneManager scene, AnimModelInfo model): base(game)
        {
            afgame = (ContainsScene)game;
            player = afgame.GetPlayer();
            this.scene = scene;
            this.model = model;
            currentstate = state.Idle;
            seekRadius = 0; // initialize radius in each of the enemy classes
            attackRadius = 0;
            enemySeekingSphere = new BoundingSphere(this.model.Position, seekRadius);
            enemyAttackingSphere = new BoundingSphere(this.model.Position, attackRadius);
            health = 100;
        }

        public void TakeDamage()
        {
            health -= 5;
        }

        public abstract void Attack();

        public state State
        {
            get { return currentstate; }
            set { currentstate = value; }
        }

        public ContainsScene AFgame
        {
            get { return afgame; }
        }

        public Player Player
        {
            get { return player; }
        }

        public void updateSeekingSphere(int rad)
        {
            seekRadius = rad;
            enemySeekingSphere = new BoundingSphere(model.Position, seekRadius);
        }

        public void updateAttackingSphere(int rad)
        {
            attackRadius = rad;
            enemyAttackingSphere = new BoundingSphere(model.Position, attackRadius);
        }

        public void updateSpheres()
        {
            enemySeekingSphere.Center = model.Position;
            enemyAttackingSphere.Center = model.Position;
        }

        public ModelInfo Model
        //public AnimModelInfo Model
        {
            get { return model; }
            set { model = value; }
        }

        public override void Update(GameTime gameTime)
        {
            switch (currentstate)
            {
                case state.Idle:
                    ActIdle();
                    Console.WriteLine("Idling");
                    break;
                case state.Seeking:
                    ActSeeking();
                    Console.WriteLine("Seeking");
                    break;
                case state.Attacking:
                    ActAttacking(gameTime);
                    Console.WriteLine("Attacking");
                    break;
                case state.Damaged:
                    ActDamaged();
                    Console.WriteLine("Taking Damage");
                    break;
            }
            Console.WriteLine(player.Health + " - Health");
            updateSpheres();
        }

        public void MoveTowards(Vector3 pos)
        {
            float opposite, adjacent, hypotenuse, angle;
            Matrix rotation;
            Vector3 moveVector;

            opposite = player.Position.Z - model.Position.Z;
            adjacent = player.Position.X - model.Position.X;
            hypotenuse = (float) Math.Sqrt((opposite * opposite) + (adjacent * adjacent));
            angle = (float) Math.Asin(opposite / hypotenuse);

            Console.WriteLine(angle + "angle");
            rotation = Matrix.CreateRotationY(MathHelper.ToRadians(angle));
            if (angle + (float)Math.PI / 2 > 0 && angle + (float)Math.PI / 2 <= Math.PI)
            {
                model.Rotation = new Vector3(0, angle + (float)Math.PI / 2, 0);
            }
            moveVector = new Vector3((float)(player.Position.X-model.Position.X)/450, 0, (float)(player.Position.Z-model.Position.Z)/450);
            model.Position = model.Position + moveVector;
            updateSpheres();
        }

        public float CalculateNewRotation(Vector3 pos)
        {
            float enemyX = model.Position.X;
            float enemyZ = model.Position.Z;
            float playerX = pos.X;
            float playerZ = pos.Z;
            float adjacent, opposite; // talking about sides of a triangle here
            float angle; // angle to be subtracted
            float quadAngle; // angle to subtract from
            int quadrant = 0;
            if (enemyX == playerX && enemyZ == playerZ) // if somehow, the player and the enemy occupy the same space
            {
                return model.Rotation.Y;
            }
            else
            {
                if (enemyX <= playerX && enemyZ <= playerZ) // first quadrant
                {
                    quadrant = 1;
                    adjacent = playerZ - enemyZ;
                    opposite = playerX - enemyX;
                    quadAngle = 90;
                }
                else if (enemyX >= playerX && enemyZ <= playerZ) // second quadrant
                {
                    quadrant = 2;
                    adjacent = enemyX - playerX;
                    opposite = playerZ - enemyZ;
                    quadAngle = 180;
                }
                else if (enemyX >= playerX && enemyZ >= playerZ) // third quadrant
                {
                    quadrant = 3;
                    adjacent = enemyZ - playerZ;
                    opposite = enemyX - playerX;
                    quadAngle = 270;
                }
                else //if (enemyX <= playerX && enemyZ >= playerZ) // fourth quadrant
                {
                    quadrant = 4;
                    adjacent = playerX - enemyX;
                    opposite = enemyZ - playerZ;
                    quadAngle = 360;
                }

                angle = (float)Math.Atan(opposite / adjacent);

                return (quadAngle - angle);
            }
            

        }

        public void SetState(state newState)
        {
            currentstate = newState;
        }

        public abstract void ActIdle();
        public abstract void ActSeeking();
        public abstract void ActAttacking(GameTime g);
        public abstract void ActDamaged();

        public BoundingSphere SeekingBoundingSphere
        {
            
            get { return enemySeekingSphere.Transform(WorldMatrix); }
        }

        public BoundingSphere AttackingBoundingSphere
        {
            get { return enemyAttackingSphere.Transform(WorldMatrix); }
        }

        public Matrix WorldMatrix
        {
            get
            {
                return (Matrix.CreateRotationX(MathHelper.ToRadians(model.Rotation.X)) *
                    Matrix.CreateRotationY(model.Rotation.Y) * Matrix.CreateRotationZ(MathHelper.ToRadians(model.Rotation.Z)) *
                    Matrix.CreateScale(model.Scale.X, model.Scale.Y, model.Scale.Z) * Matrix.CreateTranslation(model.Position));
            }
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine
{
    public abstract class Enemy:GameComponent
    {
        enum state { Idle, Seeking, Attacking, Damaged };

        AnimModelInfo model;
        SceneManager scene;
        state currentstate;

        public Enemy(Game game, SceneManager scene, AnimModelInfo model): base(game)
        {
            this.scene = scene;
            this.model = model;
            currentstate = state.Idle;
        }

        public AnimModelInfo Model
        {
            get { return model; }
            set { model = value; }
        }

        public void Update()
        {
            switch (currentstate)
            {
                case state.Idle:
                    ActIdle();
                    break;
                case state.Seeking:
                    ActSeeking();
                    break;
                case state.Attacking:
                    ActAttacking();
                    break;
                case state.Damaged:
                    ActDamaged();
                    break;
            }

        }

        public void MoveTowards(Vector3 pos)
        {
            float newRotation = CalculateNewRotation(pos);
            model.Position = Vector3.Transform(Model.Position, 
                Matrix.CreateTranslation(model.Position) * 
                Matrix.CreateRotationY(MathHelper.ToRadians(newRotation)) * 
                Matrix.CreateTranslation(new Vector3(1f, 0, 0)));
       
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
                if (enemyX <= playerX && enemyY <= playerZ) // first quadrant
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

                angle = Math.Atan(opposite / adjacent);

                return (quadAngle - angle);
            }
            

        }

        public void SetState(state newState)
        {
            currentstate = newState;
        }

        public abstract void ActIdle();
        public abstract void ActSeeking();
        public abstract void ActAttacking();
        public abstract void ActDamaged();
    }
}

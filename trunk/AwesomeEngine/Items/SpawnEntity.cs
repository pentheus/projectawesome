using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AwesomeEngine.Enemies;
using XNAnimation;

namespace AwesomeEngine
{
    public class SpawnEntity:LogicEntity
    {
        private const int cooldown = 1000;
        private int timer = cooldown;
        private bool isSpawnAlive = false;
        Model spawnmodel;
        ShadowEnemy shadowEnemy;
        AnimModelInfo enemyModel;

        public SpawnEntity(Game game, Model model, Vector3 position, SkinnedModel evilmodel) : 
            base(game, model, position)
        {
            spawnmodel = model;
            enemyModel = new AnimModelInfo(position, Vector3.Zero, Vector3.One, evilmodel, "", Game);
            shadowEnemy = new ShadowEnemy(Game, ((ContainsScene)Game).GetScene(), enemyModel);
        }

        public override void Update(GameTime gameTime)
        {
            if ((Game as ContainsScene).GetPlayer() != null)
            {
                if ((Game as ContainsScene).GetPlayer().BoundingSphere.Contains(this.Position) == ContainmentType.Intersects)
                    Game.Components.Remove(this);
                if (timer <= 1000 && !isSpawnAlive)
                    Spawn();
                timer += gameTime.ElapsedGameTime.Milliseconds;
            }
            base.Update(gameTime);
        }

        public void Spawn()
        {
            Console.WriteLine("Spawned an enemy.");
            this.isSpawnAlive = true;
            this.timer -= cooldown;
            shadowEnemy.HP = 10;
            shadowEnemy.Model.Position = new Vector3(this.Position.X, this.Position.Y + 20, this.Position.Z);
            shadowEnemy.SpawnPoint = this;
            ((ContainsScene)Game).GetScene().SceneGraph.GetEnemies().Add(shadowEnemy);
            shadowEnemy.Initialize();
            Game.Components.Add(shadowEnemy);
        }

        public bool IsAlive
        {
            get { return isSpawnAlive; }
            set { isSpawnAlive = value; }
        }
    }
}
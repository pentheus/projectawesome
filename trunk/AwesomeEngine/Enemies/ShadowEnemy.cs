using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using XNAnimation;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeEngine.Enemies
{
    public class ShadowEnemy:Enemy
    {
        private int accumulator, currentTime, cooldown;
        private int hp = 10;
        private SpawnEntity spawnPoint;
        private int damagetimer = 0;

        AnimModelInfo enemyModelInfo;
        SkinnedModel enemySkinnedModel;
        Texture2D enemyTexture;
        Effect effect;

        public ShadowEnemy(Game game, SceneManager scene, AnimModelInfo model):
        //public ShadowEnemy(Game game, SceneManager scene, AnimModelInfo model): 
            base(game, scene, model)
        {
            this.updateSeekingSphere(220); // updated the seeking bounding sphere's radius to 30 units
            this.updateAttackingSphere(20); // updated the attacking bounding sphere's radius to 8 units
            accumulator = 0;
            cooldown = 2000;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void  LoadContent()
        {
            effect = Game.Content.Load<Effect>("Simple");
            enemySkinnedModel = LoadSkinnedModel("shadowmonster");
            enemyTexture = Game.Content.Load<Texture2D>(".\\Textures\\shadowmonster_Plane");
            enemyModelInfo = new AnimModelInfo(this.model.Position, Vector3.Zero, new Vector3(1), enemySkinnedModel, "shadowmonster", Game);
            //enemyModelInfo.Body.EnableBody();
            this.Model = enemyModelInfo;
 	        base.LoadContent();
        }

        public SkinnedModel LoadSkinnedModel(String assetName)
        {
            SkinnedModel model = Game.Content.Load<SkinnedModel>(@"Models\" + assetName);

            foreach (ModelMesh mesh in model.Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect e = effect.Clone(Game.GraphicsDevice);
                    part.Effect = e;
                }
            return model;
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix[] modelTransforms = new Matrix[model.Model.Bones.Count];
            model.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Vector3 center = model.Position;

            foreach (ModelMesh mesh in model.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect.Parameters["xTexture"].SetValue(enemyTexture);
                    part.Effect.Parameters["matBones"].SetValue(model.AnimationController.SkinnedBoneTransforms);
                    part.Effect.Parameters["xTextureEnabled"].SetValue(true);
                }
                foreach (Effect e in mesh.Effects)
                {
                    e.CurrentTechnique = ((ContainsScene)Game).GetScene().Effect.Techniques["AnimatedLambertTest"];
                    e.Parameters["xWorld"].SetValue(Matrix.CreateRotationY(MathHelper.ToRadians(this.model.Rotation.Y)) * Matrix.CreateTranslation(this.Model.Body.Position));
                    e.Parameters["xView"].SetValue(((ContainsScene)Game).GetCamera().View);
                    e.Parameters["xProjection"].SetValue(((ContainsScene)Game).GetCamera().Projection);
                    e.Parameters["xCenter"].SetValue(this.Model.Body.Position);
                    e.Parameters["xRange"].SetValue(4f);


                    //effect.World = modelTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(playerRotation)
                    //    * Matrix.CreateTranslation(playerPosition);
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
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
                Model.animateModel("Walk");
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
                Model.animateModel("Idle");
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
                Model.animateModel("Idle");
            }
        }
        public override void ActDamaged()
        {
            if (damagetimer <= 0)
            {
                Console.WriteLine("Remaining Health: " + hp);
                SetState(state.Idle);
                Model.animateModel("Idle");
                return;
            }

            if (hp <= 0)
            {
                Console.WriteLine("I died!");
                if(spawnPoint != null)
                    spawnPoint.IsAlive = false;
                Game.Components.Remove(this);
            }

            damagetimer--;
            
        }

        public override void Attack()
        {
            this.Player.TakeRegularDamage();
        }

        public override void TakeDamage(int damage)
        {
            damagetimer = 120;
            hp -= damage;
            SetState(state.Damaged);
            Model.animateModel("Damage");
        }

        public SpawnEntity SpawnPoint
        {
            get { return spawnPoint; }
            set { spawnPoint = value; }
        }

        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

    }
}

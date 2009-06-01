using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using AwesomeEngine;
using AwesomeEngine.Items;
using Microsoft.Xna.Framework.Graphics;
using XNAnimation;
using Microsoft.Xna.Framework.Input;

namespace AwesomeEngine
{
    public class Player : DrawableGameComponent
    {
        ContainsScene game;
        AnimModelInfo model;
        BoundingSphere boundary;
        int health;
        List<Item> inventory;
        FlashLightItem flashlight;
        Item currentitem;

        Vector3 playerPosition = Vector3.Zero;
        float playerRotation = 0.0f;
        Vector3 playerVelocity = Vector3.Zero;

        Effect drawModelEffect;

        public Player(Game game) :
            base(game)
        {
            this.game = (ContainsScene)game;
            health = 3;
            inventory = new List<Item>();
            flashlight = null;
        }

        public Player(Game game, List<Item> inv, FlashLightItem light) :
            base(game)
        {
            this.game = (ContainsScene)game;
            health = 3;
            inventory = inv;
            flashlight = light;
        }

        protected override void LoadContent()
        {
            SkinnedModel playermodel = new SkinnedModel();
            ModelInfo.LoadModel(ref playermodel, game.GetScene().Textures, game.GetContent(), game.GetGraphics(), "PlayerMarine_mdla", game.GetScene().Effect);
            model = new AnimModelInfo(Vector3.Zero, Vector3.Zero, new Vector3(1f), playermodel, "PlayerMarine_mdla");

            //Load shaders
            drawModelEffect = game.GetContent().Load<Effect>("Simple");
        }

        public override void Update(GameTime gameTime)
        {
            model.AnimationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
            KeyboardState currentState = Keyboard.GetState(PlayerIndex.One);

            // Rotate the model using the left thumbstick, and scale it down.
            if (currentState.IsKeyDown(Keys.L))
            {
                playerRotation -= 0.075f;
            }
            if (currentState.IsKeyDown(Keys.J))
            {
                playerRotation += 0.075f;
            }


            // Create some velocity if the right trigger is down.
            Vector3 modelVelocityAdd = Vector3.Zero;

            // Find out what direction we should be thrusting, using rotation.
            modelVelocityAdd.X = -(float)Math.Sin(playerRotation);
            modelVelocityAdd.Z = -(float)Math.Cos(playerRotation);

            // Now scale our direction by how hard the trigger is down.

            bool add = false;

            if (currentState.IsKeyDown(Keys.K))
            {
                modelVelocityAdd *= .5f;
                add = true;
                model.animateModel("Run");
            }

            if (currentState.IsKeyDown(Keys.I))
            {
                modelVelocityAdd *= -.5f;
                add = true;
                model.animateModel("Run");
            }

            if (!add)
            {
                modelVelocityAdd *= .00f;
                model.animateModel("Idle");
            }

            // Finally, add this vector to our velocity.
            playerVelocity += modelVelocityAdd;

            if (currentState.IsKeyDown(Keys.Home))
            {
                playerPosition = Vector3.Zero;
                playerVelocity = Vector3.Zero;
                playerRotation = 0.0f;
            }

            playerPosition += playerVelocity;
            playerVelocity *= 0f;

            model.AnimationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);

            base.Update(gameTime);
        }

        public void Draw()
        {
            SkinnedModel skinnedModel = model.AnimatedModel;
            Matrix[] modelTransforms = new Matrix[model.Model.Bones.Count];// = model.AnimationController.SkinnedBoneTransforms;
            modelTransforms = model.AnimationController.SkinnedBoneTransforms;
            Vector3 center = model.Position;

            foreach (ModelMesh mesh in skinnedModel.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    //May be more efficient in future to store player textures in new dictionary and load them from there
                    //Would require new LoadModel function somewhere in this namespace
                    part.Effect.Parameters["xTexture"].SetValue(game.GetScene().Textures[part]);
                    part.Effect.Parameters["matBones"].SetValue(model.AnimationController.SkinnedBoneTransforms);
                    part.Effect.Parameters["xTextureEnabled"].SetValue(true);
                }
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = drawModelEffect.Techniques["AnimatedLambertTest"];
                    effect.Parameters["xWorld"].SetValue(Matrix.CreateRotationY(playerRotation) * Matrix.CreateTranslation(playerPosition));
                    effect.Parameters["xView"].SetValue(game.GetCamera().View);
                    effect.Parameters["xProjection"].SetValue(game.GetCamera().Projection);
                    effect.Parameters["xCenter"].SetValue(model.Position);
                    effect.Parameters["xRange"].SetValue(4f);


                    //effect.World = modelTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(playerRotation)
                    //    * Matrix.CreateTranslation(playerPosition);
                }
                mesh.Draw();
            }
        }
    }
}

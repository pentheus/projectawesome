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

        public enum state { Idle, Running, Attacking, Damaged };
        ContainsScene game;
        AnimModelInfo model;
        BoundingSphere boundary;
        int health;
        List<Item> inventory;
        FlashLightItem flashlight;
        Item currentitem;
        state currentplayerstate;

        Vector3 playerPosition = Vector3.Zero;
        Vector3 playerVelocity = Vector3.Zero;
        float playerRotation = 0.0f;

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
            model = new AnimModelInfo(Vector3.Zero, Vector3.Zero, new Vector3(1f), playermodel, "PlayerMarine_mdla", (Game)this.game);

            //Load shaders
            drawModelEffect = game.GetContent().Load<Effect>("Simple");
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState(PlayerIndex.One);

            // Rotate the model using the left thumbstick, and scale it down.
            if (currentState.IsKeyDown(Keys.L))
            {
                playerRotation -= 3f;
            }
            if (currentState.IsKeyDown(Keys.J))
            {
                playerRotation += 3f;
            }

            // Now scale our direction by how hard the trigger is down.

            bool isIdle = true;

            state oldstate = currentplayerstate;

            if (currentState.IsKeyDown(Keys.K))
            {
                playerVelocity = new Vector3(0, 0, -1);
                isIdle = false;
                currentplayerstate = state.Running;
            }

            if (currentState.IsKeyDown(Keys.I))
            {
                playerVelocity = new Vector3(0, 0, 1);
                isIdle = false;
                currentplayerstate = state.Running;
            }

            if (isIdle)
            {
                playerVelocity = Vector3.Zero;
                currentplayerstate = state.Idle;
            }

            //Start animations if necessary
            if (oldstate != currentplayerstate)
            {
                if (currentplayerstate == state.Running)
                    model.animateModel("Run");
                else if (currentplayerstate == state.Idle)
                    model.animateModel("Idle");
            }

            if (currentState.IsKeyDown(Keys.Home))
            {
                playerPosition = Vector3.Zero;
                playerVelocity = Vector3.Zero;
                playerRotation = 0.0f;
            }

            playerPosition += Vector3.Transform(playerVelocity, Matrix.CreateRotationY(MathHelper.ToRadians(playerRotation)));
            model.AnimationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
            model.Position = playerPosition;
            model.Rotation = new Vector3(0, playerRotation, 0);
            base.Update(gameTime);
        }

        public void Draw()
        {
            Matrix[] modelTransforms = new Matrix[model.Model.Bones.Count];
            model.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Vector3 center = model.Position;

            foreach (ModelMesh mesh in model.Model.Meshes)
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
                    effect.Parameters["xWorld"].SetValue(Matrix.CreateRotationY(MathHelper.ToRadians(playerRotation)) * Matrix.CreateTranslation(playerPosition));
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

        public Vector3 Position
        {
            get { return playerPosition; }
        }
    }
}

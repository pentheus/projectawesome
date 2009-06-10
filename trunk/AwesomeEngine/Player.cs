﻿using System;
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
using JigLibX;
using JigLibX.Physics;

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
        FuseItem fuse;
        Boolean hasFlashLight, hasFuse;
        Item currentitem;
        state currentplayerstate;

        Vector3 playerPosition = new Vector3(0, 9, 0);
        Vector3 playerVelocity = Vector3.Zero;
        float playerRotation = 0.0f;

        Effect drawModelEffect;

        public Player(Game game) :
            base(game)
        {
            this.game = (ContainsScene)game;
            health = 230;
            inventory = new List<Item>();
            flashlight = null;
            fuse = null;
            hasFlashLight = false;
            hasFuse = false;
        }

        public Player(Game game, List<Item> inv, FlashLightItem light) :
            base(game)
        {
            this.game = (ContainsScene)game;
            health = 230;
            inventory = inv;
            flashlight = light;
        }

        public void Pickup(FlashLightItem fl)
        {
            flashlight = fl;
            hasFlashLight = true;
        }

        public void Pickup(FuseItem fi)
        {
            fuse = fi;
            hasFuse = true;
        }

        protected override void LoadContent()
        {
            SkinnedModel playermodel = new SkinnedModel();
            ModelInfo.LoadModel(ref playermodel, game.GetScene().Textures, game.GetContent(), game.GetGraphics(), "PlayerMarine_mdla", game.GetScene().Effect);
            model = new AnimModelInfo(playerPosition, Vector3.Zero, new Vector3(1f), playermodel, "PlayerMarine_mdla", (Game)this.game);
            Model lightmodel = new Model();
            ModelInfo.LoadModel(ref lightmodel, game.GetScene().Textures, game.GetContent(), game.GetGraphics(), "sphere_mdl", game.GetScene().Effect);
            ModelInfo tempinfo = new ModelInfo(Vector3.Zero, Vector3.Zero, Vector3.One, lightmodel, "sphere_mdl");
            flashlight = new FlashLightItem((Game) game, tempinfo);
            (this.game as Game).Components.Add(flashlight);
            hasFlashLight = true;
            //Load shaders
            drawModelEffect = game.GetContent().Load<Effect>("Simple");
            //Get worldTransforms, id = 16
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
                playerVelocity = new Vector3(0, model.Body.Velocity.Y, -45);
                isIdle = false;
                currentplayerstate = state.Running;
            }

            if (currentState.IsKeyDown(Keys.I))
            {
                playerVelocity = new Vector3(0, model.Body.Velocity.Y, 45);
                isIdle = false;
                currentplayerstate = state.Running;
            }

            if (isIdle)
            {
                playerVelocity = new Vector3(0, model.Body.Velocity.Y, 0);
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

            model.Rotation = new Vector3(0, playerRotation, 0);
            model.AnimationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
            playerPosition.Y = model.Body.Position.Y;
            model.Body.Velocity = Vector3.Transform(playerVelocity, Matrix.CreateRotationY(MathHelper.ToRadians(playerRotation)));
            /*
            playerPosition += Vector3.Transform(playerVelocity, Matrix.CreateRotationY(MathHelper.ToRadians(playerRotation)));

            model.Body.MoveTo(playerPosition, Matrix.Identity);
             * */
            if (flashlight != null)
                flashlight.model.Position = model.AnimationController.SkinnedBoneTransforms[12].Translation;
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
                    effect.Parameters["xWorld"].SetValue(Matrix.CreateRotationY(MathHelper.ToRadians(playerRotation)) * Matrix.CreateTranslation(model.Body.Position));
                    effect.Parameters["xView"].SetValue(game.GetCamera().View);
                    effect.Parameters["xProjection"].SetValue(game.GetCamera().Projection);
                    effect.Parameters["xCenter"].SetValue(model.Position);
                    effect.Parameters["xRange"].SetValue(4f);


                    //effect.World = modelTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(playerRotation)
                    //    * Matrix.CreateTranslation(playerPosition);
                }
                mesh.Draw();
            }
            if (flashlight != null)
                game.GetScene().DrawModel(flashlight.model);
        }

        public bool DidDamage(BoundingSphere enemysphere)
        {
            return flashlight.Light.Intersects(enemysphere);
        }

        public Vector3 Position
        {
            get { return model.Body.Position; }
        }


        public BoundingSphere BoundingSphere
        {
            get { return boundary.Transform(WorldMatrix); }
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

        public AnimModelInfo Model
        {
            get { return this.model; }
        }

        public void TakeRegularDamage()
        {
            health -= 3;
        }

        public void TakeBossDamage()
        {
            health -= 8;
        }

        public int Health
        {
            get { return health; }
        }

        public Vector3 Rotation
        {
            get { return model.Rotation; }
        }

        public FlashLightItem Flashlight
        {
            get { return flashlight; }
        }
    }
}
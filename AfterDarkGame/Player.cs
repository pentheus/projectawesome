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

namespace AfterDarkGame
{
    public class Player:DrawableGameComponent
    {
        ContainsScene game;
        AnimModelInfo model;
        BoundingSphere boundary;
        int health;
        List<Item> inventory;
        FlashLightItem flashlight;
        Item currentitem;

        Effect drawModelEffect;

        public Player(Game game):
            base(game)
        {
            this.game = (ContainsScene)game;
            LoadContent();
            health = 3;
            inventory = new List<Item>();
            flashlight = null;
        }

        public Player(Game game, List<Item> inv, FlashLightItem light):
            base(game)
        {
            this.game = (ContainsScene)game;
            LoadContent();
            health = 3;
            inventory = inv;
            flashlight = light;
        }

        public void LoadContent()
        {
            SkinnedModel playermodel = new SkinnedModel();
            ModelInfo.LoadModel(ref playermodel, game.GetScene().Textures, game.GetContent(), game.GetGraphics(), "Player", game.GetScene().Effect);
            model = new AnimModelInfo(Vector3.Zero, Vector3.Zero, Vector3.Zero, playermodel, "Player");

            //Load shaders
            drawModelEffect = game.GetScene().Effect;
        }

        public void Update(GameTime gameTime)
        {
            //TODO: Possibly need to update flashlight's position
            model.AnimationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
            base.Update(gameTime);
        }

        public void Draw()
        {
            //Draw Animated Model
            SkinnedModel skinnedModel = model.AnimatedModel;
            Matrix[] modelTransforms = new Matrix[model.Model.Bones.Count];// = model.AnimationController.SkinnedBoneTransforms;
            model.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
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
                    effect.Parameters["xWorld"].SetValue(model.WorldMatrix);
                    effect.Parameters["xView"].SetValue(game.GetCamera().View);
                    effect.Parameters["xProjection"].SetValue(game.GetCamera().Projection);
                    effect.Parameters["xCenter"].SetValue(model.Position);
                    effect.Parameters["xRange"].SetValue(4f);
                }
                mesh.Draw();
            }

            //Draw Flashlight
            ModelInfo flashmodel = flashlight.Model;
            modelTransforms = new Matrix[flashmodel.Model.Bones.Count];
            flashmodel.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            center = model.Position;

            foreach (ModelMesh mesh in flashmodel.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                    part.Effect.Parameters["xTexture"].SetValue(game.GetScene().Textures[part]);

                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = drawModelEffect.Techniques["LambertTest"];
                    effect.Parameters["xWorld"].SetValue(modelTransforms[mesh.ParentBone.Index] * model.WorldMatrix);
                    effect.Parameters["xView"].SetValue(game.MainCamera.View);
                    effect.Parameters["xProjection"].SetValue(game.MainCamera.Projection);
                    effect.Parameters["xCenter"].SetValue(model.Position);
                    effect.Parameters["xRange"].SetValue(4f);
                }
                mesh.Draw();
            }
        }

        public List<Item> Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }

        public AfterDarkGame Game
        {
            get { return game; }
            set { game = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public FlashLightItem FlashLight
        {
            get { return flashlight; }
            set { flashlight = value; }
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
    }
}

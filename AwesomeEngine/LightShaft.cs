using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace AwesomeEngine
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LightShaft : Microsoft.Xna.Framework.DrawableGameComponent
    {
        ModelInfo lightShaft;
        Model lightModel;
        Effect lightEffect;
        ContainsScene game;
        
        public LightShaft(Game game)
            : base(game)
        {
            this.game = (ContainsScene)game;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            lightModel = Game.Content.Load<Model>("cone_mdl");
            lightShaft = new ModelInfo(Vector3.Zero, new Vector3(0,90,0), new Vector3(10f), lightModel, "cone_mdl");
            lightEffect = Game.Content.Load<Effect>("Simple");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            //Calculate Translated Position
            lightShaft.Position = game.GetPlayer().Position + Vector3.Transform((new Vector3(0, 15f, 35f)), Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)));
            lightShaft.Rotation = game.GetPlayer().Rotation;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix[] modelTransforms = new Matrix[lightShaft.Model.Bones.Count];
            lightShaft.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Vector3 center = lightShaft.Position;
            Vector3 adjustedCenter = lightShaft.Position + Vector3.Transform(new Vector3(0, 0, -35), Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)));
            foreach (ModelMesh mesh in lightShaft.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect effect = lightEffect.Clone(Game.GraphicsDevice);
                    effect.CurrentTechnique = effect.Techniques["LightShaftTest"];
                    effect.Parameters["xWorld"].SetValue(Matrix.CreateScale(lightShaft.Scale)*Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y))*Matrix.CreateTranslation(lightShaft.Position));
                    effect.Parameters["xView"].SetValue(game.GetCamera().View);
                    effect.Parameters["xProjection"].SetValue(game.GetCamera().Projection);
                    effect.Parameters["xCenter"].SetValue(adjustedCenter);
                    effect.Parameters["xRange"].SetValue(10f);
                    effect.Parameters["xTextureEnabled"].SetValue(false);
                    part.Effect = effect;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
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
        BoundingSphere innersphere, middlesphere, outersphere;
        Boolean on = false;
        
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
            Vector3 uniZ = Vector3.UnitZ;
            Vector3 innerorigin = game.GetPlayer().Flashlight.model.Position + Vector3.Transform((new Vector3(0, 0, 20f)), Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)));
            Vector3 centerorigin = game.GetPlayer().Flashlight.model.Position + Vector3.Transform((new Vector3(0, 0, 38f)), Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)));
            Vector3 outerorigin = game.GetPlayer().Flashlight.model.Position + Vector3.Transform((new Vector3(0, 0, 55f)), Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)));
            float rot = game.GetPlayer().Rotation.Y;
            innersphere = new BoundingSphere(innerorigin, 9);
            middlesphere = new BoundingSphere(centerorigin, 9);
            outersphere = new BoundingSphere(outerorigin, 9);
            initRays();
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
            lightShaft.Position = game.GetPlayer().Flashlight.model.Position + Vector3.Transform((new Vector3(0, 0f, 30f)), Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)));
            lightShaft.Rotation = game.GetPlayer().Rotation;


            //Update light spheres
            Vector3 innerorigin = game.GetPlayer().Flashlight.model.Position + Vector3.Transform((new Vector3(0, 0, 26f)), Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)));
            Vector3 centerorigin = game.GetPlayer().Flashlight.model.Position + Vector3.Transform((new Vector3(0, 0, 44f)), Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)));
            Vector3 outerorigin = game.GetPlayer().Flashlight.model.Position + Vector3.Transform((new Vector3(0, 0, 61f)), Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)));
            innersphere.Center = innerorigin;
            middlesphere.Center = centerorigin;
            outersphere.Center = outerorigin;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix[] modelTransforms = new Matrix[lightShaft.Model.Bones.Count];
            lightShaft.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Vector3 center = lightShaft.Position;
            Vector3 adjustedCenter = lightShaft.Position + Vector3.Transform(new Vector3(0, 0, -35), Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)));
            if (on == true)
            {
                foreach (ModelMesh mesh in lightShaft.Model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        Effect effect = lightEffect.Clone(Game.GraphicsDevice);
                        effect.CurrentTechnique = effect.Techniques["LightShaftTest"];
                        effect.Parameters["xWorld"].SetValue(Matrix.CreateScale(lightShaft.Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(game.GetPlayer().Rotation.Y)) * Matrix.CreateTranslation(lightShaft.Position));
                        effect.Parameters["xView"].SetValue(game.GetCamera().View);
                        effect.Parameters["xProjection"].SetValue(game.GetCamera().Projection);
                        effect.Parameters["xCenter"].SetValue(adjustedCenter);
                        effect.Parameters["xRange"].SetValue(10f);
                        effect.Parameters["xTextureEnabled"].SetValue(false);
                        part.Effect = effect;
                    }
                    mesh.Draw();
                }
            }
            base.Draw(gameTime);
        }

        public void initRays()
        {
            Vector3 uniZ = Vector3.UnitZ;
            Vector3 origin = game.GetPlayer().Position;
            float rot = game.GetPlayer().Rotation.Y;
        }

        public Boolean Intersects(BoundingSphere boundingSphere)
        {
            if (on == true)
            {
                foreach (BoundingSphere sphere in GetSpheres())
                {
                    if (sphere.Intersects(boundingSphere))
                    {
                        Console.WriteLine("Intersected");
                        return true;
                    }
                }
            }
            return false;
        }

        public BoundingSphere[] GetSpheres()
        {
            BoundingSphere[] spheres = new BoundingSphere[3];
            spheres[0] = innersphere;
            spheres[1] = middlesphere;
            spheres[2] = outersphere;
            return spheres;
        }

        public Boolean On
        {
            get { return on; }
            set { on = value; }
        }
    }
}
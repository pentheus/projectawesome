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
using AwesomeEngine;
using AwesomeEngine.Camera;

namespace TestScene
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ModelInfo ship;
        Light light;
        Matrix lightProjection;
        Model ship1;


        Effect shadowMapEffect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ship = new ModelInfo();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            shadowMapEffect = Content.Load<Effect>("ShadowMap");
            
            //ship.Model = LoadModel("Ship");
            ship.Model = LoadModel("Ship");
            ship1 = Content.Load<Model>("Ship");
            ship.Position = Vector3.Zero;
            ship.Rotation = Vector3.Zero;
            ship.Scale = Vector3.One;
            light = new Light(new Vector3(2000, 0, 0), Vector3.Zero);
            // TODO: use this.Content to load your game content here
        }

        private Model LoadModel(string assetName)
        {

            Model newModel = Content.Load<Model>(assetName);
  
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = shadowMapEffect.Clone(graphics.GraphicsDevice);

            return newModel;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Viewport viewPort = graphics.GraphicsDevice.Viewport;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            lightProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, viewPort.AspectRatio, 0.1f, 10000.0f);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            //graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawModel(ship);
            base.Draw(gameTime);
        }

        
        public void DrawModel(ModelInfo model)
        {
            Matrix[] modelTransforms = new Matrix[model.Model.Bones.Count];
            model.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh mesh in model.Model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * Matrix.Identity;
                    Matrix lightWorldViewProjection = worldMatrix * light.ViewMatrix * lightProjection;
                    effect.CurrentTechnique = effect.Techniques["CreateShadowMap"];
                    effect.Parameters["LightWorldViewProjection"].SetValue(lightWorldViewProjection);
                }
                
                mesh.Draw();
            }
        }


    }
}

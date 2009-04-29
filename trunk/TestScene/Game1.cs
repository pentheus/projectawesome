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
        ModelInfo floor;
        ModelInfo wall;

        Light light;
        Camera camera;
        Matrix lightProjection;
        Effect shadowMapEffect;

        RenderTarget2D renderTarget;
        Texture2D shadowMap;

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
            ship = new ModelInfo();
            floor = new ModelInfo();
            wall = new ModelInfo();
            shadowMapEffect = Content.Load<Effect>("ShadowMap");

            ship.Model = LoadModel("Tank");
            floor.Model = LoadModel("Floor");
            wall.Model = LoadModel("Floor");
            ship.Position = Vector3.Zero;
            ship.Rotation = Vector3.Zero;
            ship.Scale = Vector3.One;
            //ship.Scale = new Vector3(0.002f);

            floor.Position = new Vector3(0, -1, 0);
            floor.Rotation = Vector3.Zero;
            floor.Scale = Vector3.One;

            wall.Position = new Vector3(8, 0, 0);
            wall.Rotation = new Vector3(0, 0, 90);
            wall.Scale = Vector3.One;

<<<<<<< .mine
            light = new Light(new Vector3(-18,5,-2), new Vector3(0f),  500f);
=======
            light = new Light(new Vector3(-18, 5, 0), new Vector3(0f), 100f);
>>>>>>> .r46

            camera = new ThirdPersonCamera(new Vector3(-10, 10, 10), new Vector3(0, 0, 0), GraphicsDevice.Viewport.AspectRatio, 0.1f, 10000.0f);

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1, SurfaceFormat.Single);
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

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                light.Position = light.Position - 0.5f * Vector3.UnitX;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                light.Position = light.Position + 0.5f * Vector3.UnitX;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                light.Position = light.Position - 0.5f * Vector3.UnitY;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                light.Position = light.Position + 0.5f * Vector3.UnitY;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                ship.Rotation = ship.Rotation - 0.05f * Vector3.UnitY;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                ship.Rotation = ship.Rotation + 0.05f * Vector3.UnitY;

            lightProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 1f, light.LightFar);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(0, renderTarget);
            graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            DrawModel(ship, "CreateShadowMap");
            DrawModel(floor, "CreateShadowMap");
            DrawModel(wall, "CreateShadowMap");
            GraphicsDevice.SetRenderTarget(0, null);
            shadowMap = renderTarget.GetTexture();

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.AliceBlue, 1.0f, 0);

            DrawModel(ship, "ShadowedScene");
            DrawModel(floor, "ShadowedScene");
            DrawModel(wall, "ShadowedScene");

            base.Draw(gameTime);
        }


        public void DrawModel(ModelInfo model, String technique)
        {
            Matrix[] modelTransforms = new Matrix[model.Model.Bones.Count];
            model.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh mesh in model.Model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * model.WorldMatrix;
                    Matrix lightWorldViewProjection = worldMatrix * light.ViewMatrix * lightProjection;

                    effect.CurrentTechnique = effect.Techniques[technique];
                    effect.Parameters["WorldViewProjection"].SetValue(worldMatrix * camera.View * camera.Projection);
                    effect.Parameters["LightWorldViewProjection"].SetValue(lightWorldViewProjection);
                    effect.Parameters["ShadowMap"].SetValue(shadowMap);
                    effect.Parameters["Ambient"].SetValue(0.2f);
                    effect.Parameters["LightPos"].SetValue(light.Position);
                    effect.Parameters["LightPower"].SetValue(1.5f);
                    effect.Parameters["TextureEnabled"].SetValue(false);
                    effect.Parameters["World"].SetValue(worldMatrix);
                }

                mesh.Draw();
            }
        }


    }
}

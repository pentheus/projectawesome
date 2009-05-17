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

namespace GameEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
		ReferenceGrid grid;
		Model reference;
		Matrix cameraProjectionMatrix;
		Matrix cameraViewMatrix;

        int xMatrix = 0;
        int yMatrix = 0;
        int zMatrix = 0;
        float rot = 0;

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
            IsMouseVisible = true;
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here

            CreateViewMatrix();
            CreateProjectionMatrix();

            reference = Content.Load<Model>(@"Tank");

            // Load grid
            grid = new ReferenceGrid(GraphicsDevice, 25, 500, Color.Peru);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                yMatrix -= 20;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                yMatrix += 20;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                xMatrix -= 20;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                xMatrix += 20;
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                zMatrix -= 20;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                zMatrix += 20;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                rot -= 0.01f;
            if (Keyboard.GetState().IsKeyDown(Keys.X))
                rot += 0.01f;

            cameraViewMatrix = Matrix.CreateTranslation(new Vector3(xMatrix, yMatrix, zMatrix)) * Matrix.CreateRotationX(rot);
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw reference
            foreach (ModelMesh mesh in reference.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World =
                        Matrix.CreateScale(0.1f) *
                        Matrix.CreateTranslation(Vector3.Zero);

                    effect.Projection = cameraProjectionMatrix;
                    effect.View = cameraViewMatrix;
                }
                mesh.Draw();
            }


            // Draw grid
            grid.Draw(cameraViewMatrix, cameraProjectionMatrix);

            base.Draw(gameTime);
        }

        void CreateViewMatrix()
        {
            cameraViewMatrix = Matrix.CreateLookAt(new Vector3(4000, 4000, 4000), Vector3.Zero, Vector3.Up);
        }

        void CreateProjectionMatrix()
        {
            // Projection matrix
            cameraProjectionMatrix =
                Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45f),
                    graphics.GraphicsDevice.Viewport.AspectRatio,
                    1f, 12000f);
        }
    }
}
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

namespace OctreeTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OctreeTest : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect basicEffect;
        Camera mainCamera;

        public OctreeTest()
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
            basicEffect = new BasicEffect(GraphicsDevice, null);
            mainCamera = new ThirdPersonCamera(new Vector3(0, 0, -10), Vector3.Zero, GraphicsDevice.Viewport.AspectRatio, 1f, 10000f);
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

            // TODO: use this.Content to load your game content here
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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            DrawBoundingBox(new BoundingBox(new Vector3(-1,-1,-1), new Vector3(1,1,1)));
            base.Draw(gameTime);
        }

        public void DrawBoundingBox(BoundingBox boundingBox)
        {
            VertexPositionColor[] points = new VertexPositionColor[8];
            Vector3[] corners = boundingBox.GetCorners();

            points[0] = new VertexPositionColor(corners[1], Color.Red);
            points[1] = new VertexPositionColor(corners[0], Color.Red);
            points[2] = new VertexPositionColor(corners[2], Color.Red);
            points[3] = new VertexPositionColor(corners[3], Color.Red);
            points[4] = new VertexPositionColor(corners[5], Color.Red);
            points[5] = new VertexPositionColor(corners[4], Color.Red);
            points[6] = new VertexPositionColor(corners[6], Color.Red);
            points[7] = new VertexPositionColor(corners[7], Color.Red);

            short[] inds = {
                            0, 1, 0, 2, 1, 3, 2, 3,
                            4, 5, 4, 6, 5, 7, 6, 7,
                            0, 4, 1, 5, 2, 6, 3, 7
                            };

            GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

            basicEffect.World = Matrix.Identity;
            basicEffect.View = mainCamera.View;
            basicEffect.Projection = mainCamera.Projection;

            basicEffect.Begin(SaveStateMode.SaveState);
            for (int pass = 0; pass < basicEffect.CurrentTechnique.Passes.Count; pass++)
            {
                basicEffect.CurrentTechnique.Passes[pass].Begin();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, points, 0, 8, inds, 0, 12);
                basicEffect.CurrentTechnique.Passes[pass].End();
            }
            basicEffect.End();
        }
    }
}

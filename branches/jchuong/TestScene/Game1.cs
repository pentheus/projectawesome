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

        //----------------------
        Camera fpsCam;
        Camera lightCam;
        Light light;
        Model ship;
        Effect shadowMapEffect;
        //----------------------

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
            light = new Light(new Vector3(10,0,0), new Vector3(0,0,0));
            
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
          
            fpsCam = new FirstPersonCamera(new Vector3(0,0,-10),Vector3.Zero,graphics.GraphicsDevice.DisplayMode.AspectRatio,
                0.1f,10000.0f);
            lightCam = new OrthoCamera(light.Position, light.LightTarget,
                graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height,
                0.1f, 10000.0f);

            ship = Content.Load<Model>("Ship");
            shadowMapEffect = Content.Load<Effect>("ShadowMap");

            foreach (ModelMesh mesh in ship.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = shadowMapEffect.Clone(graphics.GraphicsDevice);
            
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
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            // TODO: Add your drawing code here


            Matrix[] transforms = new Matrix[ship.Bones.Count];

            ship.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.

            foreach (ModelMesh mesh in ship.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    // Specify which effect technique to use.
                    effect.CurrentTechnique = effect.Techniques["CreateShadowMap"];

                    Matrix localWorld = transforms[mesh.ParentBone.Index] * Matrix.CreateWorld(Vector3.Zero,Vector3.Forward,Vector3.Up);

                    //effect.Parameters["World"].SetValue(localWorld);
                    effect.Parameters["LightWorld"].SetValue(lightCam.View*lightCam.Projection);
                    //effect.Parameters["Projection"].SetValue(projection);
                }

                mesh.Draw();

            }

            base.Draw(gameTime);
        }
    }
}

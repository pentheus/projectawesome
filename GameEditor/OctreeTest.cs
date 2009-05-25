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
using AwesomeEngine.Items;
using System.IO;


namespace GameEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OctreeTest : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        List<ModelInfo> modelInfos = new List<ModelInfo>();
        List<Item> items = new List<Item>();
        SpriteBatch spriteBatch;
        BasicEffect basicEffect;
        SceneManager sceneMgr;
        ThirdPersonCamera mainCamera;
        ReferenceGrid grid;
        SpriteFont spriteFont;
        Vector2 fontPos;
        Vector3 translationVector = Vector3.Zero;
        XMLParser parser;
        float theta = 0f;
        float phi = 10f;
        Vector3 vector3 = Vector3.Zero;
        float radius = 100f;

        Vector3 modelPosition = Vector3.Zero;
        float modelRotation = 0.0f;
        Vector3 modelVelocity = Vector3.Zero;

        Model cursor;
        KeyboardState previousKeyboardState = Keyboard.GetState();
        long timer = 0;
        int currentIndex = 0;

        public OctreeTest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            sceneMgr = new SceneManager(this);
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
            mainCamera = new ThirdPersonCamera(new Vector3(35f, -24f, -30f), Vector3.Zero, GraphicsDevice.Viewport.AspectRatio, 1f, 10000f);
            parser = new XMLParser(this);
            fontPos = new Vector2(1.0f, 1.0f);
            
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
            spriteFont = Content.Load<SpriteFont>(@"Fonts\DemoFont");

            sceneMgr.MainCamera = mainCamera;
            
            //ShipModel = Content.Load<Model>("Ship");
            //Ship = new ModelInfo(new Vector3(0f, 0f, 0f), Vector3.Zero,new Vector3(0.01f), ShipModel, "Ship");
            //sceneMgr.SceneGraph.addObject(Ship);
            //sceneMgr.SceneGraph.addGeometry(Ship);
            //parser.SaveScene(sceneMgr.SceneGraph, "C:/Users/Spike/Documents/Visual Studio 2008/Projects/projectawesome", "shitsingiggles.xml");
            //sceneMgr.SceneGraph = null;
            //sceneMgr.SceneGraph = parser.ReadScene( "C:/Users/Spike/Documents/Visual Studio 2008/Projects/projectawesome", "shitsingiggles.xml");


            cursor = Content.Load<Model>(@"Ship");

            DirectoryInfo d = new DirectoryInfo("C:\\Users\\Spike\\Documents\\Visual Studio 2008\\Projects\\projectawesome\\GameEditor\\Content");
            FileInfo[] files = d.GetFiles("*.fbx");
            foreach (FileInfo f in files)
            {
                string[] split = f.ToString().Split('.');
                Model model = Content.Load<Model>(split[0]);
                ModelInfo modelInfo = new ModelInfo(new Vector3(0f, 0f, 0f), Vector3.Zero, new Vector3(0.01f), model, split[0]);
                if(split[0].ToLower().Contains("item"))
                {
                    if (split[0].ToLower().Contains("battery"))
                    {
                        items.Add(new BatteryItem(this, modelInfo));
                    }
                    else if (split[0].ToLower().Contains("fuse"))
                    {
                        items.Add(new FuseItem(this, modelInfo));
                    }
                    else if (split[0].ToLower().Contains("glowstick"))
                    {
                        items.Add(new GlowStickItem(this, modelInfo));
                    }
                    else if (split[0].ToLower().Contains("flash"))
                    {
                        //items.Add(new FuseItem(this, modelInfo));
                    }

                }
                else
                    //REMOVE modelInfos.Add(modelInfo);
                Console.WriteLine(f.ToString());
            }

            grid = new ReferenceGrid(GraphicsDevice, 10, 100, Color.LimeGreen);
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
            KeyboardState k = Keyboard.GetState();

            //Spherical Camera controls
            if (k.IsKeyDown(Keys.Up))
                phi += 1f;
            if (k.IsKeyDown(Keys.Down))
                phi -= 1f;
            if (k.IsKeyDown(Keys.Left))
                theta -= 1f;
            if (k.IsKeyDown(Keys.Right))
                theta += 1f;

            float x = (float)(radius * Math.Sin(MathHelper.ToRadians(theta)) * Math.Sin(MathHelper.ToRadians(phi))); ;
            float y = (float)(radius * Math.Cos(MathHelper.ToRadians(phi)));
            float z = (float)(radius * Math.Cos(MathHelper.ToRadians(theta)) * Math.Sin(MathHelper.ToRadians(phi)));

            if (k.IsKeyDown(Keys.E))
                radius -= 2f;
            if (k.IsKeyDown(Keys.F))
                radius += 2f;

            if (k.IsKeyDown(Keys.W))
                translationVector += new Vector3(0f, 0f, -1f);
            if (k.IsKeyDown(Keys.S))
                translationVector += new Vector3(0f, 0f, 1f);
            if (k.IsKeyDown(Keys.A))
                translationVector += new Vector3(-1f, 0f, 0f);
            if (k.IsKeyDown(Keys.D))
                translationVector += new Vector3(1f, 0f, 0f);

            UpdateCursorInput();
            // Add velocity to the current position.
            modelPosition += modelVelocity;
            modelVelocity *= 0f;

            if (k.IsKeyUp(Keys.Enter) && previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                AddModel("Tank" + timer);
            }

            if (k.IsKeyUp(Keys.OemOpenBrackets) && previousKeyboardState.IsKeyDown(Keys.OemOpenBrackets))
                currentIndex--;
            if (k.IsKeyUp(Keys.OemCloseBrackets) && previousKeyboardState.IsKeyDown(Keys.OemCloseBrackets))
                currentIndex++;

            if (currentIndex >= modelInfos.Count)
                currentIndex = 0;
            if (currentIndex < 0)
                 currentIndex = modelInfos.Count -1 ;

            mainCamera.Pos = (new Vector3(x,y,z));
            mainCamera.Pos = Vector3.Transform(mainCamera.Pos, Matrix.CreateTranslation(translationVector));
            mainCamera.LookAt = translationVector;

            base.Update(gameTime);

            previousKeyboardState = k;
            timer++;
        }

        private void AddModel(string s)
        {
            Model model = Content.Load<Model>(@"Tank");
            ModelInfo modelInfo = new ModelInfo(new Vector3(0f, 0f, 0f), Vector3.Zero, new Vector3(0.01f), model, s);
            modelInfos.Add(modelInfo);
        }

        protected void UpdateCursorInput()
        {
            // Get the game pad state.
            KeyboardState currentState = Keyboard.GetState(PlayerIndex.One);

            // Rotate the model using the left thumbstick, and scale it down.
            if (currentState.IsKeyDown(Keys.L))
            {
                modelRotation -= 0.075f;
            }
            if (currentState.IsKeyDown(Keys.J))
            {
                modelRotation += 0.075f;
            }
            

            // Create some velocity if the right trigger is down.
            Vector3 modelVelocityAdd = Vector3.Zero;

            // Find out what direction we should be thrusting, using rotation.
            modelVelocityAdd.X = -(float)Math.Sin(modelRotation);
            modelVelocityAdd.Z = -(float)Math.Cos(modelRotation);

            // Now scale our direction by how hard the trigger is down.

            bool add = false;

            if (currentState.IsKeyDown(Keys.K))
            {
                modelVelocityAdd *= .5f;
                add = true;
            }

            if (currentState.IsKeyDown(Keys.I))
            {
                modelVelocityAdd *= -.5f;
                add = true;
            }

            if (!add)
            {
                modelVelocityAdd *= .00f;
            }

            // Finally, add this vector to our velocity.
            modelVelocity += modelVelocityAdd;

            // In case you get lost, press A to warp back to the center.
            if (currentState.IsKeyDown(Keys.Home))
            {
                modelPosition = Vector3.Zero;
                modelVelocity = Vector3.Zero;
                modelRotation = 0.0f;
            }
        }   

        private void DrawText()
        {
            string text = null;
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();

            if(modelInfos.Count!=0)
                buffer.Append(modelInfos[currentIndex].FileName);

            text = buffer.ToString();
            
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            spriteBatch.DrawString(spriteFont, text, fontPos, Color.Yellow);
            spriteBatch.End();
        }

        public void DrawOctree(Node parent)
        {
            if (!parent.HasChildren())
            {
                DrawBoundingBox(parent.BoundingBox);
                return;
            }

            foreach (Node child in parent.Children)
            {
                DrawOctree(child);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            DrawCursor();
            DrawModelInfos();

            DrawText();
            grid.Draw(mainCamera.View, mainCamera.Projection);
            base.Draw(gameTime);
        }

        private void DrawCursor()
        {
            Matrix[] transforms = new Matrix[cursor.Bones.Count];
            cursor.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in cursor.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation)
                        * Matrix.CreateTranslation(modelPosition);

                    effect.Projection = mainCamera.Projection;
                    effect.View = mainCamera.View;
                }
                mesh.Draw();
            }
        }

        private void DrawModelInfos()
        {
            foreach (ModelInfo mi in modelInfos)
            {
                Matrix[] transforms = new Matrix[mi.Model.Bones.Count];
                mi.Model.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in mi.Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation)
                        * Matrix.CreateTranslation(modelPosition);

                        effect.Projection = mainCamera.Projection;
                        effect.View = mainCamera.View;
                    }
                    mesh.Draw();
                }
            }

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

            short[] inds = {0, 1, 0, 2, 1, 3, 2, 3,
                            4, 5, 4, 6, 5, 7, 6, 7,
                            0, 4, 1, 5, 2, 6, 3, 7};

            GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

            basicEffect.World = Matrix.Identity;
            basicEffect.View = mainCamera.View;
            basicEffect.Projection = mainCamera.Projection;
            basicEffect.DiffuseColor = new Vector3(0.6f, 0f, 0f);

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

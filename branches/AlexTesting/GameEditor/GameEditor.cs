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
using XNAnimation;


namespace GameEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameEditor : Microsoft.Xna.Framework.Game, ContainsScene
    {
        GraphicsDeviceManager graphics;
        Dictionary<String, Model> props = new Dictionary<String, Model>();
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
        ToolBar toolBar;
        float theta = 0f;
        float phi = 10f;
        Vector3 vector3 = Vector3.Zero;
        float radius = 100f;
        ModelInfo cursor = new ModelInfo();

        //This is for testing and can be deleted
        AnimModelInfo testmarine;

        public GameEditor()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            sceneMgr = new SceneManager(this);
            Components.Add(sceneMgr);
            toolBar = new ToolBar(this);
            toolBar.Show();
            this.IsMouseVisible = true;
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
            
            DirectoryInfo d = new DirectoryInfo(Content.RootDirectory+"\\Models\\");
            FileInfo[] files = d.GetFiles("*mdl.xnb");
            
            foreach (FileInfo f in files)
            {
                string[] split = f.ToString().Split('.');
                Model model = new Model();
                ModelInfo.LoadModel(ref model, sceneMgr.Textures, Content, graphics.GraphicsDevice, split[0], sceneMgr.Effect);
                ModelInfo modelInfo = new ModelInfo(new Vector3(0f, 0f, 0f), Vector3.Zero, new Vector3(0.1f), model, split[0]);
                if (split[0].ToLower().Contains("item"))
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
                {
                    props.Add(modelInfo.FileName, model);
                    Console.WriteLine("Object added");
                    toolBar.TreeView.Nodes["Props"].Nodes.Add(modelInfo.FileName,modelInfo.FileName);
                    toolBar.TreeView.Nodes["Props"].Nodes[modelInfo.FileName].Name = modelInfo.FileName;
                    cursor = modelInfo;
                }

                Console.WriteLine(f.ToString());
            }

            SkinnedModel marinemodel = new SkinnedModel();
            ModelInfo.LoadModel(ref marinemodel, sceneMgr.Textures, Content, graphics.GraphicsDevice, "PlayerMarine_mdla", sceneMgr.Effect);
            testmarine = new AnimModelInfo(new Vector3(0f, 0f, 0f), Vector3.Zero, new Vector3(1f), marinemodel, "PlayerMarine_mdla");
            testmarine.animateModel("Run");
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

            if (!toolBar.Focused)
            {
                //Spherical Camera controls
                if (k.IsKeyDown(Keys.Up))
                    phi += 1f;
                if (k.IsKeyDown(Keys.Down))
                    phi -= 1f;
                if (k.IsKeyDown(Keys.Left))
                    theta -= 1f;
                if (k.IsKeyDown(Keys.Right))
                    theta += 1f;

             
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
            }
            float x = (float)(radius * Math.Sin(MathHelper.ToRadians(theta)) * Math.Sin(MathHelper.ToRadians(phi))); ;
            float y = (float)(radius * Math.Cos(MathHelper.ToRadians(phi)));
            float z = (float)(radius * Math.Cos(MathHelper.ToRadians(theta)) * Math.Sin(MathHelper.ToRadians(phi)));
            
            mainCamera.Pos = (new Vector3(x,y,z));
            mainCamera.Pos = Vector3.Transform(mainCamera.Pos, Matrix.CreateTranslation(translationVector));

            mainCamera.LookAt = translationVector;
            testmarine.AnimationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
            base.Update(gameTime); 
        }

        private void DrawText()
        {
            string text = null;
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();

            buffer.AppendLine("Testing");

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
            // TODO: Add your drawing code here
            DrawText();
            sceneMgr.DrawAnimatedModel(testmarine);
            sceneMgr.DrawModel(cursor);
            grid.Draw(mainCamera.View, mainCamera.Projection);
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

        public void SetCursorModel(String name)
        {
            try
            {
                cursor = new ModelInfo(Vector3.Zero, Vector3.Zero, Vector3.One, props[name], name);
                //cursor = props[name];
            }
            catch (KeyNotFoundException e)
            {
                //do nothing
            }
        }

        public SceneManager GetScene()
        {
            return sceneMgr;
        }

        public GraphicsDevice GetGraphics()
        {
            return graphics.GraphicsDevice;
        }

        public ContentManager GetContent()
        {
            return Content;
        }
    }
}

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
using JigLibX.Physics;
using AwesomeEngine.Items;
using JigLibX.Geometry;
using System.IO;
using JigLibX.Collision;
using XNAnimation;
using AwesomeEngine.Enemies;


namespace AfterDarkGame
{//
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AfterDarkGame : Microsoft.Xna.Framework.Game, ContainsScene
    {
        GraphicsDeviceManager graphics;


        //Game Specific Variables
        SpriteBatch spriteBatch;
        BasicEffect basicEffect;
        SceneManager sceneMgr;
        XMLParser parser;
        Player player;

        //Camera Variables
        ThirdPersonCamera mainCamera;
        float theta = 0f;
        float phi = 45f;
        float radius = 100f;
        Vector3 vector3 = Vector3.Zero;
        Vector3 translationVector = Vector3.Zero;

        //Editor Variables
        ModelInfo cursor;
        Dictionary<String, Model> props = new Dictionary<String, Model>();
        List<Item> items = new List<Item>();
        SpriteFont spriteFont;
        Vector2 fontPos;
        MouseState oldMouseState = new MouseState();
        LightShaft lightShaft;
        AnimModelInfo enemyModelInfo;
        //AnimModelInfo enemyModelInfo;
        SkinnedModel enemyModel;
        PhysicsSystem physics;

        //Trigger spheres
        TriggerEntity rushtrigger;
        TriggerEntity endtrigger;
        //End spawners
        SpawnEntity[] endspawns = new SpawnEntity[6];
        SpawnEntity endspawn1;
        SpawnEntity endspawn2;
        SpawnEntity endspawn3;
        SpawnEntity endspawn4;
        SpawnEntity endspawn5;
        SpawnEntity endspawn6;
      
        public AfterDarkGame()
        {
            cursor = new ModelInfo();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            sceneMgr = new SceneManager(this);
            lightShaft = new LightShaft(this);            //sceneMgr.UpdateOrder = 0;
            Components.Add(sceneMgr);
            sceneMgr.DrawOrder=0;
            lightShaft.DrawOrder=10;
            this.IsMouseVisible = false;
            parser = new XMLParser(this);

            InitializePhysics();
        }

        private void InitializePhysics()
        {
            physics = new PhysicsSystem();
            physics.CollisionSystem = new CollisionSystemSAP();
            physics.NumCollisionIterations = 8;
            physics.NumContactIterations = 8;

            physics.CollisionTollerance = 1;
            physics.AllowedPenetration = 1;

            physics.EnableFreezing = true;
            physics.SolverType = PhysicsSystem.Solver.Normal;
            physics.CollisionSystem.UseSweepTests = true;
            physics.NumPenetrationRelaxtionTimesteps = 15;
            physics.Gravity = new Vector3(0, -918, 0);
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
            fontPos = new Vector2(1.0f, 1.0f);
            player = new Player(this);
            //player.UpdateOrder = 1;
            this.Components.Add(player);
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

            ModelInfo.LoadModel(ref enemyModel, sceneMgr.Textures, Content, GraphicsDevice, "shadowmonster", sceneMgr.Effect);
            enemyModelInfo = new AnimModelInfo(new Vector3(0, 20, 0), Vector3.Zero, new Vector3(15), enemyModel, "shadowmonster", this);

            sceneMgr.MainCamera = mainCamera;

            DirectoryInfo d = new DirectoryInfo(Content.RootDirectory + "\\Models\\");

            OpenLevel(Content.RootDirectory + "/scene.xml");
            // TODO: use this.Content to load your game content here

            Model triggermodel = Content.Load<Model>("Models/ent_mdl");
            rushtrigger = new TriggerEntity(this, triggermodel, new Vector3(23.5f, -243f, 231.5f));
            rushtrigger.BoundingSphere = new BoundingSphere(rushtrigger.Position, 40);
            endtrigger = new TriggerEntity(this, triggermodel, new Vector3(183.5f, -232.5f, 123.5f));
            endtrigger.BoundingSphere = new BoundingSphere(endtrigger.Position, 25);

            endspawn1 = new SpawnEntity(this, triggermodel, new Vector3(133.5f, -243, 59.5f), enemyModel);
            endspawns[0] = endspawn1;
            endspawn2 = new SpawnEntity(this, triggermodel, new Vector3(125f, -243, 170.5f), enemyModel);
            endspawns[1] = endspawn2;
            endspawn3 = new SpawnEntity(this, triggermodel, new Vector3(153, -243, 105), enemyModel);
            endspawns[2] = endspawn3;
            endspawn4 = new SpawnEntity(this, triggermodel, new Vector3(78.5f, -243, 96), enemyModel);
            endspawns[3] = endspawn4;
            endspawn5 = new SpawnEntity(this, triggermodel, new Vector3(85.5f, -243, 34), enemyModel);
            endspawns[4] = endspawn5;
            endspawn6 = new SpawnEntity(this, triggermodel, new Vector3(50f, -243, 162.5f), enemyModel);
            endspawns[5] = endspawn6;
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
            //Check the player's bounding sphere and camera's frustom for intersections with nodes in the octree (including children nodes)
            //For each component that needs to be updated, add it to the this.Components
            //Each component should check to see if it's disappeared, if it has, remove it from the components list
            //

            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState k = Keyboard.GetState();
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.X >= 0 && currentMouseState.X <= GraphicsDevice.Viewport.Width &&
                currentMouseState.Y >= 0 && currentMouseState.Y <= GraphicsDevice.Viewport.Height)
            {
                //Spherical Camera controls
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    phi -= 0.8f * (currentMouseState.Y - oldMouseState.Y);
                    theta -= 0.8f * (currentMouseState.X - oldMouseState.X);

                }
                radius -= 0.03f * (currentMouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue);
            }
            if (k.IsKeyDown(Keys.W))
                translationVector += new Vector3(0f, 0f, -1f);
            if (k.IsKeyDown(Keys.S))
                translationVector += new Vector3(0f, 0f, 1f);
            if (k.IsKeyDown(Keys.A))
                translationVector += new Vector3(-1f, 0f, 0f);
            if (k.IsKeyDown(Keys.D))
                translationVector += new Vector3(1f, 0f, 0f);

            float x = (float)(radius * Math.Sin(MathHelper.ToRadians(theta + player.Rotation.Y)) * Math.Sin(MathHelper.ToRadians(phi))); 
            float y = (float)(radius * Math.Cos(MathHelper.ToRadians(phi)));
            float z = (float)(radius * Math.Cos(MathHelper.ToRadians(theta + player.Rotation.Y)) * Math.Sin(MathHelper.ToRadians(phi)));

            mainCamera.Pos = (new Vector3(x, y, z)) + player.Position;
            //mainCamera.Pos = Vector3.Transform(mainCamera.Pos, Matrix.CreateTranslation(player.Position));

            mainCamera.LookAt = player.Position;

            oldMouseState = currentMouseState;
            //Integrating phyiscs system
            float timeStep = (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            PhysicsSystem.CurrentPhysicsSystem.Integrate(timeStep);
            
            //Check for player's intersection with trigger spheres.
            if (rushtrigger.BoundingSphere.Intersects(player.ItemSphere) && player.HasFuse())
            {
                foreach (SpawnEntity spawner in endspawns)
                {
                    if (!this.Components.Contains(spawner))
                        this.Components.Add(spawner);
                }
            }
            if (endtrigger.BoundingSphere.Intersects(player.ItemSphere) && player.HasFuse())
            {
                //Win the game
                Console.WriteLine("Won the game.");
            }
            //Console.WriteLine("Shadow's position" + shadow.Model.Position);
            base.Update(gameTime);
        }

        private void DrawText()
        {
            string text = Mouse.GetState().X.ToString() + " " + this.oldMouseState.X;

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            spriteBatch.DrawString(spriteFont, text, fontPos, Color.Yellow);
            spriteBatch.End();
        }

        public void DrawOctree(Node parent)
        {
            foreach (Node child in parent.Children)
            {
                DrawOctree(child);
            }
        }

        public void OpenLevel(string level)
        {
            XMLParser parser = new XMLParser(this);
            sceneMgr.SceneGraph = parser.ReadScene(level);
            foreach(Item item in sceneMgr.SceneGraph.GetItems())
                Components.Add(item);
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
            //sceneMgr.DrawModel(cursor);
            player.Draw();
            //sceneMgr.DrawAnimatedModel(shadow.Model);
            BoundingSphereRenderer.Render(player.BoundingSphere, graphics.GraphicsDevice, mainCamera.View, mainCamera.Projection, Color.Red);
            BoundingSphereRenderer.Render(rushtrigger.BoundingSphere, graphics.GraphicsDevice, mainCamera.View, mainCamera.Projection, Color.Red);
            BoundingSphereRenderer.Render(endtrigger.BoundingSphere, graphics.GraphicsDevice, mainCamera.View, mainCamera.Projection, Color.Red);
            foreach(Enemy enemy in sceneMgr.SceneGraph.GetEnemies())
            {
                BoundingSphereRenderer.Render(enemy.SeekingBoundingSphere, graphics.GraphicsDevice, mainCamera.View, mainCamera.Projection, Color.Red);
            }
            base.Draw(gameTime);
        }

        public void DrawBoundingBox(BoundingBox boundingBox)
        {
            if (this.player.Position != null && this.player.Flashlight.Light != null)
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
                    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineList, points, 0, 8, inds, 0, 12);
                    basicEffect.CurrentTechnique.Passes[pass].End();
                }
                basicEffect.End();
            }
        }

        public SceneManager GetScene()
        {
            return sceneMgr;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public void SetScene(AwesomeEngine.Octree scene)
        {
            sceneMgr.SceneGraph = scene;
        }

        public GraphicsDevice GetGraphics()
        {
            return graphics.GraphicsDevice;
        }

        public ContentManager GetContent()
        {
            return Content;
        }


        public ThirdPersonCamera GetCamera()
        {
            return mainCamera;
        }

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public Vector3 GetCursorLocation()
        {
            return player.Position;
        }
    }
}

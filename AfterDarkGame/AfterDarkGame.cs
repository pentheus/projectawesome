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
        ModelInfo cursor = new ModelInfo();
        Dictionary<String, Model> props = new Dictionary<String, Model>();
        List<Item> items = new List<Item>();
        SpriteFont spriteFont;
        Vector2 fontPos;
        MouseState oldMouseState = new MouseState();

        public AfterDarkGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            sceneMgr = new SceneManager(this);
            //sceneMgr.UpdateOrder = 0;
            Components.Add(sceneMgr);
            this.IsMouseVisible = false;
            parser = new XMLParser(this);

            InitializePhysics();
        }

        private void InitializePhysics()
        {
            PhysicsSystem world = new PhysicsSystem();
            world.CollisionSystem = new CollisionSystemSAP();
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

            sceneMgr.MainCamera = mainCamera;

            DirectoryInfo d = new DirectoryInfo(Content.RootDirectory + "\\Models\\");
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
                    cursor = modelInfo;
                }

                Console.WriteLine(f.ToString());
            }
            

            OpenLevel("C:\\Users\\Spike\\Desktop\\projectawesomeNew\\scene.xml");
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
            //Check the player's bounding sphere and camera's frustom for intersections with nodes in the octree (including children nodes)
            //For each component that needs to be updated, add it to the this.Components
            //Each component should check to see if it's disappeared, if it has, remove it from the components list
            //
            player.Update(gameTime);

            // integrating timeStep
            float timeStep = (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            PhysicsSystem.CurrentPhysicsSystem.Integrate(timeStep);
            
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

            float x = (float)(radius * Math.Sin(MathHelper.ToRadians(theta)) * Math.Sin(MathHelper.ToRadians(phi))); ;
            float y = (float)(radius * Math.Cos(MathHelper.ToRadians(phi)));
            float z = (float)(radius * Math.Cos(MathHelper.ToRadians(theta)) * Math.Sin(MathHelper.ToRadians(phi)));

            mainCamera.Pos = (new Vector3(x, y, z));
            mainCamera.Pos = Vector3.Transform(mainCamera.Pos, Matrix.CreateTranslation(translationVector));

            mainCamera.LookAt = translationVector;

            oldMouseState = currentMouseState;
          
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

        public void OpenLevel()
        {
            XMLParser parser = new XMLParser(this);
            sceneMgr.SceneGraph = parser.ReadScene("Something");
        }        

        public void OpenLevel(string level)
        {
            XMLParser parser = new XMLParser(this);
            sceneMgr.SceneGraph = parser.ReadScene(level);
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
            sceneMgr.DrawModel(cursor);
            player.Draw();
            base.Draw(gameTime);
        }

        public SceneManager GetScene()
        {
            return sceneMgr;
        }

        public void SetScene(Octree scene)
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
    }
}

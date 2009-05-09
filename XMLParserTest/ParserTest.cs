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
using System.IO;


namespace XMLParserTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ParserTest : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Octree testtree;
        Effect shadowMapEffect;
        FileInfo outfile;
        StreamWriter writer;
        SceneManager sceneMgr;
        Camera mainCamera;

        public ParserTest()
        {
            testtree = new Octree(54);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            outfile = new FileInfo("C:/Documents and Settings/Alex/My Documents/Inf 125/ProjectAwesome/TestingOutput.txt");
            writer = outfile.CreateText();
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

            mainCamera = new ThirdPersonCamera(new Vector3(0, 200, 200), Vector3.Zero, GraphicsDevice.Viewport.AspectRatio, 1f, 10000f);
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
            BuildTree();
            SaveScene();
            LoadScene();

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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            sceneMgr.Draw(gameTime);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void BuildTree()
        {
                testtree.addGeometry(new ModelInfo(new Vector3(0), new Vector3(0), new Vector3(0), Content.Load<Model>("Floor"), "Floor"));
                testtree.addObject(new ModelInfo(new Vector3(2), new Vector3(0), new Vector3(0), Content.Load<Model>("Tank"), "Tank"));
                testtree.addObject(new ModelInfo(new Vector3(5), new Vector3(0), new Vector3(0), Content.Load<Model>("Floor"), "Floor"));
                testtree.addObject(new ModelInfo(new Vector3(8), new Vector3(0), new Vector3(0), Content.Load<Model>("Tank"), "Tank"));
        }

        public void SaveScene()
        {
            XMLParser parser = new XMLParser(this);
            parser.SaveScene(testtree, "C:/Documents and Settings/Alex/My Documents/Inf 125/ProjectAwesome/XMLParserTest", "TestScene.xml");
        }

        public void LoadScene()
        {
            XMLParser parser = new XMLParser(this);
            testtree = parser.ReadScene("C:/Documents and Settings/Alex/My Documents/Inf 125/ProjectAwesome/XMLParserTest", "TestScene.xml");
            sceneMgr.SceneGraph = testtree;
            DrawTree(testtree);
        }

        private void DrawTree(Octree tree)
        {
            ModelInfo minfo;
            writer.WriteLine("Tree's size is " + tree.TreeSize);

            minfo = tree.getGeometry();
            writer.WriteLine("Geometry's scale is " + minfo.Scale.ToString());
            writer.WriteLine("Geometry's model is " + minfo.FileName);
            writer.WriteLine("");

            foreach (ModelInfo nextone in tree.getDrawableObjects())
            {
                minfo = nextone;
                writer.WriteLine("Object's position is " + minfo.Position.ToString());
                writer.WriteLine("Object's rotation is " + minfo.Rotation.ToString());
                writer.WriteLine("Object's scale is " + minfo.Scale.ToString());
                writer.WriteLine("Object's model is " + minfo.FileName);
            }
            writer.Flush();
            writer.Close();
        }
    }
}
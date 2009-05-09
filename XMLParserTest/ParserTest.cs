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
        Game game;
        Effect shadowMapEffect;

        public ParserTest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            game = new Game();
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

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void BuildTree()
        {
            testtree = new Octree(54);
                testtree.addGeometry(new ModelInfo(new Vector3(0), new Vector3(0), new Vector3(0), LoadModel("Floor"), "Floor"));
            testtree.addObject(new ModelInfo(new Vector3(2), new Vector3(0), new Vector3(0), LoadModel("Tank"), "Tank"));
            testtree.addObject(new ModelInfo(new Vector3(5), new Vector3(0), new Vector3(0), LoadModel("Floor"), "Floor"));
            testtree.addObject(new ModelInfo(new Vector3(8), new Vector3(0), new Vector3(0), LoadModel("Tank"), "Tank"));
        }

        public void SaveScene()
        {
            XMLParser parser = new XMLParser(game);
            parser.SaveScene(testtree, "C:/Documents and Settings/Alex/My Documents/Inf 125/ProjectAwesome/XMLParserTest", "TestScene.xml");
        }

        public void LoadScene()
        {
            XMLParser parser = new XMLParser(game);
            testtree = parser.ReadScene("C:/Documents and Settings/Alex/My Documents/Inf 125/ProjectAwesome/XMLParserTest", "TestScene.xml");
            DrawTree(testtree);
        }

        private void DrawTree(Octree tree)
        {
            ModelInfo minfo;
            Console.WriteLine("Tree's size is " + tree.TreeSize);

            minfo = tree.getGeometry();
            Console.WriteLine("Geometry's scale is " + minfo.Scale.ToString());
            Console.WriteLine("Geometry's model is " + minfo.FileName);
            Console.WriteLine("");

            foreach (ModelInfo nextone in tree.getDrawableObjects())
            {
                minfo = nextone;
                Console.WriteLine("Object's position is " + minfo.Position.ToString());
                Console.WriteLine("Object's rotation is " + minfo.Rotation.ToString());
                Console.WriteLine("Object's scale is " + minfo.Scale.ToString());
                Console.WriteLine("Object's model is " + minfo.FileName);
            }
        }


        private Model LoadModel(string assetName)
        {

            Model newModel = game.Content.Load<Model>(assetName);

            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = shadowMapEffect.Clone(graphics.GraphicsDevice);

            return newModel;
        }
    }
}

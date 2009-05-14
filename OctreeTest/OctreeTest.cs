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
        SceneManager sceneMgr;
        Camera mainCamera;
        ModelInfo Ship, Ship2,Ship3,Ship4,Ship5,Ship6;
 
        Model ShipModel;
        XMLParser parser;
        float theta = 0f;
        float phi = 10f;
        float radius = 100f;

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
            //sceneMgr.SceneGraph.SplitNode(sceneMgr.SceneGraph.Root);
            //sceneMgr.SceneGraph.SplitNode(sceneMgr.SceneGraph.Root.Children[0]);
            //sceneMgr.SceneGraph.SplitNode(sceneMgr.SceneGraph.Root.Children[1]);
            //sceneMgr.SceneGraph.SplitNode(sceneMgr.SceneGraph.Root.Children[2]);
            //sceneMgr.SceneGraph.SplitNode(sceneMgr.SceneGraph.Root.Children[6]);
            //sceneMgr.SceneGraph.SplitNode(sceneMgr.SceneGraph.Root.Children[7]);
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

            sceneMgr.MainCamera = mainCamera;
            
            ShipModel = Content.Load<Model>("Ship");
            Ship = new ModelInfo(new Vector3(0f, 0f, 0f), Vector3.Zero,new Vector3(0.01f), ShipModel, "Ship");
            Ship2 = new ModelInfo(new Vector3(-20f, 30f, 40f), Vector3.Zero, new Vector3(0.01f), ShipModel, "Ship");
            Ship3 = new ModelInfo(new Vector3(40f, -30f, -30f), Vector3.Zero, new Vector3(0.01f), ShipModel, "Ship");
            Ship4 = new ModelInfo(new Vector3(-35f, -21f, -30f), Vector3.Zero, new Vector3(0.01f), ShipModel, "Ship");
            Ship5 = new ModelInfo(new Vector3(-35f, -21f, -30f), Vector3.Zero, new Vector3(0.01f), ShipModel, "Ship");
            Ship6 = new ModelInfo(new Vector3(-35f, -21f, -30f), Vector3.Zero, new Vector3(0.01f), ShipModel, "Ship");
            sceneMgr.SceneGraph.addObject(Ship);
            sceneMgr.SceneGraph.addObject(Ship2);
            sceneMgr.SceneGraph.addObject(Ship3);
            sceneMgr.SceneGraph.addObject(Ship4);
            sceneMgr.SceneGraph.addObject(Ship5);
            sceneMgr.SceneGraph.addObject(Ship6);
            sceneMgr.SceneGraph.addGeometry(Ship);
            parser.SaveScene(sceneMgr.SceneGraph, "C:/Users/Jonathan/Documents/Visual Studio 2008/Projects/projectawesome", "shitsingiggles.xml");
            sceneMgr.SceneGraph = null;
            sceneMgr.SceneGraph = parser.ReadScene( "C:/Users/Jonathan/Documents/Visual Studio 2008/Projects/projectawesome", "shitsingiggles.xml");
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
            if (k.IsKeyDown(Keys.W))
                phi += 1f;
            if (k.IsKeyDown(Keys.S))
                phi -= 1f;
            if (k.IsKeyDown(Keys.A))
                theta -= 1f;
            if (k.IsKeyDown(Keys.D))
                theta += 1f;
 
            float x = (float)(radius * Math.Sin(MathHelper.ToRadians(theta)) * Math.Sin(MathHelper.ToRadians(phi))); ;
            float y = (float)(radius * Math.Cos(MathHelper.ToRadians(phi)));
            float z = (float)(radius * Math.Cos(MathHelper.ToRadians(theta)) * Math.Sin(MathHelper.ToRadians(phi)));

            mainCamera.Pos = (new Vector3(x,y,z));
            base.Update(gameTime);
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
            sceneMgr.Draw(gameTime);
            // TODO: Add your drawing code here
            DrawOctree(sceneMgr.SceneGraph.Root);
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
    }
}

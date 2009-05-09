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


namespace AwesomeEngine
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SceneManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Octree sceneGraph;
        Game game;
        ShadowRenderer shadowRenderer;
        Effect drawModelEffect;
        Texture2D renderTarget;
        Camera.Camera mainCamera;
        
        public SceneManager(Game game)
            : base(game)
        {
            sceneGraph = new Octree(100f);
            this.game = game;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            shadowRenderer = new ShadowRenderer(game.Content.Load<Effect>("ShadowMap"));
            drawModelEffect = game.Content.Load<Effect>("DrawModel");
      
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawScene(sceneGraph.Root);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws a scene
        /// </summary>
        public void DrawScene(Node parent) 
        {
            if (!parent.HasChildren())
            {
                DrawNode(parent);
                return;
            }

            foreach (Node child in parent.Children)
            {
                if(child.BoundingBox.Intersects(mainCamera.BoundingFrustum))
                    DrawScene(child);
            }
        }

        /// <summary>
        /// Draws visible objects in the node
        /// </summary>
        /// <param name="node">A node from the Octree scene graph. Contains a list of drawable objects</param>
        public void DrawNode(Node node)
        {
            foreach (ModelInfo model in node.DrawableObjects)
            {
                //shadowRenderer.CreateShadowMap(model, out renderTarget);
                if (!CheckIfCullable(model))
                {
                    DrawLitModel(model); 
                } 
            }
        }

        /// <summary>
        /// Draws the completely lit scene with shadows
        /// </summary>
        /// <param name="model"></param>
        public void DrawLitModel(ModelInfo model)
        {
            Matrix[] modelTransforms = new Matrix[model.Model.Bones.Count];
            model.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            foreach (ModelMesh mesh in model.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * model.WorldMatrix;
                    /*
                    effect.CurrentTechnique = drawModelEffect.Techniques["DrawModel"];
                    effect.Parameters["xWorld"].SetValue(worldMatrix);
                    effect.Parameters["ColorMap"].SetValue((effect as BasicEffect).Texture);
                    effect.Parameters["Ambient"].SetValue(0.5f); //Later have have a global variable for ambient that we will use for this.
                    effect.Parameters["TextureEnabled"].SetValue(false); //have to change this later...
                    //insert more parameters*/
                    effect.AmbientLightColor = Vector3.One;
                    effect.DirectionalLight0.DiffuseColor= new Vector3(0.5f,0.5f,0.5f);
                    effect.DirectionalLight0.Direction = new Vector3(-1f,-1f,-1f);
                    effect.DirectionalLight0.Enabled = true;
                    effect.LightingEnabled = true;

                    effect.PreferPerPixelLighting = true;
                    effect.SpecularColor = Vector3.Zero;
                    effect.SpecularPower = 0f;
                    effect.View = mainCamera.View;
                    effect.Projection = mainCamera.Projection;
                    effect.World = worldMatrix;
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// Checks if model's bounding volume intersects the view frustum.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CheckIfCullable(ModelInfo model)
        {
            foreach (ModelMesh mesh in model.Model.Meshes)
            {
                if (mesh.BoundingSphere.Intersects(mainCamera.BoundingFrustum))
                    return false;
            }
            return true;
        }

        public Octree SceneGraph
        {
            get { return sceneGraph; }
            set { sceneGraph = value; }
        }

        public Camera.Camera MainCamera
        {
            get { return mainCamera; }
            set { mainCamera = value; }
        }
    }
}
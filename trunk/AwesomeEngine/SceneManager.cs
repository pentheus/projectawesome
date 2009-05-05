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
        Texture2D renderTarget;
        Camera.Camera mainCamera;

        public SceneManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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
            DrawScene();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws a scene
        /// </summary>
        public void DrawScene() 
        {
            foreach (Node node in sceneGraph)
            {
                if(node.intersectsWith(mainCamera.BoundingFrustum));
                    DrawNode(node);
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
                shadowRenderer.CreateShadowMap(model, out renderTarget);
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
                foreach (Effect effect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * model.WorldMatrix;
                    //(effect as BasicEffect).Texture;
                    //insert set effects and properties
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
    }
}
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
    {//
        Octree sceneGraph;
        Game game;
        ShadowRenderer shadowRenderer;
        Effect drawModelEffect;
        Texture2D renderTarget;
        Camera.Camera mainCamera;
        IGraphicsDeviceService graphicsService;
        Dictionary<ModelMeshPart, Texture2D> textures;


        public SceneManager(Game game)
            : base(game)
        {
            sceneGraph = new Octree(200f);
            textures = new Dictionary<ModelMeshPart, Texture2D>();
            this.game = game;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            graphicsService = (IGraphicsDeviceService)Game.Services.GetService(typeof(IGraphicsDeviceService));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            shadowRenderer = new ShadowRenderer(game.Content.Load<Effect>("ShadowMap"));
            drawModelEffect = game.Content.Load<Effect>("Simple");

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

            if (sceneGraph.getGeometry() != null)
                DrawModel(sceneGraph.getGeometry());
            DrawScene(sceneGraph.Root);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws a scene
        /// </summary>
        public void DrawScene(Node parent)
        {
            DrawNode(parent);
            if (parent.HasChildren())
            {
                foreach (Node child in parent.Children)
                {
                    if (child.BoundingBox.Intersects(mainCamera.BoundingFrustum))
                    {
                        DrawNode(child);
                        DrawScene(child);
                    }
                }
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
                    DrawModel(model);
                }
            }
        }

        /// <summary>
        /// Draws the completely lit scene with shadows
        /// </summary>
        /// <param name="model"></param>
        public void DrawModel(ModelInfo model)
        {
            Matrix[] modelTransforms = new Matrix[model.Model.Bones.Count];
            model.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Vector3 center = model.Position;

            foreach (ModelMesh mesh in model.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                    part.Effect.Parameters["xTexture"].SetValue(this.Textures[part]);

                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = drawModelEffect.Techniques["Flat"];
                    effect.Parameters["xWorld"].SetValue(modelTransforms[mesh.ParentBone.Index] * model.WorldMatrix);
                    effect.Parameters["xView"].SetValue(mainCamera.View);
                    effect.Parameters["xProjection"].SetValue(mainCamera.Projection);
                    effect.Parameters["xCenter"].SetValue(model.Position);
                    effect.Parameters["xRange"].SetValue(4f);
                }
                mesh.Draw();
            }
        }

        public void DrawFlat(ModelInfo model)
        {
            Matrix[] modelTransforms = new Matrix[model.Model.Bones.Count];
            model.Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Vector3 center = model.Position;

            foreach (ModelMesh mesh in model.Model.Meshes)
            {

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    //Effect e = drawModelEffect.Clone(graphicsService.GraphicsDevice);
                    part.Effect.CurrentTechnique = drawModelEffect.Techniques["Flat"];
                    part.Effect.Parameters["xWorld"].SetValue(modelTransforms[mesh.ParentBone.Index] * model.WorldMatrix);
                    part.Effect.Parameters["xView"].SetValue(mainCamera.View);
                    part.Effect.Parameters["xProjection"].SetValue(mainCamera.Projection);
                    part.Effect.Parameters["xTexture"].SetValue(this.Textures[part]);
                    part.Effect.Parameters["xCenter"].SetValue(model.Position);
                    part.Effect.Parameters["xRange"].SetValue(10f);
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

        public Dictionary<ModelMeshPart, Texture2D> Textures
        {
            get { return textures; }
        }

        public Effect Effect
        {
            get { return this.drawModelEffect; }
        }

        override public String ToString()
        {
            return "" + sceneGraph.getDrawableObjects().Count();
        }

    }
}
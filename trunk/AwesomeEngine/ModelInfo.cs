using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace AwesomeEngine
{
        /// <summary>
    /// This is wrapper for XNA's model class. It provides pertinent information about the
    /// model. For example, its position, rotation, scale, the model itself, the bounding volume,
    /// and a pointer to the node in which in resides.
    /// </summary>
    public class ModelInfo
    {
        Vector3 pos;
        Vector3 rotation;
        Vector3 scale;
        Model model;
        Node node;
        String fileName;
        BoundingSphere boundingSphere;
        Dictionary<ModelMeshPart,Texture2D> textures;

        public delegate void RenderHandler();

        public ModelInfo()
        {
        }
        
        public ModelInfo(Vector3 pos, Vector3 rotation, Vector3 scale, Model model, String fileName)
        {
            this.pos = pos;
            this.rotation = rotation;
            this.scale = scale;
            this.model = model;
            this.fileName = fileName;
            textures = new Dictionary<ModelMeshPart, Texture2D>();
            CreateBoundingSphere(out boundingSphere);
            LoadModelTextures(model);
        }

        public void LoadModelTextures(Model model)
        {
            foreach (ModelMesh mesh in model.Meshes)
                foreach(ModelMeshPart part in mesh.MeshParts)
                {
                    textures.Add(part, (part.Effect as BasicEffect).Texture);
                }
        }

        public void DrawEffect(Effect effect, RenderHandler SetParameter)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            SetParameter.Invoke();
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (textures[part] != null)
                        effect.Parameters["xTexture"].SetValue(textures[part]);
                    part.Effect = effect;
                }
                mesh.Draw();
            }
        }

        public void DrawBasic(Matrix View, Matrix Projection)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * WorldMatrix;
                    effect.EnableDefaultLighting();
                    effect.World = worldMatrix;
                    effect.View = View;
                    effect.TextureEnabled = true;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }
        }

        public void SwitchEffect(Effect effect, GraphicsDevice device)
        {
            foreach(ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect.Clone(device);
                    part.Effect.Parameters["xTexture"].SetValue(textures[part]);
                }
        }

        public void CreateBoundingSphere(out BoundingSphere mergedSphere)
        {
            mergedSphere = new BoundingSphere();
            foreach(ModelMesh mesh in Model.Meshes)
            {
                mergedSphere = BoundingSphere.CreateMerged(mesh.BoundingSphere, mergedSphere);
            }
        }

        public BoundingSphere BoundingSphere
        {
            get{ return boundingSphere.Transform(WorldMatrix); } 
        }

        public Vector3 Position
        {
            get{ return pos; }
            set{ pos = value; }
        }

        public Vector3 Rotation
        {
            get{ return rotation; }
            set{ rotation = value; }
        }

        public Vector3 Scale
        {
            get{ return scale; }
            set{ scale = value; }
        }

        public Matrix WorldMatrix
        {
            get
            {
                return ( Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X)) * 
                    Matrix.CreateRotationY(rotation.Y) *Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z)) * 
                    Matrix.CreateScale(scale.X, scale.Y, scale.Z)*Matrix.CreateTranslation(pos));
            }
        }

        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        public Node Node
        {
            get { return node; }
            set { node = value; }
        }

        public String FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
    }

}

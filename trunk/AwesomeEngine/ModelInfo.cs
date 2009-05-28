using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
//using XNAnimation;

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
            //LoadModelTextures(model);
            
        }

        public static void LoadModel(ref Model model, Dictionary<ModelMeshPart, Texture2D> textures, ContentManager Content, GraphicsDevice graphics, String assetName, Effect effect)
        {
            //Check if model has been loaded already

            model = Content.Load<Model>(@"Models\" + assetName);
            Texture2D test;
            if (!textures.TryGetValue(model.Meshes[0].MeshParts[0], out test))
                foreach (ModelMesh mesh in model.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        Effect e = effect.Clone(graphics);
                        if ((part.Effect as BasicEffect).Texture != null)
                        {

                            textures.Add(part, (part.Effect as BasicEffect).Texture);
                            e.Parameters["xTextureEnabled"].SetValue(true);

                        }
                        else
                        {
                            textures.Add(part, (part.Effect as BasicEffect).Texture);
                            e.Parameters["xTextureEnabled"].SetValue(false);
                        }
                        part.Effect = e;
                    }
            else
            {
                foreach (ModelMesh mesh in model.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        Effect e = effect.Clone(graphics);
                        if (textures[part] == null)
                            e.Parameters["xTextureEnabled"].SetValue(false);
                        else
                            e.Parameters["xTextureEnabled"].SetValue(true);
                        part.Effect = e;
                    }
            }
        }

       /* public static void LoadAnimatedModel(ref SkinnedModel model, Dictionary<ModelMeshPart, Texture2D> textures, ContentManager content, GraphicsDevice graphics, String assetName, Effect effect)
        {
            model = content.Load<SkinnedModel>(@"Models\" + assetName);

            foreach(ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect.Clone(game.GraphicsDevice);
                }
            foreach (ModelMesh mesh in model.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect e = effect.Clone(graphics);
                    if ((part.Effect as BasicEffect).Texture != null)
                    {
                        textures.Add(part, (part.Effect as BasicEffect).Texture);
                        e.Parameters["xTextureEnabled"].SetValue(true);
                    }
                    else
                    {
                        textures.Add(part, (part.Effect as BasicEffect).Texture);
                        e.Parameters["xTextureEnabled"].SetValue(false);
                    }
                    part.Effect = e;
                }
            }
        }*/

        public void CreateBoundingSphere(out BoundingSphere mergedSphere)
        {
            mergedSphere = new BoundingSphere();

            foreach (ModelMesh mesh in model.Meshes)
            {
                BoundingSphere meshBoundingSphere = mesh.BoundingSphere;

                BoundingSphere.CreateMerged(ref mergedSphere,
                                            ref meshBoundingSphere,
                                            out mergedSphere);
            }

        }

        public void UpdateBoundingSphere()
        {
            CreateBoundingSphere(out boundingSphere);
        }

        public BoundingSphere BoundingSphere
        {
            get
            {
                return boundingSphere.Transform(WorldMatrix);
            }
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

        public Dictionary<ModelMeshPart, Texture2D> Textures
        {
            get { return textures; }
        }
    }

}

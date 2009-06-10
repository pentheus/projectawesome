using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using XNAnimation;
using JigLibX.Math;
using JigLibX.Physics;
using JigLibX.Geometry;
using JigLibX.Collision;
using XNAnimation;
using XNAnimation.Effects;


namespace AwesomeEngine
{
        /// <summary>
    /// This is wrapper for XNA's model class. It provides pertinent information about the
    /// model. For example, its position, rotation, scale, the model itself, the bounding volume,
    /// and a pointer to the node in which in resides.
    /// </summary>
    public class ModelInfo
    {
        protected Vector3 pos;
        protected Vector3 rotation;
        protected Vector3 scale;
        protected float bSphereRadius;
        protected float bSphereScale = 1f;
        protected Model model;
        protected Node node;
        protected String fileName;
        protected BoundingSphere boundingSphere;
        protected Dictionary<ModelMeshPart, Texture2D> textures;

        //Collision Variables
        protected Body body;
        protected CollisionSkin skin;
        protected TriangleMesh triangleMesh;

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
            bSphereRadius = boundingSphere.Radius;

            //Collision parts
            body = new Body();
            skin = new CollisionSkin(body);
            body.CollisionSkin = skin;
            triangleMesh = new TriangleMesh();

            List<Vector3> vertexList = new List<Vector3>();
            List<TriangleVertexIndices> indexList = new List<TriangleVertexIndices>();
            ExtractData(vertexList, indexList, model);
            triangleMesh.CreateMesh(vertexList, indexList, 4, 1.0f);
            skin.AddPrimitive(triangleMesh, new MaterialProperties(0f, 0f, 0f));
            /*
            Sphere spheremesh = new Sphere(new Vector3(0, 12, 0), 15);
            skin.AddPrimitive(spheremesh, new MaterialProperties(1.0f, 1.0f, 1.0f));
             * */

            Vector3 com = SetMass(100.0f);
            body.MoveTo(pos, Matrix.Identity);
            skin.ApplyLocalTransform(new Transform(-com, Matrix.Identity));
            body.Immovable = true;
            body.EnableBody();
            body.Immovable = true;
        }

        public static void LoadModel(ref Model model, Dictionary<ModelMeshPart, Texture2D> textures, ContentManager Content, GraphicsDevice graphics, String assetName, Effect effect)
        {
            //Check if model has been loaded already
            
            model = Content.Load<Model>(@"Models\"+assetName);
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
                        e.Parameters["xDiffuseColor"].SetValue(new Vector4((part.Effect as BasicEffect).DiffuseColor,1f));
                        part.Effect = e;
                    }
            else
            {
                foreach(ModelMesh mesh in model.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        Effect e = effect.Clone(graphics);
                        if (textures[part] == null)
                            e.Parameters["xTextureEnabled"].SetValue(false);
                        else
                            e.Parameters["xTextureEnabled"].SetValue(true);
                        e.Parameters["xDiffuseColor"].SetValue(new Vector4(1f,0,0,1));
                        part.Effect = e;
                    }
            }

        }

        public static void LoadModel(ref SkinnedModel model, Dictionary<ModelMeshPart, Texture2D> textures, ContentManager content, GraphicsDevice graphics,
            String assetName, Effect effect)
        {
            model = content.Load<SkinnedModel>(@"Models\" + assetName);

            foreach (ModelMesh mesh in model.Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect e = effect.Clone(graphics);
                    if ((part.Effect as SkinnedModelBasicEffect).DiffuseMap != null)
                    {
                        textures.Add(part, (part.Effect as SkinnedModelBasicEffect).DiffuseMap);
                        e.Parameters["xTextureEnabled"].SetValue(true);
                    }
                    else
                    {
                        textures.Add(part, (part.Effect as SkinnedModelBasicEffect).DiffuseMap);
                        e.Parameters["xTextureEnabled"].SetValue(false);
                    }
                    e.Parameters["xDiffuseColor"].SetValue(new Vector4((part.Effect as SkinnedModelBasicEffect).Material.DiffuseColor,1f));
                    part.Effect = e;
                }
        }

        public void EnableBody(bool isEnabled)
        {
            if (isEnabled)
                body.EnableBody();
            else
                body.DisableBody();
        }

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
                boundingSphere.Radius = bSphereRadius * bSphereScale;
                return boundingSphere.Transform(WorldMatrix);
            }
            set { boundingSphere = value; }
        }

        public Vector3 Position
        {
            get{ return pos; }
            set
            { 
                pos = value;
                body.MoveTo(value, body.Orientation);
            }
        }

        public Vector3 Rotation
        {
            get{ return rotation; }
            set{ 
                   rotation = value;
                   body.Orientation = Matrix.CreateScale(scale) * Matrix.CreateRotationX(MathHelper.ToRadians(value.X))
                                    * Matrix.CreateRotationY(MathHelper.ToRadians(value.Y))
                                    * Matrix.CreateRotationZ(MathHelper.ToRadians(value.Z));
               }
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
                return
            Matrix.CreateScale(scale) *
            skin.GetPrimitiveLocal(0).Transform.Orientation *
            body.Orientation *
            Matrix.CreateTranslation(body.Position);
            }
        }

        public float BSphereScale
        {
            get { return bSphereScale; }
            set { bSphereScale = value; }
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

        public Body Body
        {
            get
            {
                return body;
            }
        }

        public CollisionSkin Skin
        {
            get
            {
                return skin;
            }
        }

        protected Vector3 SetMass(float mass)
        {
            PrimitiveProperties primitiveProperties =
                new PrimitiveProperties(PrimitiveProperties.MassDistributionEnum.Solid, PrimitiveProperties.MassTypeEnum.Density, mass);

            float junk; Vector3 com; Matrix it, itCoM;

            skin.GetMassProperties(primitiveProperties, out junk, out com, out it, out itCoM);
            body.BodyInertia = itCoM;
            body.Mass = junk;

            return com;
        }

        public void ExtractData(List<Vector3> vertices, List<TriangleVertexIndices> indices, Model model)
        {
            Matrix[] bones_ = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(bones_);
            foreach (ModelMesh mm in model.Meshes)
            {
                Matrix xform = bones_[mm.ParentBone.Index];
                xform *= Matrix.CreateScale(scale);
                foreach (ModelMeshPart mmp in mm.MeshParts)
                {
                    int offset = vertices.Count;
                    Vector3[] a = new Vector3[mmp.NumVertices];
                    mm.VertexBuffer.GetData<Vector3>(mmp.StreamOffset + mmp.BaseVertex * mmp.VertexStride,
                        a, 0, mmp.NumVertices, mmp.VertexStride);
                    for (int i = 0; i != a.Length; ++i)
                        Vector3.Transform(ref a[i], ref xform, out a[i]);
                    vertices.AddRange(a);

                    if (mm.IndexBuffer.IndexElementSize != IndexElementSize.SixteenBits)
                        throw new Exception(
                            String.Format("Model uses 32-bit indices, which are not supported."));
                    short[] s = new short[mmp.PrimitiveCount * 3];
                    mm.IndexBuffer.GetData<short>(mmp.StartIndex * 2, s, 0, mmp.PrimitiveCount * 3);
                    JigLibX.Geometry.TriangleVertexIndices[] tvi = new JigLibX.Geometry.TriangleVertexIndices[mmp.PrimitiveCount];
                    for (int i = 0; i != tvi.Length; ++i)
                    {
                        tvi[i].I0 = s[i * 3 + 2] + offset;
                        tvi[i].I1 = s[i * 3 + 1] + offset;
                        tvi[i].I2 = s[i * 3 + 0] + offset;
                    }
                    indices.AddRange(tvi);
                }
            }
        }
    }

}

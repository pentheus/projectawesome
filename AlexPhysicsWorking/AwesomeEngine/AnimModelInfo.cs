using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using XNAnimation;
using XNAnimation.Pipeline;
using XNAnimation.Controllers;
using JigLibX.Physics;
using JigLibX.Collision;
using JigLibX.Geometry;

namespace AwesomeEngine
{
    public class AnimModelInfo:ModelInfo
    {
        SkinnedModel animatedModel;
        AnimationController animationController; 
        
        /*
        public AnimModelInfo()
        {

        }
         * */
        
        public AnimModelInfo(Vector3 pos, Vector3 rotation, Vector3 scale, SkinnedModel mod, String fileName, Game game):
            base(pos, rotation, scale, mod.Model, fileName, game)
        {
            animatedModel = mod;
            animationController = new AnimationController(animatedModel.SkeletonBones);
            animationController.StartClip(animatedModel.AnimationClips["Idle"]);

            //Collision parts
            body = new Body();
            skin = new CollisionSkin(null);
            skin.AddPrimitive(new Sphere(Vector3.Zero * 5.0f, 45), new MaterialProperties(0.5f, 0.7f, 0.6f));
            body.CollisionSkin = this.skin;
            Vector3 com = SetMass(10.0f);
            body.MoveTo(pos + com, WorldMatrix);
            // collision.ApplyLocalTransform(new Transform(-com, Matrix.Identity));
            body.EnableBody();
            this.scale = Vector3.One * bSphereRadius;
            PhysicsSystem.CurrentPhysicsSystem.CollisionSystem.AddCollisionSkin(skin);
            body.EnableBody();
        }

        public new void CreateBoundingSphere(out BoundingSphere mergedSphere)
        {
            mergedSphere = new BoundingSphere();
            foreach(ModelMesh mesh in AnimatedModel.Model.Meshes)
            {
                mergedSphere = BoundingSphere.CreateMerged(mesh.BoundingSphere, mergedSphere);
            }
        }

        public SkinnedModel AnimatedModel
        {
            get { return animatedModel; }
            set { animatedModel = value; }
        }

        public AnimationController AnimationController
        {
            get { return animationController; }
            set { animationController = value; }
        }

        public void animateModel(String animation)
        {
            animationController.CrossFade(animatedModel.AnimationClips[animation], TimeSpan.FromSeconds(0.3f));
        }
    }
}

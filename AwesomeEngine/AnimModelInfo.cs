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

namespace AwesomeEngine
{
    public class AnimModelInfo:ModelInfo
    {
        SkinnedModel animatedModel;
        AnimationController animationController;

        public AnimModelInfo()
        {

        }
        
        public AnimModelInfo(Vector3 pos, Vector3 rotation, Vector3 scale, SkinnedModel mod, String fileName)
        {
            Position = pos;
            Rotation = rotation;
            Scale = scale;
            animatedModel = mod;
            FileName = fileName;
            animationController = new AnimationController(animatedModel.SkeletonBones);
            Model = animatedModel.Model;
            animationController.StartClip(animatedModel.AnimationClips["Idle"]);
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

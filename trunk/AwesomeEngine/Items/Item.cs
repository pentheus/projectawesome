using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine
{
    public class Item:GameComponent
    {
        ModelInfo model;
        private bool movable, pickable, picked;
        BoundingSphere itemAOE; // bounding sphere for the object. when intersected, it is either picked up or pushed
        int radius; // INITIALIZE THIS FIRST

        public Item(Game game, ModelInfo model) // initializing movable, pickable, picked as false
            : base(game)
        {
            this.model = model;
            movable = false;
            pickable = false;
            picked = false;
            //itemAOE = new BoundingSphere(model.pos, radius); // CHECK IF RADIUS IS INITIALIZED
        }

        public ModelInfo Model
        {
            get { return model; }
            set { model = value; }
        }

        public void setMovable(bool m)
        {
            movable = m;
        }

        public void setPickable(bool p)
        {
            pickable = p;
        }

        public void runScript()
        {
            //if bounding box of item intersects bounding box of character, pick up/move if movable is true
        }

        public BoundingSphere BoundingSphere
        {
            get { return itemAOE.Transform(WorldMatrix); }
        }

        public Matrix WorldMatrix
        {
            get
            {
                return (Matrix.CreateRotationX(MathHelper.ToRadians(model.Rotation.X)) *
                    Matrix.CreateRotationY(Model.Rotation.Y) * Matrix.CreateRotationZ(MathHelper.ToRadians(Model.Rotation.Z)) *
                    Matrix.CreateScale(Model.Scale.X, Model.Scale.Y, Model.Scale.Z) * Matrix.CreateTranslation(Model.Position));
            }
        }
    }
}
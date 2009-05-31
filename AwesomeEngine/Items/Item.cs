using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine
{
    public abstract class Item:GameComponent
    {
        public ModelInfo model;
        public bool movable, pickable, picked;
        public BoundingSphere itemAOE; // bounding sphere for the object. when intersected, it is either picked up or pushed
        private int radius; // INITIALIZE THIS FIRST

        public Item(Game game, ModelInfo model) // initializing movable, pickable, picked as false
            : base(game)
        {
            this.model = model;
            movable = false;
            pickable = false;
            picked = false;
            radius = 0; // initialized to zero, change it in each of the items classes
            itemAOE = new BoundingSphere(this.model.Position, radius); // must initialize radius in each items class
        }

        public void updateAOE(int rad)
        {
            radius = rad;
            itemAOE = new BoundingSphere(model.Position, radius);
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

        public abstract void runScript();
        


        public BoundingSphere BoundingSphere
        {
            get { return itemAOE.Transform(WorldMatrix); }
        }

        public Matrix WorldMatrix
        {
            get
            {
                return (Matrix.CreateRotationX(MathHelper.ToRadians(model.Rotation.X)) *
                    Matrix.CreateRotationY(model.Rotation.Y) * Matrix.CreateRotationZ(MathHelper.ToRadians(model.Rotation.Z)) *
                    Matrix.CreateScale(model.Scale.X, model.Scale.Y, model.Scale.Z) * Matrix.CreateTranslation(model.Position));
            }
        }
    }
}

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
        ContainsScene afgame;
        public ModelInfo model;
        public bool movable, pickable, picked;
        public BoundingSphere itemAOE; // bounding sphere for the object. when intersected, it is either picked up or pushed
        private int radius; // INITIALIZE THIS FIRST
        Player player;

        public Item(Game game, ModelInfo model) // initializing movable, pickable, picked as false
            : base(game)
        {
            afgame = (ContainsScene)game;
            player = afgame.GetPlayer();
            this.model = model;
            this.model.Body.DisableBody();
            movable = false;
            pickable = true;
            picked = false;
            radius = 3; // initialized to zero, change it in each of the items classes
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

        public override void Update(GameTime gameTime)
        {
            if (pickable && !picked)
            {
                if (itemAOE.Intersects(((ContainsScene)Game).GetPlayer().ItemSphere))
                {
                    runScript();
                }
            }
        }

        public Player Player
        {
            get { return player; }
        }

        public BoundingSphere BoundingSphere
        {
            get { return itemAOE.Transform(WorldMatrix); }
            set { model.BoundingSphere = value; }
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

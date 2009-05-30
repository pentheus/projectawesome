using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeEngine
{
    public abstract class LogicEntity:GameComponent
    {
        /*Create LogicEntity class - Contains Vector3 position, Vector3 scale, model

        Logic Entity subclasses:
        -SpawnEntity
        -EnemySpawnEntity
        -TriggerEntity

        Create list of LogicEntity objects in nodes

        Create getEntities method in in octree and nodes
        Create addEntity method to octree and node      
        -Basically do just like adding and getting items, but with LogicEntities*/

        private Vector3 position;
        private Vector3 scale;
        private Model model;
        private BoundingSphere boundingSphere;

        public LogicEntity(Game game, Model model, Vector3 position, Vector3 scale) : base(game)
        {
            this.position = position;
            this.scale = scale;
            this.model = model;
            this.boundingSphere = new BoundingSphere(position, 5f);
        }

        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
            set { boundingSphere = value; }
        }
    }
}

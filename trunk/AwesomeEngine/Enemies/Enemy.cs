using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine
{
    public class Enemy:GameComponent
    {
        enum state { Idle, Seeking, Attacking, Damaged };

        ModelInfo model;
        SceneManager scene;
        state currentstate;

        public Enemy(Game game, SceneManager scene, ModelInfo model): base(game)
        {
            this.scene = scene;
            this.model = model;
            currentstate = state.Idle;
        }

        public ModelInfo Model
        {
            get { return model; }
            set { model = value; }
        }

        public void Act()
        {

        }

        public void MoveTowards(Vector3 pos)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeEngine
{
    class Enemy
    {
        enum state { Idle, Seeking, Attacking, Damaged };

        ModelInfo model;
        SceneManager scene;
        state currentstate;

        public Enemy(SceneManager scene, ModelInfo model)
        {
            this.scene = scene;
            this.model = model;
            currentstate = state.Idle;
        }

        public void Act()
        {

        }
    }
}

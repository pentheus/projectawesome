using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;


namespace AwesomeEngine.Enemies
{
    public class FirstBossEnemy:Enemy
    {
        public FirstBossEnemy(Vector3 playerPOS, SceneManager scene, AnimModelInfo model): 
            base(game, scene, model)
        {

        }
        

        public override void ActIdle()        {        }        public override void ActSeeking()        {        }        public override void ActAttacking()        {        }        public override void ActDamaged()        {        }
    }
}

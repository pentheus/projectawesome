using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeEngine
{
    class Enemy
    {
        String type;

        public Enemy(String enemytype)
        {
            type = enemytype;
        }

        public string Type
        {
            get { return type; }
        }

        public void runScript(ScriptDelegate function)
        {

        }
    }
}

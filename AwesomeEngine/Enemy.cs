using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeEngine
{
    class Enemy:ScriptClass
    {
        ModelInfo model;

        public Enemy(types settotype, ModelInfo model)
        {
            this.thisType = settotype;
            this.model = model;
        }

        public void runScript(ScriptDelegate function)
        {

        }
    }
}

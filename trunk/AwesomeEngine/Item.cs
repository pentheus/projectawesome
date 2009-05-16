using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeEngine
{
    class Item:ScriptClass
    {
        ModelInfo model;

        public Item(types settotype, ModelInfo model)
        {
            this.thisType = settotype;
            this.model = model;
        }

        public void runScript(ScriptDelegate function)
        {

        }
}

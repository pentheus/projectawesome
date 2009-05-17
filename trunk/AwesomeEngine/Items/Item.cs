<<<<<<< .mine
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine.Items
{
    public class Item:GameComponent
    {
        ModelInfo model;

        public Item(Game game, ModelInfo model):base(game)
        {
            this.model = model;
        }

        public ModelInfo Model
        {
            get { return model; }
            set { model = value; }
        }

        public void runScript()
        {

        }
    }
}
=======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace AwesomeEngine.Items
{
    class Item:GameComponent
    {
        ModelInfo model;

        public Item(Game game, ModelInfo model):base(game)
        {
            this.model = model;
        }

        public ModelInfo Model
        {
            get { return model; }
            set { model = value; }
        }

        public void runScript()
        {

        }
    }
}
>>>>>>> .r105

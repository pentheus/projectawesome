using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeEngine
{
    public delegate void ScriptDelegate(ScriptClass target, SceneManager sceneMgr);

    public class Scripts
    {
        ScriptDelegate pickUp, push;

        public Scripts(SceneManager sceneManager)
        {
            
        }


        public static void pickUp(Item item)
        {

        }

        public static void push(Item item)
        {

        }
    }
}

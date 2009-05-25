using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeEngine
{
    abstract class ScriptClass
    {
        public enum types { Shadow, Lantern, Flashlight, Battery, Fuse, Chair, GlowStick };

        protected types thisType;

        public types Type
        {
            get { return thisType; }
            set { thisType = value; }
        }
    }
}

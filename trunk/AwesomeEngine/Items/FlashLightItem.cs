using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AwesomeEngine.Items
{
    public class FlashLightItem : Item
    {
        private int dps, battLife;
        private LightShaft light;
        private int battdraincounter = 1000;
        Game game;
        public bool firing = false;

        public FlashLightItem(Game game, ModelInfo model) :
            base(game, model)
        {
            this.setPickable(true);
            dps = 3; // medium damage per second
            battLife = 10; // initialized flashlight battery life to 10
            this.game = game;
        }

        public override void Initialize()
        {
            light = new LightShaft(game);
            game.Components.Add(light);
            base.Initialize();
        }
        // scripts
        // if not picked up, if bounding spheres intersect, pick up
        // if picked up, turn on, turn off
        public override void runScript()
        {
            this.Player.Pickup(this);
            this.picked = true;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState(PlayerIndex.One);
            if (currentState.IsKeyDown(Keys.Space))
            {
                //Fire the flashlight
                if (battdraincounter <= 0)
                {
                    //Drain the battery
                    battLife -= 1;
                    battdraincounter = 1000;
                }
                else
                {
                    battdraincounter -= 1;
                }
                firing = true;
            }
            else
                firing = false;
            base.Update(gameTime);
        }

        public LightShaft Light
        {
            get { return light; }
        }

        public int Damage
        {
            get { return dps; }
        }

        public int BatteryLife
        {
            get { return battLife; }
            set { battLife = value; }
        }
    }
}

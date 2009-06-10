using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace AfterDarkGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameUI : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont font;
        AfterDarkGame game;

        public GameUI(Game game)
            : base(game)
        {
            this.game = game;
        }



        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("Arial");
            spriteBatch = new SpriteBatch(GraphicsDevice);
         
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawText();
            base.Draw(gameTime);
        }

        private void DrawText()
        {
            string text = "Player Health: " + game.Player.Health + "    Battery Life: " + game.Player.Flashlight.BatteryLife;

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            spriteBatch.DrawString(spriteFont, text, Vector2(10,10), Color.Yellow);
            spriteBatch.End();
        }
    }
}
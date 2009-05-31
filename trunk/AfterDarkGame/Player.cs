using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using AwesomeEngine;
using AwesomeEngine.Items;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeEngine
{
    class Player:DrawableGameComponent
    {
        AfterDarkGame.AfterDarkGame game;
        ModelInfo model;
        BoundingSphere boundary;
        int health;
        List<Item> inventory;
        FlashLightItem flashlight;
        Item currentitem;

        public Player(Game game):
            base(game)
        {
            this.game = (AfterDarkGame.AfterDarkGame)game;
            LoadContent();
            health = 3;
            inventory = new List<Item>();
            flashlight = null;
        }

        public Player(Game game, List<Item> inv, FlashLightItem light):
            base(game)
        {
            this.game = (AfterDarkGame.AfterDarkGame)game;
            LoadContent();
            health = 3;
            inventory = inv;
            flashlight = light;
        }

        public override void LoadContent()
        {
            Model playermodel = new Model();
            ModelInfo.LoadModel(ref playermodel, game.GetScene().Textures, game.GetContent(), game.GetGraphics(), "Player", game.GetScene().Effect);
        }
    }
}

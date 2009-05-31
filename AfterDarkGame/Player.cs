using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using AwesomeEngine;
using AwesomeEngine.Items;
using Microsoft.Xna.Framework.Graphics;
using XNAnimation;

namespace AfterDarkGame
{
    public class Player:DrawableGameComponent
    {
        AfterDarkGame game;
        AnimModelInfo model;
        BoundingSphere boundary;
        int health;
        List<Item> inventory;
        FlashLightItem flashlight;
        Item currentitem;

        public Player(Game game):
            base(game)
        {
            this.game = (AfterDarkGame)game;
            LoadContent();
            health = 3;
            inventory = new List<Item>();
            flashlight = null;
        }

        public Player(Game game, List<Item> inv, FlashLightItem light):
            base(game)
        {
            this.game = (AfterDarkGame)game;
            LoadContent();
            health = 3;
            inventory = inv;
            flashlight = light;
        }

        public void LoadContent()
        {
            SkinnedModel playermodel = new SkinnedModel();
            ModelInfo.LoadModel(ref playermodel, game.GetScene().Textures, game.GetContent(), game.GetGraphics(), "Player", game.GetScene().Effect);
            model = new AnimModelInfo(Vector3.Zero, Vector3.Zero, Vector3.Zero, playermodel, "Player");
        }

        public void Update()
        {

        }

        public void Draw()
        {

        }
    }
}

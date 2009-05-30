﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using AwesomeEngine;
using AwesomeEngine.Items

namespace AwesomeEngine
{
    class Player:DrawableGameComponent
    {
        Game game;
        ModelInfo model;
        BoundingSphere boundary;
        int health;
        List<Item> inventory;
        FlashLightItem flashlight;
        Item currentitem;

        public Player(Game game):
            base(game)
        {
            this.game = game;
            model = mod;
            health = 3;
            inventory = new List<Item>();
            flashlight = null;
        }

        public Player(Game game, List<Item> inv, FlashLightItem light):
            base(game)
        {
            this.game = game;
            model = mod;
            health = 3;
            inventory = inv;
            flashlight = light;
        }

        public override void LoadContent()
        {
            ModelInfo.LoadModel(ref model, game.Get

    }
}

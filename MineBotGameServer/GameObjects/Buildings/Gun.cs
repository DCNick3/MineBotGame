﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects.Buildings
{
    public class Gun : Building
    {
        public Gun() : base(new Vector2(2, 2))
        {
            _hp = 1000;
            _def = 10;
            _energy = 10;
            availableLResearches = LocalResearch.None;
            onAdd = (p) =>
            {
                p.EnergyGeneration += 50.0;
                p.ResourceLimits.AddAll(200);
            };
            onRemove = (p) =>
            {
                p.EnergyGeneration -= 40.0;
                p.ResourceLimits.AddAll(-200);
            };
        }
        public Gun(Player ownerPlayer, int id, Vector2 size) : base(ownerPlayer, id, new Vector2(2, 2), size)
        {
            _hp = 250;
            _def = 10;
            _energy = 15;
            availableLResearches = LocalResearch.TestResearch;
            onAdd = (p) => { };
            onRemove = (p) => { };
        }
    }
}
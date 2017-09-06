using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects.Buildings
{
    public class Generator : Building
    {
        public Generator() : base(new Vector2(2, 2))
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
        public Generator(Player ownerPlayer, int id, Vector2 size) : base(ownerPlayer, id, new Vector2(2, 2), size)
        {
            _hp = 100;
            _def = 5;
            _energy = 0;
            availableLResearches = LocalResearch.MoreEnergy;
            onAdd = (p) =>
            {
                p.EnergyGeneration += 30.0;
            };
            onRemove = (p) =>
            {
                p.EnergyGeneration -= 30.0;
            };
        }
    }
}

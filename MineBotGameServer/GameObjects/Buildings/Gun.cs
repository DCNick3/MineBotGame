using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects.Buildings
{
    public class Gun : Building
    {
        private static readonly int maxHP = 250;
        private static readonly int startDefence = 10;
        private static readonly int startEnergyConsumation = 15;
        private static readonly Vector2 size = new Vector2(2, 2);
        private static new readonly LocalResearch availableLResearches = LocalResearch.None;
        private static new readonly GlobalResearch availableGResearches = GlobalResearch.None;
        private static new readonly BuildingOperationType availableOperations = BuildingOperationType.None;
        public Gun(Player ownerPlayer, int id, Vector2 pos) : base(ownerPlayer, id, pos, size,maxHP,startDefence,startEnergyConsumation, availableOperations, availableGResearches, availableLResearches)
        {
            onAdd = (p) => { };
            onRemove = (p) => { };
        }
        public override void Update()
        {

        }
    }
}

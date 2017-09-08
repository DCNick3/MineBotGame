using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects.Buildings
{
    public class Generator : Building
    {
        private static readonly int maxHP = 100;
        private static readonly int startDefence = 5;
        private static readonly int startEnergyConsumation = 10;
        private static readonly Vector2 size = new Vector2(2,2);
        private static new readonly LocalResearch availableLResearches = LocalResearch.MoreEnergy;
        private static new readonly GlobalResearch availableGResearches = GlobalResearch.None;
        private static new readonly BuildingOperationType availableOperations = BuildingOperationType.DoLocalResearch;
        public Generator(Player ownerPlayer, int id, Vector2 pos) : base(ownerPlayer, id, pos, size,maxHP,startDefence,startEnergyConsumation, availableOperations, availableGResearches, availableLResearches)
        {
            onAdd = (p) =>
            {
                p.EnergyGeneration += 40.0;
            };
            onRemove = (p) =>
            {
                p.EnergyGeneration -= 40.0;
            };

        }
        public override void Update()
        {
            if (OperationQueue.Count != 0)
            {
                var op = OperationQueue.First();
                op.Done++;
                if (op.Done == op.NeedDone)
                {
                    Dequeue();
                    FinalizeOperation(op);
                }
            }
        }
    }
}

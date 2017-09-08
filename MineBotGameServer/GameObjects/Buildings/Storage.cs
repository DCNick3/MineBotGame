using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects.Buildings
{
    public class Storage : Building
    {
        private static readonly int maxHP = 200;
        private static readonly int startDefence = 5;
        private static readonly int startEnergyConsumation = 5;
        private static readonly Vector2 size = new Vector2(3, 3);
        private static new readonly LocalResearch availableLResearches = LocalResearch.MoreStorage;
        private static new readonly GlobalResearch availableGResearches = GlobalResearch.None;
        private static new readonly BuildingOperationType availableOperations = BuildingOperationType.DoLocalResearch;
        public Storage(Player ownerPlayer, int id, Vector2 pos) : base(ownerPlayer, id, pos, size,maxHP,startDefence,startEnergyConsumation, availableOperations, availableGResearches, availableLResearches)
        {
            onAdd = (p) =>
            {
                p.ResourceLimits.AddAll(100);
            };
            onRemove = (p) =>
            {
                p.ResourceLimits.AddAll(-100);
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

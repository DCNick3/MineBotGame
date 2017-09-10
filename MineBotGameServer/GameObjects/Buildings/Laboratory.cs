using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects.Buildings
{
    public class Laboratory : Building
    {
        private static readonly int maxHP = 400;
        private static readonly int startDefence = 5;
        private static readonly int startEnergyConsumation = 40;
        private static readonly Vector2 size = new Vector2(3, 3);
        private static new readonly LocalResearch availableLResearches = LocalResearch.None;
        private static new readonly GlobalResearch availableGResearches = GlobalResearch.All;
        private static new readonly BuildingOperationType availableOperations = BuildingOperationType.DoGlobalResearch;
        public Laboratory(Player ownerPlayer, int id, Vector2 pos) : base(ownerPlayer, id, pos, size,maxHP,startDefence,startEnergyConsumation, availableOperations, availableGResearches, availableLResearches)
        {
            onAdd = (p) => { };
            onRemove = (p) => { };
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

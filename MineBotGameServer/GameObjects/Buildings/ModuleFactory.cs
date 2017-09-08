using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects.Buildings
{
    public class ModuleFactory : Building
    {
        private static readonly int maxHP = 800;
        private static readonly int startDefence = 10;
        private static readonly int startEnergyConsumation = 30;
        private static readonly Vector2 size = new Vector2(4, 4);
        private static new readonly LocalResearch availableLResearches = LocalResearch.None;
        private static new readonly GlobalResearch availableGResearches = GlobalResearch.None;
        private static new readonly BuildingOperationType availableOperations = BuildingOperationType.NewModule|BuildingOperationType.NewUpgrade;
        public ModuleFactory(Player ownerPlayer, int id, Vector2 pos) : base(ownerPlayer, id, pos, size,maxHP,startDefence,startEnergyConsumation, availableOperations, availableGResearches, availableLResearches)
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

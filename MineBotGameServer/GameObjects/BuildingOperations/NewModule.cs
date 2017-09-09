using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects
{
    class NewModule : BuildingOperation
    {
        static readonly int needDone = 2;
        static readonly int type = 8;
        static readonly int startEnergyCostumation = 3;
        private Unit unit;
        private UnitModule module;
        public NewModule(Building building, Unit unit,UnitModule module) : base(needDone,type,new ResourceTotality(new int[] { 5, 0, 0, 0 }), startEnergyCostumation, building)
        {
            this.unit = unit;
            this.module = module;
            StartOperation();
        }
        public override bool CanBeDone()
        {
            if (base.CanBeDone())
            {
                if (building.GetNearFreeSpace().Contains(unit.Position.Integerize()) && unit.CanPlaceModule(module))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public override void StartOperation()
        {
            base.StartOperation();
            unit.Paralizers.Add(building);
        }
        public override void FinalizeOperation()
        {
            unit.Paralizers.Remove(building);
        }
    }
}

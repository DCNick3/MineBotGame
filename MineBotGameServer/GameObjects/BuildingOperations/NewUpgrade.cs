using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;


namespace MineBotGame.GameObjects
{
    class NewUpgrade : BuildingOperation
    {
        static readonly int needDone = 2;
        static readonly int type = 4;
        static readonly int startEnergyCostumation = 3;
        public Unit unit;
        public int upgrate;
        public NewUpgrade(Building building,Unit unit,int upgrate) : base(needDone, type, new ResourceTotality(new int[] { 5, 0, 0, 0 }), startEnergyCostumation, building)
        {
            this.unit = unit;
            this.upgrate = upgrate;
            StartOperation();
        }
        public override bool CanBeDone()
        {
            if (base.CanBeDone())
            {
                if (unit.CanUpgrade(upgrate) && building.GetNearFreeSpace().Contains(unit.Position))
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
            unit.SetUpgrade(upgrate, unit.GetUpgrade(upgrate) + 1);
        }
    }
}

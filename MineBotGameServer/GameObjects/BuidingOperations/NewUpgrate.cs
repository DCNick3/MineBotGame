using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;


namespace MineBotGame.GameObjects.BuidingOperations
{
    class NewUpgrate : BuildingOperation
    {
        static readonly int needDone = 2;
        static readonly int type = 4;
        static readonly int startEnergyCostumation = 3;
        public Unit unit;
        public int upgrate;
        public NewUpgrate(Building building,Unit unit,int upgrate) : base(needDone, type, new ResourceTotality(new int[] { 5, 0, 0, 0 }), startEnergyCostumation, building)
        {
            this.unit = unit;
            this.upgrate = upgrate;
        }
        public override bool CanBeDone()
        {
            if (unit.CanUpgrade(upgrate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void FinalizeOperation()
        {
            unit.SetUpgrade(upgrate, unit.GetUpgrade(upgrate) + 1);
        }
    }
}

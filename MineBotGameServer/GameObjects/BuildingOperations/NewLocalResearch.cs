using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame.GameObjects
{
    class NewLocalResearch : BuildingOperation
    {
        static readonly int needDone = 2;
        static readonly int type = 8;
        static readonly int startEnergyCostumation = 3;
        private LocalResearch research;
        public NewLocalResearch(Building building,  LocalResearch research) : base(needDone,type,new ResourceTotality(new int[] { 5, 0, 0, 0 }), startEnergyCostumation, building)
        {
            this.research = research;
            StartOperation();
        }
        public override bool CanBeDone()
        {
            if (base.CanBeDone())
            {
                if (building.CanDoLocalResearche(research))
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
        public override void FinalizeOperation()
        {
            building.LResearches |= research;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame.GameObjects
{
    class NewGlobalResearch : BuildingOperation
    {
        static readonly int needDone = 20;
        static readonly int type = 16;
        static readonly int startEnergyCostumation = 3;
        private GlobalResearch research;
        public NewGlobalResearch(Building building, GlobalResearch research) : base(needDone,type,new ResourceTotality(new int[] { 5, 0, 0, 0 }), startEnergyCostumation, building)
        {
            this.research = research;
            StartOperation();
        }
        public override bool CanBeDone()
        {
            if (base.CanBeDone())
            {
                if ((GameObject.globalResearches & research) == 0)
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
            GameObject.globalResearches |= research;
        }
    }
}

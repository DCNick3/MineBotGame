using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame.GameObjects
{
    class UnitOperation
    {
        public UnitOperation(Unit unit)
        {
            this.unit = unit;
        }
        public int Done { get; set; }
        public int NeedDone { get; set; }
        public virtual void FinalizeOperation()
        {

        }
        public virtual void StartOperation()
        {

        }
        public virtual void DoneAdd()
        {
            Done++;
        }
        private Unit unit;
    }
}

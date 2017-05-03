using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame.GameObjects
{
    public class BuildingOperation
    {
        private BuildingOperation()
        { }
        
        public int Done { get; set; }
        public int NeedDone { get; private set; }
    }
}

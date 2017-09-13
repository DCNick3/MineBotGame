using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame.GameObjects
{
    class BuildStart : UnitOperation
    {
        public BuildStart(Unit unit,BuildingType building, GameObjectPos pos) : base(unit)
        {

        }
    }
}

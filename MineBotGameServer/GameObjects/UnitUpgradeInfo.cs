using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineBotGame;

namespace MineBotGame.GameObjects
{
    public static class UnitUpgradeInfo
    {
        static UnitUpgradeInfo()
        {
            dat = new UnitStats[Enum.GetValues(typeof(UnitUpgrade)).Length];

        }

        private static UnitStats[] dat;

        private static void Set(UnitUpgrade up, UnitStats v)
        {
            dat[(int)up] = v;
        }

        public static UnitStats Get(UnitUpgrade up)
        {
            return dat[(int)up];
        }
    }
}

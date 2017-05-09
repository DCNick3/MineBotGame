using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame.GameObjects
{
    public class Unit : GameObject
    {
        private Unit(Vector2 pos, Player ownerPlayer, int id) : base(ownerPlayer, id, new Vector2(1,1))
        {
            _pos = pos;
            unitModules = new bool[Enum.GetValues(typeof(UnitModule)).Length];
            unitUpgrades = new int[Enum.GetValues(typeof(UnitUpgrade)).Length];
        }

        bool[] unitModules;
        int[] unitUpgrades;

        private UnitStats stats;
        
        private Vector2 _pos;

        public override double HP
        {
            get
            {
                return stats[UnitStatType.HP];
            }
        }
        public override double Defence
        {
            get
            {
                return stats[UnitStatType.Defence];
            }
        }
        public override double EnergyConsumation
        {
            get
            {
                return stats.OverallEnergy;
            }
        }
        public override double ScoutRange
        {
            get
            {
                return stats[UnitStatType.ScoutRange];
            }
        }

        public override Vector2 Position
        {
            get
            {
                return _pos;
            }
        }

        public void SetModule(UnitModule module, bool value)
        {
            unitModules[(int)module] = value;
        }
        public bool GetModule(UnitModule module)
        {
            return unitModules[(int)module];
        }

        public void SetUpgrade(UnitUpgrade upgrade, int value)
        {
            unitUpgrades[(int)upgrade] = value;
        }
        public int GetUpgrade(UnitUpgrade upgrade)
        {
            return unitUpgrades[(int)upgrade];
        }

        /// <summary>
        /// Updates stats, recalculating it by upgrades & modules
        /// </summary>
        public void UpdateValues()
        {
            foreach (var m in Enum.GetValues(typeof(UnitUpgrade)).Cast<UnitUpgrade>().Where((_) => _ != UnitUpgrade.None))
            {
                int n = GetUpgrade(m);
                stats = stats + n * UnitUpgradeInfo.Get(m);
            }
        }

        public override GameObject Clone()
        {
            return new Unit(Position, OwnerPlayer, Id) { stats = stats.Clone(), unitModules = (bool[])unitModules.Clone(), unitUpgrades = (int[])unitUpgrades.Clone() };
        }
    }

    public class UnitStats
    {
        public UnitStats()
        {
            dat = new double[Enum.GetValues(typeof(UnitStatType)).Length];
            this[UnitStatType.HP] = 100;
            this[UnitStatType.EnergyK] = 1.0;
            this[UnitStatType.EnergyUp] = 1.0;
        }

        private double[] dat;

        public double this[UnitStatType index]
        {
            get
            {
                return dat[(int)index];
            }
            set
            {
                dat[(int)index] = value;
            }
        }

        public double OverallEnergy
        {
            get
            {
                return this[UnitStatType.EnergyUp] * this[UnitStatType.EnergyK];
            }
        }

        public UnitStats Clone()
        {
            return new UnitStats() { dat = (double[])dat.Clone() };
        }

        public static UnitStats operator +(UnitStats a, UnitStats b)
        {
            var x = new UnitStats();
            for (int i = 0; i < b.dat.Length; i++)
            {
                if ((UnitStatType)i != UnitStatType.EnergyK)
                    x.dat[i] = a.dat[i] + b.dat[i];
                else
                    x.dat[i] = a.dat[i] * b.dat[i];
            }
            return x;
        }

        public static UnitStats operator *(int a, UnitStats b)
        {
            var x = new UnitStats();
            for (int i = 0; i < b.dat.Length; i++)
            {
                if ((UnitStatType)i != UnitStatType.EnergyK)
                    x.dat[i] = b.dat[i] * a;
                else
                    x.dat[i] = Math.Pow(b.dat[i], a);
            }
            return x;
        }
    }
}

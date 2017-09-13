using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects
{
    /// <summary>
    /// Represents Unit <see cref="GameObject"/>
    /// </summary>
    public class Unit : GameObject
    {
        private Unit(Vector2 pos, Player ownerPlayer, int id) : base(ownerPlayer, id, new Vector2(1,1))
        {
            this.pos = pos;
            unitModules = new bool[Enum.GetValues(typeof(UnitModule)).Length];
            unitUpgrades = new int[Enum.GetValues(typeof(UnitUpgrade)).Length];
        }

        bool[] unitModules;
        int[] unitUpgrades;
        readonly int[] maxUpgradeLevels = new int[] {0,5,3,3,3,5,3,3,3};//customizable constant
        private UnitStats stats;
        private Vector2 pos;
        public List<Building> Paralizers = new List<Building>();//на случай если юнита сдерживает несколько зданий
        public override double HP
        {
            get
            {
                return stats[UnitStatType.HP];
            }
            protected set { }
        }
        public override double Defence
        {
            get
            {
                return stats[UnitStatType.Defence];
            }
            protected set { }
        }
        public override double EnergyConsumation
        {
            get
            {
                return stats.OverallEnergy;
            }
            protected set { }
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
                return pos;
            }
        }
        public double ActionPoints { get; set; }
        private UnitOperation performingOperation;

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
        public void SetUpgrade(int upgrade, int value)
        {
            unitUpgrades[upgrade] = value;
        }
        public int GetUpgrade(UnitUpgrade upgrade)
        {
            return unitUpgrades[(int)upgrade];
        }
        public int GetUpgrade(int upgrade)
        {
            return unitUpgrades[upgrade];
        }
        public bool CanUpgrade(UnitUpgrade upgrate)
        {
            if (unitUpgrades[(int)upgrate] + 1 <= maxUpgradeLevels[(int)upgrate])
            {
                switch (upgrate)
                {
                    //case UnitUpgrade.Efficiency://можно всегда
                    //    break;
                    case UnitUpgrade.MiningBoost:
                        if (!unitModules[(int)UnitModule.Mining])
                        {
                            return false;
                        }
                        break;    
                    case UnitUpgrade.MeleeBoost:
                        if (!unitModules[(int)UnitModule.MeleeAttack])
                        {
                            return false;
                        }
                        break;
                    case UnitUpgrade.DistantBoost:
                        if (!unitModules[(int)UnitModule.RangeAttack])
                        {
                            return false;
                        }
                        break;
                    //case UnitUpgrade.MovingBoost://можно всегда
                    //    break;
                    case UnitUpgrade.BuildingBoost:
                        if (!unitModules[(int)UnitModule.Building])
                        {
                            return false;
                        }
                        break;
                    //case UnitUpgrade.VisibilityRadiusIncrease://можно всегда
                    //    break;
                    //case UnitUpgrade.HeavyArmor://можно всегда
                    //    break;
                    case UnitUpgrade.RepairBoost:
                        if (!unitModules[(int)UnitModule.Repair])
                        {
                            return false;
                        }
                        break;
                }
                return true;
            }
            return false;
        }
        public bool CanUpgrade(int upgrate)
        {
            if (unitUpgrades[upgrate] + 1 <= maxUpgradeLevels[upgrate])
            {
                //TODO it later
            }
            return false;
        }
        public bool CanPlaceModule(UnitModule module)
        {
            return !unitModules[(int)module];
        }
        private UnitOperation CreateUnitOperation(UnitOperationType type)
        {
            switch (type)
            {
                case UnitOperationType.Move:
                    return new Move(this);
                case UnitOperationType.Mine:
                    return new Mine(this);
                case UnitOperationType.RangeHit:
                    return new RangeHit(this);
                case UnitOperationType.MeleeHit:
                    return new MeleeHit(this);
                case UnitOperationType.BuildStart:
                    return new BuildStart(this);
                case UnitOperationType.Repair:
                    return new Repair(this);
            }
            return null;
        }
        public void Update(UnitOperationType operation)
        {
            if (operation != UnitOperationType.None)
            {
                performingOperation = CreateUnitOperation(operation);
                performingOperation.StartOperation();
            }
            if (performingOperation != null)
            {
                performingOperation.DoneAdd();
                if (performingOperation.Done == performingOperation.NeedDone)
                {
                    performingOperation.FinalizeOperation();
                    performingOperation = null;
                }
            }
        }
        /// <summary>
        /// Updates stats, recalculating it by upgrades & modules
        /// </summary>
        public void UpdateValues()
        {
            foreach (var m in Enum.GetValues(typeof(UnitUpgrade)).Cast<UnitUpgrade>().Where((_) => _ != UnitUpgrade.None))//перечисление всех юнит-upgrade-ов
            {
                int n = GetUpgrade(m);
                stats = stats + n * UnitUpgradeInfo.Get(m);
            }
        }
        public override GameObject Clone()
        {
            return new Unit(Position, OwnerPlayer, Id) { stats = stats.Clone(), unitModules = (bool[])unitModules.Clone(), unitUpgrades = (int[])unitUpgrades.Clone() };
        }
        public static Unit NewUnit(Vector2 pos, Player ownerPlayer, int id)
        {
            return new Unit(pos, ownerPlayer, id);
        }
        public override void Serialize(Stream str)
        {
            base.Serialize(str);
            BinaryWriter bw = new BinaryWriter(str);
            bw.WriteIVector(Position);
            bw.WriteBools(unitModules.Skip(1).ToArray()); /* Skip "None" module */
            stats.Serialize(str);
        }
    }

    /// <summary>
    /// Defines various <see cref="Unit"/> stats
    /// </summary>
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

        public void Serialize(Stream str)
        {
            BinaryWriter bw = new BinaryWriter(str);
            bw.WriteDoubles(dat.Skip(1).ToArray()); /* Skip "None" Upgrade */
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

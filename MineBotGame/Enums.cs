using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame
{
    public enum BuildingType
    {
        None = 0,
        Base = 1,
        Storage = 2,
        ModuleFactory = 3,
        Workshop = 4,
    }

    public enum ResourceType
    {
        None = 0,
        Iron = 1,
        Silicon = 2,
        Uranium = 3,
        Quartz = 4,
    }

    public enum UnitModules
    {
        None = 0,
        Mining = 1,
        Melee = 2,
        DistantBattle = 3,
        Building = 4,
        Repair = 5,
    }

    public enum UnitUpgrade
    {
        None = 0,
        Efficiency = 1,
        MiningBoost = 2,
        MeleeBoost = 3,
        DistantBoost = 4,
        MovingBoost = 5,
        BuildingBoost = 6,
        VisibilityRadiusIncrease = 7,
        HeavyArmor = 8,
        EnergyArmor = 9,
        Carry = 10,
    }

    public enum BuildingOperationType
    {
        None = 0,
        NewUnit = 1,
        NewModule = 2,
        NewUpgrade = 3,
    }

}

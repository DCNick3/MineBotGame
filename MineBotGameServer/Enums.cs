﻿using System;
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
        Generator = 4,
        Gun = 5,
        Laboratory = 6,
    }

    public enum ResourceType
    {
        None = 0,
        Iron = 1,
        Silicon = 2,
        Uranium = 3,
        Quartz = 4,
    }

    public enum UnitModule
    {
        None = 0,
        Mining = 1,
        MeleeAttack = 2,
        RangeAttack = 3,
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

    public enum UnitStatType
    {
        None = 0,
        Defence = 1,
        HP = 2,
        EnergyUp = 3,
        EnergyK = 4,
        ScoutRange = 5,
        Speed = 6,
        MineSpeed = 7,
        BuildSpeed = 8,
        MeleeSpeed = 9,
        RangeSpeed = 10,
        MaxCarry = 11,
        Weight = 12,
    }

    public enum PlayerActionType
    {
        Idle = 0,
        Move = 1,
        RangeHit = 2,
        MeleeHit = 3,
        BuildStart = 4,  /* Приступаем к строительству... */
        BuildEnd = 5,    /* Бросаем строительство, теперь можно двигаться */
        StartResearch = 6,  /* Здание начинает исследование (помещает в очередь) */
        StartUnitSpawn = 7, /* Здание начинает создание юнита (помещает в оцередь) */
        StartUnitUpgrade = 8, /* Здание начинает апгрейд юнита (помещает в очередь). Юнит должен быть рядом, обездвиживается */
        CancelBuildingAction = 9, /* Здание отменает действие */
        SelfDestruct = 10,
    }
}
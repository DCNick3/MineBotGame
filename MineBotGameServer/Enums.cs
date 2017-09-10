namespace MineBotGame
{
    // This is defenitions of most enums, used in a game.

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
        Repair = 5
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
    }

    public enum BuildingOperationType
    {
        None = 0,
        NewUnit = 1,
        NewModule = 2,
        NewUpgrade = 4,
        DoLocalResearch = 8,
        DoGlobalResearch = 16
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

    public enum GlobalResearch
    {
        None = 0,

        UraniumGenerators = 1,
        BetterModules = 2,
        BestModules = 4,

        All = UraniumGenerators | BetterModules | BestModules,
    }

    public enum LocalResearch
    {
        None = 0,

        MoreEnergy = 1,
        MoreStorage = 2,
        TestResearch = 4,

        All = MoreEnergy | MoreStorage | TestResearch,
    }

    public enum ActionError
    {
        Succeed = 0,
        NoResources = 1,
        NotInRange = 2,
        ImpossibleAction = 3, //In fact this mustn't be happened if client has not bugs and player is not cheating
        NoQueueSpace = 4,
        GameObjectDoesNotExists = 5,
        NoEnergy = 6,
        InvalidQueueIndex = 7,
        InvalidParam = 8
    }
}

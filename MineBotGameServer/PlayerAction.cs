using System;
using System.IO;
using System.Numerics;

namespace MineBotGame.PlayerActions
{
    /// <summary>
    /// Class, that represents Player Action (minimal <see cref="PlayerController"/> command)
    /// </summary>
    public class PlayerAction
    {
        public PlayerAction(PlayerActionType type, int actionId)
        {
            ActionType = type;
            ActionId = actionId;
        }

        public PlayerActionType ActionType { get; private set; }
        public int ActionId { get; private set; }

        /// <summary>
        /// Deserializes <see cref="PlayerAction"/> class from stream 
        /// </summary>
        /// <param name="str">Stream with read possibility, to read <see cref="PlayerAction"/> data from</param>
        /// <returns>Deserialized <see cref="PlayerAction"/> instance</returns>
        public static PlayerAction Deserialize(Stream str)
        {
            BinaryReader br = new BinaryReader(str);
            int id = br.ReadInt32();
            PlayerActionType actionType = (PlayerActionType)br.ReadInt32();
            switch (actionType)
            {
                case PlayerActionType.Idle:
                    return new PlayerAction(actionType, id);
                case PlayerActionType.Move:
                case PlayerActionType.MeleeHit:
                case PlayerActionType.RangeHit:
                    return new PlayerActionVectorized(actionType, id, br.ReadInt32(), br.ReadIVector());
                case PlayerActionType.BuildStart:
                    return new PlayerActionBuild(actionType, id, br.ReadInt32(), br.ReadIVector(), br.ReadInt32());
                case PlayerActionType.BuildEnd:
                case PlayerActionType.SelfDestruct:
                    return new PlayerActionObject(actionType, id, br.ReadInt32());
                case PlayerActionType.StartResearch:
                case PlayerActionType.StartUnitSpawn:
                    return new PlayerActionOperation(actionType, id, br.ReadInt32(), br.ReadInt32());
                case PlayerActionType.StartUnitUpgrade:
                    return new PlayerActionUpgrade(actionType, id, br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                case PlayerActionType.CancelBuildingAction:
                    return new PlayerActionCancel(actionType, id, br.ReadInt32(), br.ReadInt32());
            }
            throw new InvalidDataException("Unknown PlayerActionType");
        }

        /// <summary>
        /// Deserializes <see cref="PlayerAction"/> class from int arrays
        /// </summary>
        /// <param name="data">Int attay to read data from</param>
        /// <returns>Deserialized <see cref="PlayerAction"/> instance</returns>
        public static PlayerAction Deserialize(int[] data)
        {
            int i = 0;
            try
            {
                int id = data[++i];
                PlayerActionType actionType = (PlayerActionType)data[++i];
                switch (actionType)
                {
                    case PlayerActionType.Idle:
                        return new PlayerAction(actionType, id);
                    case PlayerActionType.Move:
                    case PlayerActionType.MeleeHit:
                    case PlayerActionType.RangeHit:
                        return new PlayerActionVectorized(actionType, id, data[++i], new Vector2(data[++i], data[++i]));
                    case PlayerActionType.BuildStart:
                        return new PlayerActionBuild(actionType, id, data[++i], new Vector2(data[++i], data[++i]), data[++i]);
                    case PlayerActionType.BuildEnd:
                    case PlayerActionType.SelfDestruct:
                        return new PlayerActionObject(actionType, id, data[++i]);
                    case PlayerActionType.StartResearch:
                    case PlayerActionType.StartUnitSpawn:
                        return new PlayerActionOperation(actionType, id, data[++i], data[++i]);
                    case PlayerActionType.StartUnitUpgrade:
                        return new PlayerActionUpgrade(actionType, id, data[++i], data[++i], data[++i]);
                    case PlayerActionType.CancelBuildingAction:
                        return new PlayerActionCancel(actionType, id, data[++i], data[++i]);
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidDataException("Not enough data");
            }
            throw new InvalidDataException("Unknown PlayerActionType");
        }

        public override string ToString()
        {
            return ActionType.ToString();
        }
    }
    /*
     * From MineBotProtocol
     * 
     *  CommandName          | Parameters        | Type                   | Description
     *  ---------------------|-------------------|------------------------|--------------
     *  Idle                 | Nothin'           | PlayerAction           | Ну собсно.... Зачем? хз.
     *  Move                 | Id, IVector       | PlayerActionVectorized | Толи волю тратить, то ли замораживать..
     *  RangeHit             | Id, IVector       | PlayerActionVectorized | Толи волю тратить, то ли замораживать..
     *  MeleeHit             | Id, IVector       | PlayerActionVectorized | Толи волю тратить, то ли замораживать..
     *  BuildStart           | Id, IVector, Type | PlayerActionBuild      | Приступаем к строительству... Юнит обездвиживается
     *  BuildEnd             | Id                | PlayerActionObject     | Бросаем строительство, теперь можно двигаться 
     *  StartResearch        | Id, Type          | PlayerActionOperation  | Здание начинает исследование (помещает в очередь), ресурсы берём сразу
     *  StartUnitSpawn       | Id, Type          | PlayerActionOperation  | Здание начинает создание юнита (помещает в оцередь), 
     *  StartUnitUpgrade     | Id, Id, Type      | PlayerActionUpgrade    | Здание начинает апгрейд юнита (помещает в очередь), ресурсы берём сразу. Юнит должен быть рядом, обездвиживается (сразу, при помещении в очередь). 
     *  CancelBuildingAction | Id, int           | PlayerActionCancel     | Здание отменает действие, ресурсы высвобождаются, и прочая фигня
     *  SelfDestruct         | Id                | PlayerActionObject     | Бум
     */
     
    public class PlayerActionObject : PlayerAction
    {
        public int Id { get; private set; }
        public PlayerActionObject(PlayerActionType actionType, int actionId, int id) : base(actionType, actionId)
        {
            Id = id;
        }
    }
    public class PlayerActionVectorized : PlayerActionObject
    {
        public Vector2 Vector { get; private set; }
        public PlayerActionVectorized(PlayerActionType actionType, int actionId, int id, Vector2 vector) : base(actionType, actionId, id)
        {
            Vector = vector;
        }
    }

    public class PlayerActionBuild : PlayerActionVectorized
    {
        public int Type { get; private set; }
        public PlayerActionBuild(PlayerActionType actionType, int actionId, int id, Vector2 vector, int type) : base(actionType, actionId, id, vector)
        {
            Type = type;
        }
    }

    public class PlayerActionOperation : PlayerActionObject
    {
        public int Type { get; private set; }
        public PlayerActionOperation(PlayerActionType actionType, int actionId, int id, int type) : base(actionType, actionId, id)
        {
            Type = type;
        }
    }

    public class PlayerActionUpgrade : PlayerActionObject
    {
        public int UnitId { get; private set; }
        public int Type { get; private set; }
        public PlayerActionUpgrade(PlayerActionType actionType, int actionId, int id, int unitId, int type) : base(actionType, actionId, id)
        {
            UnitId = unitId;
            Type = type;
        }
    }

    public class PlayerActionCancel : PlayerActionObject
    {
        public int QueueIndex { get; private set; }
        public PlayerActionCancel(PlayerActionType actionType, int actionId, int id, int queueIndex) : base(actionType, actionId, id)
        {
            QueueIndex = queueIndex;
        }
    }
}

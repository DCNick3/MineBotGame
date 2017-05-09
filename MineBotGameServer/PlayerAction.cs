using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame.PlayerActions
{
    /// <summary>
    /// Class, that represents Player Action
    /// </summary>
    public class PlayerAction
    {
        public PlayerAction(PlayerActionType type)
        {
            ActionType = type;
        }

        public PlayerActionType ActionType { get; private set; }

        /// <summary>
        /// Deserializes <see cref="PlayerAction"/> class from stream 
        /// </summary>
        /// <param name="str">Stream with read possibility, to read <see cref="PlayerAction"/> data from</param>
        /// <returns>Deserialized <see cref="PlayerAction"/> instance</returns>
        public static PlayerAction Deserialize(Stream str)
        {
            BinaryReader br = new BinaryReader(str);
            PlayerActionType actionType = (PlayerActionType)br.ReadInt32();
            switch (actionType)
            {
                case PlayerActionType.Idle:
                    return new PlayerAction(actionType);
                case PlayerActionType.Move:
                case PlayerActionType.MeleeHit:
                case PlayerActionType.RangeHit:
                    return new PlayerActionVectorized(actionType, br.ReadInt32(), br.ReadIVector());
                case PlayerActionType.BuildStart:
                    return new PlayerActionBuild(actionType, br.ReadInt32(), br.ReadIVector(), br.ReadInt32());
                case PlayerActionType.BuildEnd:
                case PlayerActionType.SelfDestruct:
                    return new PlayerActionObject(actionType, br.ReadInt32());
                case PlayerActionType.StartResearch:
                case PlayerActionType.StartUnitSpawn:
                    return new PlayerActionOperation(actionType, br.ReadInt32(), br.ReadInt32());
                case PlayerActionType.StartUnitUpgrade:
                    return new PlayerActionUpgrade(actionType, br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                case PlayerActionType.CancelBuildingAction:
                    return new PlayerActionCancel(actionType, br.ReadInt32(), br.ReadInt32());
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
                PlayerActionType actionType = (PlayerActionType)data[++i];
                switch (actionType)
                {
                    case PlayerActionType.Idle:
                        return new PlayerAction(actionType);
                    case PlayerActionType.Move:
                    case PlayerActionType.MeleeHit:
                    case PlayerActionType.RangeHit:
                        return new PlayerActionVectorized(actionType, data[++i], new Vector2(data[++i], data[++i]));
                    case PlayerActionType.BuildStart:
                        return new PlayerActionBuild(actionType, data[++i], new Vector2(data[++i], data[++i]), data[++i]);
                    case PlayerActionType.BuildEnd:
                    case PlayerActionType.SelfDestruct:
                        return new PlayerActionObject(actionType, data[++i]);
                    case PlayerActionType.StartResearch:
                    case PlayerActionType.StartUnitSpawn:
                        return new PlayerActionOperation(actionType, data[++i], data[++i]);
                    case PlayerActionType.StartUnitUpgrade:
                        return new PlayerActionUpgrade(actionType, data[++i], data[++i], data[++i]);
                    case PlayerActionType.CancelBuildingAction:
                        return new PlayerActionCancel(actionType, data[++i], data[++i]);
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
        public PlayerActionObject(PlayerActionType actionType, int id) : base(actionType)
        {
            Id = id;
        }
    }
    public class PlayerActionVectorized : PlayerActionObject
    {
        public Vector2 Vector { get; private set; }
        public PlayerActionVectorized(PlayerActionType actionType, int id, Vector2 vector) : base(actionType, id)
        {
            Vector = vector;
        }
    }

    public class PlayerActionBuild : PlayerActionVectorized
    {
        public int Type { get; private set; }
        public PlayerActionBuild(PlayerActionType actionType, int id, Vector2 vector, int type) : base(actionType, id, vector)
        {
            Type = type;
        }
    }

    public class PlayerActionOperation : PlayerActionObject
    {
        public int Type { get; private set; }
        public PlayerActionOperation(PlayerActionType actionType, int id, int type) : base(actionType, id)
        {
            Type = type;
        }
    }

    public class PlayerActionUpgrade : PlayerActionObject
    {
        public int UnitId { get; private set; }
        public int Type { get; private set; }
        public PlayerActionUpgrade(PlayerActionType actionType, int id, int unitId, int type) : base(actionType, id)
        {
            UnitId = unitId;
            Type = type;
        }
    }

    public class PlayerActionCancel : PlayerActionObject
    {
        public int QueueIndex { get; private set; }
        public PlayerActionCancel(PlayerActionType actionType, int id, int queueIndex) : base(actionType, id)
        {
            QueueIndex = queueIndex;
        }
    }
}

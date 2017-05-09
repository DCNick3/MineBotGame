using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame.GameObjects
{
    public class Building : GameObject
    {
        static Building()
        {
            AddBuilding(buildingNone);
            AddBuilding(buildingBase);
            AddBuilding(buildingGenerator);
            AddBuilding(buildingGun);
            AddBuilding(buildingLaboratory);
            AddBuilding(buildingModuleFactory);
            AddBuilding(buildingStorage);
        }

        private Building(Vector2 size) : base(null, -1, size)
        { } 

        private Building(Player ownerPlayer, int id, Vector2 pos,  Vector2 size) : base(ownerPlayer, id, size)
        {
            _pos = pos;
        }

        public List<BuildingOperation> OperationQueue { get; private set; }

        private double _hp, _def, _energy;
        private readonly Vector2 _pos;


        public override double HP
        {
            get
            {
                return _hp;
            }
        }
        public override double Defence
        {
            get
            {
                return _def;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return _pos;
            }
        }
        public override double EnergyConsumation
        {
            get
            {
                return _energy;
            }
        }

        public BuildingType Type { get; private set; }

        public void Enqueue(BuildingOperation operation)
        {
            OperationQueue.Add(operation);
        }


        public BuildingOperation Dequeue()
        {
            return DequeueAt(0);
        }

        public BuildingOperation DequeueAt(int i)
        {
            if (OperationQueue.Count <= i)
                throw new InvalidOperationException();
            var r = OperationQueue[i];
            OperationQueue.RemoveAt(i);
            return r;
        }

        public Building Clone(Player ownerPlayer, int id, Vector2 pos)
        {
            return new Building(ownerPlayer, id, pos, Size)
            {
                _hp = _hp,
                _def = _def,
                _energy = _energy,
            };
        }


        public static readonly Building buildingNone = new Building(new Vector2(0, 0))
        { _hp = 0, _def = 0, _energy = 0, Type = BuildingType.None};

        public static readonly Building buildingBase = new Building(new Vector2(4, 4))
            { _hp = 1000, _def = 10, _energy = 10, Type = BuildingType.Base };

        public static readonly Building buildingStorage = new Building(new Vector2(4, 4))
            { _hp = 1000, _def = 10, _energy = 10, Type = BuildingType.Storage };

        public static readonly Building buildingModuleFactory = new Building(new Vector2(4, 4))
            { _hp = 1000, _def = 10, _energy = 10, Type = BuildingType.ModuleFactory };

        public static readonly Building buildingGenerator = new Building(new Vector2(4, 4))
            { _hp = 1000, _def = 10, _energy = 10, Type = BuildingType.Generator};

        public static readonly Building buildingGun = new Building(new Vector2(2, 2))
            { _hp = 1000, _def = 10, _energy = 10, Type = BuildingType.Gun };

        public static readonly Building buildingLaboratory = new Building(new Vector2(2, 2))
            { _hp = 1000, _def = 10, _energy = 10, Type = BuildingType.Laboratory };

        private static Dictionary<BuildingType, Building> blds = new Dictionary<BuildingType, Building>();
        private static void AddBuilding(Building b)
        {
            blds.Add(b.Type, b);
        }

        public static Building NewBuilding(BuildingType t, Player owner, int id, Vector2 position)
        {
            return blds[t].Clone(owner, id, position);
        }
    }
}

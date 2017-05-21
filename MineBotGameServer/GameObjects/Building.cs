using System;
using System.Collections.Generic;
using System.IO;
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
        {
            _scout = 10.0;
            OperationQueue = new List<BuildingOperation>();
        }

        private Building(Player ownerPlayer, int id, Vector2 pos, Vector2 size) : base(ownerPlayer, id, size)
        {
            _pos = pos;
            _scout = 10.0;
            OperationQueue = new List<BuildingOperation>();
        }

        public List<BuildingOperation> OperationQueue { get; private set; }
        private const int QUEUE_SIZE = 5;

        private double _hp, _def, _energy, _scout;
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
        public override double EnergyConsumation
        {
            get
            {
                return _energy;
            }
        }
        public override double ScoutRange
        {
            get
            {
                return _scout;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return _pos;
            }
        }

        public BuildingType Type { get; private set; }
        public LocalResearch Researches { get; private set; }
        private LocalResearch availableLResearches = LocalResearch.None;
        private GlobalResearch availableGResearches = GlobalResearch.None;


        public bool Enqueue(BuildingOperation operation)
        {
            if (OperationQueue.Count == QUEUE_SIZE)
                return false;


            OperationQueue.Add(operation);
            return true;
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
            var r = new Building(ownerPlayer, id, pos, Size)
            {
                _hp = _hp,
                _def = _def,
                _energy = _energy,
                _scout = _scout,
                Type = Type,
                availableGResearches = availableGResearches,
                availableLResearches = availableLResearches,
                OperationQueue = new List<BuildingOperation>(),
                Researches = Researches,
                onAdd = onAdd,
                onRemove = onRemove,
            };
            r.OperationQueue.AddRange(OperationQueue);
            return r;
        }

        public ActionError DoGlobalResearch(GlobalResearch type)
        {
            if ((type & availableGResearches) == 0)
                return ActionError.ImpossibleAction;

            BuildingOperation op = BuildingOperation.NewGlobalResearch(type);
            return DoOperation(op);
        }

        public ActionError DoLocalResearch(LocalResearch type)
        {
            if ((type & availableLResearches) == 0)
                return ActionError.ImpossibleAction;

            BuildingOperation op = BuildingOperation.NewLocalResearch(type);
            return DoOperation(op);
        }

        public ActionError DoOperation(BuildingOperation op)
        {
            if (OperationQueue.Count == QUEUE_SIZE)
                return ActionError.NoQueueSpace;
            if (!OwnerPlayer.CheckResources(op.ResourceConsumation))
                return ActionError.NoResources;
            if (!OwnerPlayer.CheckEnergy(op.EnergyConsumation))
                return ActionError.NoEnergy;

            OwnerPlayer.UtilizeEnergy(op.EnergyConsumation);
            OwnerPlayer.UtilizeResources(op.ResourceConsumation);

            Enqueue(op);
            return ActionError.Succeed;
        }

        public ActionError CancelOperation(int queueIndex)
        {
            if (queueIndex >= OperationQueue.Count)
                return ActionError.InvalidQueueIndex;
            OperationQueue.RemoveAt(queueIndex);
            return ActionError.Succeed;
        }

        public void OnAdd(Player p)
        {
            p.EnergyConsumation += EnergyConsumation;
            onAdd(p);
        }

        public override void Update()
        {
            if (OperationQueue.Count != 0)
            {
                var op = OperationQueue.First();
                op.Done++;
                if (op.Done == op.NeedDone)
                {
                    Dequeue();
                    FinalizeOperation(op);
                }
            }
        }

        public void OnRemove(Player p)
        {
            p.EnergyConsumation -= EnergyConsumation;
            onRemove(p);
        }

        private Action<Player> onAdd = (p) => { };
        private Action<Player> onRemove = (p) => { };

        public static readonly Building buildingNone = new Building(new Vector2(0, 0))
        { _hp = 0, _def = 0, _energy = 0, Type = BuildingType.None };

        public static readonly Building buildingBase = new Building(new Vector2(4, 4))
        {
            _hp = 1000,
            _def = 10,
            _energy = 10,
            Type = BuildingType.Base,
            availableLResearches = LocalResearch.TestResearch,
            onAdd = (p) => 
            {
                p.EnergyGeneration += 40.0;
                p.ResourceLimits.AddAll(200);
            },
            onRemove = (p) => 
            {
                p.EnergyGeneration -= 40.0;
                p.ResourceLimits.AddAll(-200);
            },
        };

        public static readonly Building buildingStorage = new Building(new Vector2(4, 4))
        {
            _hp = 1000,
            _def = 10,
            _energy = 10,
            Type = BuildingType.Storage,
            availableLResearches = LocalResearch.TestResearch,
        };

        public static readonly Building buildingModuleFactory = new Building(new Vector2(4, 4))
            { _hp = 1000, _def = 10, _energy = 10, Type = BuildingType.ModuleFactory,
            availableLResearches = LocalResearch.TestResearch,
        };

        public static readonly Building buildingGenerator = new Building(new Vector2(4, 4))
            { _hp = 1000, _def = 10, _energy = 10, Type = BuildingType.Generator,
            availableLResearches = LocalResearch.TestResearch,
        };

        public static readonly Building buildingGun = new Building(new Vector2(2, 2))
            { _hp = 1000, _def = 10, _energy = 10, Type = BuildingType.Gun,
            availableLResearches = LocalResearch.TestResearch,
        };

        public static readonly Building buildingLaboratory = new Building(new Vector2(2, 2))
            { _hp = 1000, _def = 10, _energy = 10, Type = BuildingType.Laboratory,
            availableLResearches = LocalResearch.TestResearch,
        };

        private static Dictionary<BuildingType, Building> blds = new Dictionary<BuildingType, Building>();
        private static void AddBuilding(Building b)
        {
            blds.Add(b.Type, b);
        }

        public static Building NewBuilding(BuildingType t, Player owner, int id, Vector2 position)
        {
            return blds[t].Clone(owner, id, position);
        }

        public override GameObject Clone()
        {
            return Clone(OwnerPlayer, Id, Position);
        }

        private void FinalizeOperation(BuildingOperation op)
        {
            switch (op.Type)
            {
                case BuildingOperationType.DoLocalResearch:
                    {
                        OwnerPlayer.UtilizeEnergy(-op.EnergyConsumation);
                        Researches |= (LocalResearch)op.ParA;
                        // TODO: Trigger some event
                    }
                    break;
            }
        }

        public override void Serialize(Stream str)
        {
            base.Serialize(str);
            BinaryWriter bw = new BinaryWriter(str);
            bw.Write((int)Type);
            bw.WriteIVector(Position);
            bw.WriteIVector(Size);
            bw.Write((int)Researches);
            bw.Write(_hp);
            bw.Write(_def);
            bw.Write(_energy);

            bw.Write(OperationQueue.Count);
            for (int i = 0; i < OperationQueue.Count; i++)
                OperationQueue[i].Serialize(str);
        }
    }
}

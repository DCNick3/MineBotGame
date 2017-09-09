using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects
{
    /// <summary>
    /// Represents Building <see cref="GameObject"/>
    /// </summary>
    public class Building : GameObject
    {
        public Building(Player ownerPlayer, int id, Vector2 pos, Vector2 size, double hp, double def, double energyCostumation,BuildingOperationType availableOperations, GlobalResearch availableGResearches, LocalResearch availableLResearches) : base(ownerPlayer, id, size)
        {
            HP = hp;
            Defence = def;
            EnergyConsumation = energyCostumation;
            this.pos = pos;
            OperationQueue = new List<BuildingOperation>();
            this.availableLResearches = availableLResearches;
            this.availableGResearches = availableGResearches;
            this.availableOperations = availableOperations;
        }
      
        private const int QUEUE_SIZE = 5;
        private double hp;
        private double def;
        private double energy;
        protected readonly double scout = 10.0;
        private readonly Vector2 pos;
        public BuildingType Type { get; protected set; }        
        public LocalResearch Researches { get; protected set; }
        public override double HP
        {
            get
            {
                return hp;
            }
            protected set
            {
                hp = value;
            }
        }
        public override double Defence
        {
            get
            {
                return def;
            }
            protected set
            {
                def = value;
            }
        }
        public override double EnergyConsumation
        {
            get
            {
                return energy;
            }
            protected set
            {
                energy = value;
            }
        }
        public override double ScoutRange
        {
            get
            {
                return scout;
            }
        }
        public override Vector2 Position
        {
            get
            {
                return pos;
            }
        }               
        public List<BuildingOperation> OperationQueue { get; private set; }        
        protected Action<Player> onAdd = (p) => { };//ресурсы и энергия отнимаются в начале строительства
        protected Action<Player> onRemove = (p) => { };
        protected readonly LocalResearch availableLResearches = LocalResearch.None;
        protected readonly GlobalResearch availableGResearches = GlobalResearch.None;
        protected readonly BuildingOperationType availableOperations = BuildingOperationType.None;

        public Vector2[] GetNearFreeSpace()
        {
            List<Vector2> vectors= new List<Vector2>();
            for (int x = (int)Position.X; x < (int)Position.X + (int)Size.X; x++)
            {
                if (!OwnerPlayer.gameArea[x, (int)Position.Y - 1].IsObstacle)
                {
                    vectors.Add(new Vector2(x, (int)Position.Y - 1));
                }
                if (!OwnerPlayer.gameArea[x, (int)Position.Y + (int)Size.Y].IsObstacle)
                {
                    vectors.Add(new Vector2(x, (int)Position.Y + (int)Size.Y));
                }
            }
            for (int y = (int)Position.Y; y < (int)Position.Y + (int)Size.Y; y++)
            {
                if (!OwnerPlayer.gameArea[y, (int)Position.X - 1].IsObstacle)
                {
                    vectors.Add(new Vector2(y, (int)Position.X - 1));
                }
                if (!OwnerPlayer.gameArea[y, (int)Position.X + (int)Size.X].IsObstacle)
                {
                    vectors.Add(new Vector2(y, (int)Position.X + (int)Size.X));
                }
            }
            return vectors.ToArray();
        }
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
            var r = new Building(ownerPlayer, id, pos, Size, hp, def, energy,availableOperations,availableGResearches,availableLResearches)
            {
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
            //ресурсы и энергия отнимаются в начале строительства
            onAdd(p);
        }
        public void OnRemove(Player p)
        {
            p.EnergyConsumation -= EnergyConsumation;
            onRemove(p);
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
        public override GameObject Clone()
        {
            return Clone(OwnerPlayer, Id, Position);
        }
        public override void Serialize(Stream str)
        {
            base.Serialize(str);
            BinaryWriter bw = new BinaryWriter(str);
            bw.Write((int)Type);
            bw.WriteIVector(Position);
            bw.WriteIVector(Size);
            bw.Write((int)Researches);
            bw.Write(hp);
            bw.Write(def);
            bw.Write(energy);

            bw.Write(OperationQueue.Count);
            for (int i = 0; i < OperationQueue.Count; i++)
                OperationQueue[i].Serialize(str);
        }
        protected void FinalizeOperation(BuildingOperation op)
        {
            op.FinalizeOperation();          
        }
    }
}

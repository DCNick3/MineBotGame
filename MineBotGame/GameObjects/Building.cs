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
        private Building(Vector2 pos)
        {
            _pos = pos;
        }

        public List<BuildingOperation> OperationQueue { get; private set; }

        private int _hp, _def, _id;
        private readonly Vector2 _pos;


        public override int HP
        {
            get
            {
                return _hp;
            }
        }

        public override int Defence
        {
            get
            {
                return _def;
            }
        }

        public override int Id
        {
            get
            {
                return _id;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return _pos;
            }
        }

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
    }
}

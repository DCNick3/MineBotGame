using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MineBotGame.GameObjects
{
    public abstract class GameObject
    {
        public abstract int HP { get; }
        public abstract int Defence { get; }
        public abstract int Id { get; }
        public abstract Vector2 Position { get; }
        public readonly int ownerPlayer;
        public readonly Vector2 size;
    }
}

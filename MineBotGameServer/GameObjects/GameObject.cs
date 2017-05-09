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
        protected GameObject(Player ownerPlayer, int id, Vector2 size)
        {
            this.ownerPlayer = ownerPlayer;
            this.size = size;
            this.id = id;
        }

        public abstract double HP { get; }
        public abstract double Defence { get; }
        public abstract double EnergyConsumation { get; }

        public abstract Vector2 Position { get; }
        public int Id { get { return id; } }
        public Player OwnerPlayer { get { return ownerPlayer; } }
        public Vector2 Size { get { return size; } }
        public abstract double ScoutRange { get; }
        private readonly Player ownerPlayer;
        private readonly Vector2 size;
        private readonly int id;

        public Vector2 Center { get { return Position + size / 2; } }

        public abstract GameObject Clone();

        public bool IsVisible(Vector2 position)
        {
            return (Center - position).Length() <= ScoutRange;
        }
    }

    public struct GameObjectPos
    {
        public GameObjectPos(Vector2 pos, Vector2 size)
        {
            position = pos;
            this.size = size;
        }
        public Vector2 position;
        public Vector2 size;

        public static bool operator ==(GameObjectPos a, GameObjectPos b)
        {
            return a.position == b.position && a.size == b.size;
        }

        public static bool operator !=(GameObjectPos a, GameObjectPos b)
        {
            return a.position != b.position || a.size != b.size;
        }

        public override bool Equals(object obj)
        {
            return obj is GameObjectPos && ((GameObjectPos)obj) == this;
        }

        public override int GetHashCode()
        {
            return position.GetHashCode() + size.GetHashCode();
        }
    }
}

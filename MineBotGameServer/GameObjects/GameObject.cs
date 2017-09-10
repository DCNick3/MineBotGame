using System;
using System.IO;
using System.Numerics;

namespace MineBotGame.GameObjects
{
    /// <summary>
    /// Represents abstract GameObject (neither a <see cref="Unit"/> or <see cref="Building"/>)
    /// </summary>
    public abstract class GameObject
    {
        protected GameObject(Player ownerPlayer, int id, Vector2 size)
        {
            this.ownerPlayer = ownerPlayer;
            this.size = size;
            this.id = id;
        }

        public abstract double HP { get;protected set; }
        public abstract double Defence { get; protected set; }
        public abstract double EnergyConsumation { get; protected set; }
        public abstract Vector2 Position { get; }
        public int Id { get { return id; } }
        public Player OwnerPlayer { get { return ownerPlayer; } }
        public Vector2 Size { get { return size; } }
        public abstract double ScoutRange { get; }
        private readonly Player ownerPlayer;
        private readonly Vector2 size;
        private readonly int id;
        
        public static int createId()
        {
            return 0;//TODO
        }

        public Vector2 Center { get { return Position + size / 2; } }

        public abstract GameObject Clone();

        public virtual void Serialize(Stream str)
        {
            BinaryWriter bw = new BinaryWriter(str);
            if (this is Building)
            {
                bw.Write(0x01);
            }
            else if (this is Unit)
            {
                bw.Write(0x02);
            }
            else
                throw new Exception();
            bw.Write(Id);
            bw.Write(OwnerPlayer.Id);
        }

        public virtual void Update() { }

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

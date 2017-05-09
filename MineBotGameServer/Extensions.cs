using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace MineBotGame
{
    public static class Extensions
    {
        public static Vector2 Integerize(this Vector2 v)
        {
            return new Vector2((int)Math.Floor(v.X), (int)Math.Floor(v.Y));
        }

        public static Vector2 ReadIVector(this BinaryReader sr)
        {
            return new Vector2(sr.ReadInt32(), sr.ReadInt32());
        }

        public static Vector2 ReadVector(this BinaryReader sr)
        {
            return new Vector2(sr.ReadSingle(), sr.ReadSingle());
        }
    }
}

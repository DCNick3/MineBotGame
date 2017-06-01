using System;
using System.IO;
using System.Numerics;

namespace MineBotGame
{
    /// <summary>
    /// Defines various extension methods, for better usability.
    /// </summary>
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

        /// <summary>
        /// Writes number of elements (as Int32), then all elements to <see cref="BinaryWriter"/> 
        /// </summary>
        /// <param name="bw">BinaryWriter to write to</param>
        /// <param name="ints">Array of elements to write</param>
        public static void WriteInts(this BinaryWriter bw, int[] ints)
        {
            bw.Write(ints.Length);
            for (int i = 0; i < ints.Length; i++)
                bw.Write(ints[i]);
        }

        public static void WriteIVector(this BinaryWriter bw, Vector2 v)
        {
            v = v.Integerize();
            bw.Write((int)v.X);
            bw.Write((int)v.Y);
        }


        /// <summary>
        /// Writes number of elements (as Int32), then all elements to <see cref="BinaryWriter"/> 
        /// </summary>
        /// <param name="bw">BinaryWriter to write to</param>
        /// <param name="ints">Array of elements to write</param>
        public static void WriteBools(this BinaryWriter bw, bool[] bls)
        {
            bw.Write(bls.Length);
            for (int i = 0; i < bls.Length; i++)
                bw.Write(bls[i]);
        }


        /// <summary>
        /// Writes number of elements (as Int32), then all elements to <see cref="BinaryWriter"/> 
        /// </summary>
        /// <param name="bw">BinaryWriter to write to</param>
        /// <param name="ints">Array of elements to write</param>
        public static void WriteDoubles(this BinaryWriter bw, double[] dbls)
        {
            bw.Write(dbls.Length);
            for (int i = 0; i < dbls.Length; i++)
                bw.Write(dbls[i]);
        }
    }
}

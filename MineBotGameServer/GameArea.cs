using MineBotGame.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MineBotGame
{
    /// <summary>
    /// Class, designed for managing game area (walls, ores) and object dislocation.
    /// </summary>
    public sealed class GameArea
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
       (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GameArea(int width, int height)
        {
            tiles = new GameTile[width, height];
            Width = width;
            Height = height;
        }
        #region Generator
        public IEnumerable<Vector2> EnumTiles()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    yield return new Vector2(i, j);
        }

        private static bool GetTileFromBoolMap(bool[,] map, Vector2 pos, Walls walls)
        {
            int x = (int)pos.X, y = (int)pos.Y;
            if (x >= 0 && y >= 0 && x < map.GetLength(0) && y < map.GetLength(1))
                return map[x, y];
            else if (x < 0 && (walls & Walls.West) != Walls.None)
                return true;
            else if (y < 0 && (walls & Walls.North) != Walls.None)
                return true;
            else if (x >= map.GetLength(0) && (walls & Walls.East) != Walls.None)
                return true;
            else if (y >= map.GetLength(1) && (walls & Walls.South) != Walls.None)
                return true;

            return false;
        }

        public enum Walls
        {
            None = 0,
            North = 1,
            East = 2,
            South = 4,
            West = 8,
            All = North  | East | South | West,
        }

        public class GeneratorParameters
        {
            public GeneratorParameters()
            {
                InitialChance = 550;
                Width = 64;
                Height = 64;
                MaskChance = 45;
                WallWidth = 3;
                Walls = Walls.None;
                PassCount = 5;
                Smoothing = false;
                WallForcing = true;
            }

            public int InitialChance { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int MaskChance { get; set; }
            public int WallWidth { get; set; }
            public Walls Walls { get; set; }
            public int PassCount { get; set; }
            public bool Smoothing { get; set; }
            public bool WallForcing { get; set; }

            public static GeneratorParameters Default
            {
                get
                {
                    return new GameArea.GeneratorParameters()
                    { Walls = GameArea.Walls.None, PassCount = 6, InitialChance = 550, MaskChance = 35, Smoothing = true };
                }
            }
            public static GeneratorParameters Old
            {
                get
                {
                    return new GameArea.GeneratorParameters()
                    { Walls = GameArea.Walls.None, WallForcing = false, PassCount = 5, InitialChance = 550, Smoothing = false };
                }
            }
        }

        /// <summary>
        /// Generates new room
        /// </summary>
        /// <param name="rnd">Random generator to use</param>
        /// <param name="walls">Bit mask for determining where generator must provide walls and where not.</param>
        /// <returns></returns>
        public static GameArea Generate(Random rnd, GeneratorParameters parameters)
        {
            int initialChance = parameters.InitialChance;
            Walls walls = parameters.Walls;

            int w = parameters.Width, h = parameters.Height;
            log.Info("Generaing new Area...");
            bool[,] sp = new bool[w, h], dp = new bool[w, h];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    sp[i, j] = rnd.Next(1000) < initialChance;

            bool[,] mask = new bool[w, h];
            int wallWidth = parameters.WallWidth;
            int maskPercentage = parameters.MaskChance;
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    if ((walls & Walls.North) != Walls.None && j < wallWidth)
                    {
                        if (j == 0 || rnd.Next(100) < maskPercentage)
                            mask[i, j] = true;
                    }
                    if ((walls & Walls.East) != Walls.None && i > w - 1 - wallWidth)
                    {
                        if (i == w-1 || rnd.Next(100) < maskPercentage)
                            mask[i, j] = true;
                    }
                    if ((walls & Walls.South) != Walls.None && j > h - 1 - wallWidth)
                    {
                        if (j == h-1 || rnd.Next(100) < maskPercentage)
                            mask[i, j] = true;
                    }
                    if ((walls & Walls.West) != Walls.None && i < wallWidth)
                    {
                        if (i == 0 || rnd.Next(100) < maskPercentage)
                            mask[i, j] = true;
                    }
                }

            //var inp = w / 2 - 2;
            for (int i = 0; i < parameters.PassCount; i++)
            {
                for (int j = 0; j < w; j++)
                    for (int k = 0; k < h; k++)
                    {
                        int nwc = GetNeighbourgs(new Vector2(j, k)).Select((_) => 
                            GetTileFromBoolMap(sp, _, parameters.WallForcing ? walls : Walls.All)).Where((_) => _).Count();
                        if (nwc >= 5 || (parameters.Smoothing && i == parameters.PassCount - 1 && (nwc >= 2)))
                            dp[j, k] = true;
                        else
                            dp[j, k] = false;
                    }
                if (i != parameters.PassCount - 1)
                {
                    for (int j = 0; j < w; j++)
                        for (int k = 0; k < h; k++)
                            dp[j, k] |= mask[j, k];
                }
                sp = dp;
                dp = new bool[w, h];
            }

            var r = new GameArea(w, h);

            int wallNumber = 0;

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    bool wall = sp[i, j];
                    if (wall)
                    {
                        wallNumber++;
                    }

                    r[i, j] = new GameTile(wall);
                }

            log.DebugFormat("wallRatio: {0}", (double)wallNumber / (w * h));

                    /*
            int c = 12+rnd.Next(4);
            for (int i = 0; i < c; i++)
            {
                Vector2 p = new Vector2(rnd.Next(r.Width), rnd.Next(r.Height));
                int rad = 2 + rnd.Next(10);
                foreach (var t in r.EnumTiles().Where((_) => (p - _).Length() <= rad))
                    r[t] = new GameTile(true);
            }*/

            log.Info("Done generating");
            return r;
        }
        #endregion
        GameTile[,] tiles;

        
        private struct GameObjectFrame
        {
            public GameObjectFrame(GameObject o, GameObjectPos p)
            {
                obj = o;
                lastPos = p;
            }
            public GameObjectFrame(GameObject o)
            {
                obj = o;
                lastPos = new GameObjectPos(o.Position, o.Size);
            }
            public GameObject obj;
            public GameObjectPos lastPos;
        }

        Dictionary<int, GameObjectFrame> objects = new Dictionary<int, GameObjectFrame>();

        public int Width { get; private set; }
        public int Height { get; private set; }
        public IEnumerable<GameObject> Objects { get { return objects.Select((_) => _.Value.obj); } }

        private GameTile stubTile = new GameTile(false);

        public GameTile this[Vector2 position]
        {
            get
            {
                return this[(int)position.X, (int)position.Y];
            }
            set
            {
                this[(int)position.X, (int)position.Y] = value;
            }
        }
        public GameTile this[int x, int y]
        {
            get
            {
                if (x >= 0 && y >= 0 && x < Width && y < Height)
                    return tiles[x, y];
                else
                    return stubTile;
            }
            set
            {
                tiles[x, y] = value;
            }
        }

        public static IEnumerable<Vector2> GetNeighbourgs(Vector2 pos)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (i != 0 || j != 0)
                        yield return pos + new Vector2(i, j);
        }

        public static IEnumerable<Vector2> GetNeighbourgsNoDiagonal(Vector2 pos)
        {
            yield return pos + new Vector2(1, 0);
            yield return pos + new Vector2(-1, 0);
            yield return pos + new Vector2(0, 1);
            yield return pos + new Vector2(0, -1);
        }

        public Vector2 GetRandomFree(Random rnd)
        {
            var f = EnumTiles().Where((_) => !this[_].IsObstacle);
            return f.ElementAt(rnd.Next(f.Count()));
        }

        public Vector2 GetRandomFreeWithWall(Random rnd)
        {
            var f = EnumTiles().Where((_) => !this[_].IsObstacle && GetNeighbourgsNoDiagonal(_).Where((__) => this[__].IsObstacle).Count() !=0);
            return f.ElementAt(rnd.Next(f.Count()));
        }

        private bool CanPlaceObject(GameObjectPos pos)
        {
            return CanPlaceObject(pos.position, pos.size);
        }

        private bool CanPlaceObject(Vector2 pos, Vector2 size)
        {
            var p = pos.Integerize();
            var s = size.Integerize();
            for (int i = 0; i < s.X; i++)
                for (int j = 0; j < s.Y; j++)
                    if (this[p + new Vector2(i, j)].Object != null)
                        return false;
            return true;
        }

        private void PlaceObject(GameObject obj)
        {
            PlaceObject(new GameObjectPos(obj.Position, obj.Size), obj);
        }

        private void PlaceObject(GameObjectPos pos, GameObject obj)
        {
            var p = pos.position.Integerize();
            var s = pos.size.Integerize();
            for (int i = 0; i < s.X; i++)
                for (int j = 0; j < s.Y; j++)
                    this[p + new Vector2(i, j)].Object = obj;
        }

        private void DisplaceObject(GameObjectPos pos)
        {
            DisplaceObject(pos.position, pos.size);
        }

        private void DisplaceObject(Vector2 pos, Vector2 size)
        {
            var p = pos.Integerize();
            var s = size.Integerize();
            for (int i = 0; i < s.X; i++)
                for (int j = 0; j < s.Y; j++)
                    this[p + new Vector2(i, j)].Object = null;
        }

        public void AddObject(GameObject obj)
        {
            PlaceObject(obj);
            objects[obj.Id] = new GameObjectFrame(obj);
        }

        /// <summary>
        /// Updates 'Object' property of all GameTile objects.
        /// </summary>
        public void RebaseObjects()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                var ob = objects.ElementAt(i);
                var o = ob.Value;
                var p = new GameObjectPos(o.obj.Position, o.obj.Size);
                if (p != o.lastPos)
                {
                    DisplaceObject(o.lastPos);
                    PlaceObject(p, o.obj);
                    objects[ob.Key] = new GameObjectFrame(o.obj, p);
                }
            }
        }
    }
}

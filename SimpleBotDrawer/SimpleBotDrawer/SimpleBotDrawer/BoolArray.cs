using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBotDrawer
{
    public class BoolArray
    {
        public int width;
        public int height;
        public bool[,] tiles;
        public bool nullTile;
        public BoolArray(int width, int height, bool nullTile)
        {
            this.nullTile = nullTile;
            this.width = width;
            this.height = height;
            tiles = new bool[width, height];
        }
        public bool this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < width)
                {
                    if (y >= 0 && y < height)
                    {
                        return tiles[x, y];
                    }
                }
                return nullTile;
            }
            set
            {
                if (x >= 0 && x < width)
                {
                    if (y >= 0 && y < height)
                    {
                        tiles[x, y] = value;
                    }
                }
            }
        }
    }
}

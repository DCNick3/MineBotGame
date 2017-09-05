using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBotDrawer
{
    public class TileArray
    {
        public int width;
        public int height;
        public Tile[,] tiles;
        public Tile nullTile;
        public TileArray(int width, int height,Tile nullTile)
        {
            this.nullTile = nullTile;
            this.width = width;
            this.height = height;
            tiles = new Tile[width, height];
        }
        public Tile this[int x,int y]
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

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SimpleBotDrawer
{
    public class Tile
    {
        public const int minTileSize = 16;
        public const int maxTileSize = 100;
        public static int tileSize = 64;
        public static int startTileSize = 64;
        public static Texture2D tileTexture;
        public static Texture2D fogTexture;
        public static Game1 game;
        public int type;
        //0 - пусто
        //1 - стена
        public int count = 0;//для ресурсов
        public Tile(int type,int count)
        {
            this.type = type;
            this.count = count;
        }
        public void Draw(int X,int Y)
        {
            int[] ii = new int[4];
            if ((game.pole[X, Y].type == 0) && (game.pole[X - 1, Y].type != 0 && game.pole[X, Y - 1].type != 0 && game.pole[X - 1, Y - 1].type != 0) || (game.pole[X, Y].type == 1) && (game.pole[X - 1, Y].type == 0 && game.pole[X, Y - 1].type == 0))
                ii[0] = 0;
            else
                ii[0] = 102;
            if ((game.pole[X,Y].type == 0) && (game.pole[X + 1, Y].type != 0 && game.pole[X, Y - 1].type != 0 && game.pole[X + 1, Y - 1].type != 0) || (game.pole[X,Y].type == 1) && (game.pole[X + 1, Y].type == 0 && game.pole[X, Y - 1].type == 0))
                ii[1] = 0;
            else
                ii[1] = 102;
            if ((game.pole[X,Y].type == 0) && (game.pole[X + 1, Y].type != 0 && game.pole[X, Y + 1].type != 0 && game.pole[X + 1, Y + 1].type != 0) || (game.pole[X,Y].type == 1) && (game.pole[X + 1, Y].type == 0 && game.pole[X, Y + 1].type == 0))
                ii[2] = 0;
            else
                ii[2] = 102;
            if ((game.pole[X,Y].type == 0) && (game.pole[X - 1, Y].type != 0 && game.pole[X, Y + 1].type != 0 && game.pole[X - 1, Y + 1].type != 0) || (game.pole[X,Y].type == 1) && (game.pole[X - 1, Y].type == 0 && game.pole[X, Y + 1].type == 0))
                ii[3] = 0;
            else
                ii[3] = 102;
            for (int i = 0;i < 4; i++)
            {
                game.DrawIncrease(tileTexture, new FRectangle(X - game.mouseGameState.screenCenterPosition.X + 0.5f, Y - game.mouseGameState.screenCenterPosition.Y + 0.5f, (float)(Math.Ceiling(0.5f * tileSize + 1)/tileSize), (float)(Math.Ceiling(0.5f * tileSize + 1) / tileSize)), new Rectangle(ii[i], 102 * game.pole[X, Y].type, 102, 102), Color.White, i * MathHelper.PiOver2, new Vector2(102, 102), game.spriteEffect, 0);
            }
        }
        public void DrawFogOfWar(int X, int Y)
        {
            int[] ii = new int[4];
            if (game.warFog[X, Y] && (!game.warFog[X - 1, Y] && !game.warFog[X, Y - 1] && !game.warFog[X - 1, Y - 1]) || (!game.warFog[X, Y]) && (game.warFog[X - 1, Y] && game.warFog[X, Y - 1]))
                ii[0] = 0;
            else
                ii[0] = 102;
            if ((game.warFog[X, Y]) && (!game.warFog[X + 1, Y] && !game.warFog[X, Y - 1] && !game.warFog[X + 1, Y - 1]) || (!game.warFog[X, Y]) && (game.warFog[X + 1, Y] && game.warFog[X, Y - 1]))
                ii[1] = 0;
            else
                ii[1] = 102;
            if ((game.warFog[X, Y]) && (!game.warFog[X + 1, Y] && !game.warFog[X, Y + 1] && !game.warFog[X + 1, Y + 1]) || (!game.warFog[X, Y]) && (game.warFog[X + 1, Y] && game.warFog[X, Y + 1]))
                ii[2] = 0;
            else
                ii[2] = 102;
            if ((game.warFog[X, Y]) && (!game.warFog[X - 1, Y] && !game.warFog[X, Y + 1] && !game.warFog[X - 1, Y + 1]) || (!game.warFog[X, Y]) && (game.warFog[X - 1, Y] && game.warFog[X, Y + 1]))
                ii[3] = 0;
            else
                ii[3] = 102;
            for (int i = 0; i < 4; i++)
            {
                game.DrawIncrease(fogTexture, new FRectangle(X - game.mouseGameState.screenCenterPosition.X + 0.5f, Y - game.mouseGameState.screenCenterPosition.Y + 0.5f, 0.5f, 0.5f), new Rectangle(ii[i], 102 * (game.warFog[X, Y] ? 0 : 1), 102, 102), Color.White, i * MathHelper.PiOver2, new Vector2(102, 102), game.spriteEffect,0);
            }
        }
        public Vector2 GetFreePole(Random rnd)
        {
            Vector2 v;
            do
            {
                v = new Vector2(rnd.Next(Game1.poleWidth),rnd.Next(Game1.poleHeight));
            }
            while (game.pole[(int)v.X,(int)v.Y].type != 0);
            return v;
        }
    }
}

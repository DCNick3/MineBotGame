using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleBotDrawer
{
    public class Bot : GameObject
    {
        public static Texture2D botTexture;
        public static Texture2D healthTexture;
        private Game1 game;
        public Bot(Color color, Vector2 position, Game1 game)
        {           
            maxHP = 35;
            HP = 30;
            this.game = game;
            this.color = color;
            this.position = position;
        }
        public void Draw()
        {
            game.DrawIncrease(botTexture, new FRectangle((position.X - game.mouseGameState.screenCenterPosition.X), (position.Y - game.mouseGameState.screenCenterPosition.Y), 0.8f, 0.8f), new Rectangle(112, 0, 110, 110), color, 0, new Vector2(53, 55), game.spriteEffect, 0);
            game.DrawIncrease(botTexture, new FRectangle((position.X - game.mouseGameState.screenCenterPosition.X), (position.Y - game.mouseGameState.screenCenterPosition.Y), 0.8f, 0.8f), new Rectangle(2, 0, 110, 110), Color.Black, 0, new Vector2(55, 55), game.spriteEffect, 0);
            DrawHealth(position + new Vector2(-35,45));
        }
        public void DrawHealth(Vector2 v)
        {
            if (hp != maxHP)
            {
                game.DrawIncrease(healthTexture,new FRectangle((v.X - game.mouseGameState.screenCenterPosition.X), (v.Y - game.mouseGameState.screenCenterPosition.Y), 71f/Tile.startTileSize,5f / Tile.startTileSize),new Rectangle(0,0,72,8),Color.White);            
                //game.spriteBatch.Draw(healthTexture, new FRectangle((game.ScreenCenter.X - game.realPos.X + v.X + 1), (game.ScreenCenter.Y - game.realPos.Y + v.Y), 2,3), new Rectangle(1, 10, 2, 6), Color.White);
                game.DrawIncrease(healthTexture, new FRectangle((v.X - game.mouseGameState.screenCenterPosition.X - 1), (v.Y - game.mouseGameState.screenCenterPosition.Y - 1), ((float)hp / maxHP * 89f) / Tile.startTileSize, 3f / Tile.startTileSize), new Rectangle(5, 10, 22, 16), Color.White);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SimpleBotDrawer
{
    public class Building : GameObject
    {
        public int type;
        //0 - база (4*4)
        //1 - склад (3*4)
        //2 - фабрика модулей (4*4)
        //3 - генератор (2*2)
        //4 - пушка (2*2)
        //5 - лаборатория (2*2)
        private Game1 game;
        public static Texture2D gunTexture;
        public static Texture2D factoryTexture;
        public static Texture2D gearwheelTexture;        
        private Point[] GetNearTerritory(Rectangle rectangle)
        {
            List<Point> points = new List<Point>();
            for (int i = rectangle.X;i < rectangle.X + rectangle.Width; i++)
            {
                points.Add(new Point(i, rectangle.Y - 1));
                points.Add(new Point(i, rectangle.Y + rectangle.Height));
            }
            for (int i = rectangle.Y; i < rectangle.Y + rectangle.Height; i++)
            {
                points.Add(new Point(rectangle.X - 1, i));
                points.Add(new Point(rectangle.X + rectangle.Width, i));
            }
            return points.ToArray();
        }
        public Building(Color color,int type,Vector2 position,Game1 game)
        {
            this.color = color;
            this.type = type;
            this.position = position;
            this.game = game;
        }
        public void Draw(GameTime gameTime)
        {
            switch (type)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    game.DrawIncrease(factoryTexture, new FRectangle((position.X - game.mouseGameState.screenCenterPosition.X), (position.Y - game.mouseGameState.screenCenterPosition.Y),4, 4), new Rectangle(0, 0, 407, 407),color);
                    game.DrawIncrease(gearwheelTexture, new FRectangle((position.X - game.mouseGameState.screenCenterPosition.X + 2), (position.Y - game.mouseGameState.screenCenterPosition.Y + 2), 3.5f, 3.5f), new Rectangle(0,0, 94, 82), Color.White, (float)gameTime.TotalGameTime.TotalSeconds, new Vector2(47,41), game.spriteEffect, 0);
                    foreach(var p in GetNearTerritory(new Rectangle((int)position.X, (int)position.Y, 4, 4)))
                    {
                        if (game.pole[p.X,p.Y].type == 0)
                        {
                            game.DrawIncrease(Tile.tileTexture,new FRectangle(p.X - game.mouseGameState.screenCenterPosition.X, p.Y - game.mouseGameState.screenCenterPosition.Y, 1,1),new Rectangle(102,0,102,102),new Color(color.R,color.G,color.B,100));
                        }
                    }
                    break;
                case 3:
                    break;
                case 4:
                    double d = Game1.GetAngle((game.bots[0].position - new Vector2((position.X + 1),(position.Y + 1)))) - MathHelper.PiOver2;
                    game.DrawIncrease(gunTexture, new FRectangle((position.X + 1 - game.mouseGameState.screenCenterPosition.X - (float)(Math.Sqrt(3)/2)),(position.Y - game.mouseGameState.screenCenterPosition.Y), (float)Math.Sqrt(3), 2), new Rectangle(32, 16, 206, 236), Color.White, 0, new Vector2(0,0), game.spriteEffect, 0);                                        
                    game.DrawIncrease(gunTexture, new FRectangle((position.X + 1 - game.mouseGameState.screenCenterPosition.X), (position.Y - game.mouseGameState.screenCenterPosition.Y + 1), 80f/ Tile.startTileSize, 35f/ Tile.startTileSize), new Rectangle(499, 128, 124, 44), Color.White, (float)d, new Vector2(-40, 22), game.spriteEffect, 0);
                    game.DrawIncrease(gunTexture, new FRectangle((position.X + 1 - game.mouseGameState.screenCenterPosition.X), (position.Y - game.mouseGameState.screenCenterPosition.Y + 1), 1.5f, 1.5f), new Rectangle(288, 42, 180, 180), color, 0, new Vector2(90, 90), game.spriteEffect, 0);
                    break;
                case 5:
                    break;
            }
        }
    }
}

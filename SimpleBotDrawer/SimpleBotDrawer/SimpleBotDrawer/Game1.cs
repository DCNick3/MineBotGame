using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace SimpleBotDrawer
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //////////
        public const int poleWidth = 100;
        public const int poleHeight = 100;
        //////////
        Random rnd = new Random();
        RenderTarget2D target;
        GraphicsDeviceManager graphics;
        Texture2D objectBarTexture;
        Texture2D pixelTexture;
        public SpriteEffects spriteEffect;
        public SpriteBatch spriteBatch;
        public MouseGameState mouseGameState;
        public TileArray pole;
        public BoolArray warFog;
        public List<Bot> bots;
        public List<Building>  building = new List<Building>();
        public Vector2 ScreenCenter
        {
            get
            {
                return new Vector2(graphics.PreferredBackBufferWidth / 2.0F, graphics.PreferredBackBufferHeight / 2.0F);
            }
        }
        public static double GetAngle(Vector2 v)
        {
            double l = Math.Sqrt(v.X * v.X + v.Y * v.Y);
            if (l == 0)
            {
                return 0;
            }
            if (v.Y >= 0)
            {
                return Math.PI - Math.Asin(v.X / l);
            }
            else
            {
                return Math.Asin(v.X / l);
            }
        }
        public void DrawIncrease(Texture2D texture, FRectangle where,Rectangle what,Color color,float rotation,Vector2 origin ,SpriteEffects spriteEffects,float layerDepth)
        {
            spriteBatch.Draw(texture,new Rectangle((int)(ScreenCenter.X + where.X * Tile.tileSize), (int)(ScreenCenter.Y + where.Y * Tile.tileSize),(int)(where.width * Tile.tileSize),(int)(where.height * Tile.tileSize)),what,color,rotation,origin,spriteEffects,layerDepth);
        }
        public void DrawIncrease(Texture2D texture, FRectangle where, Rectangle what, Color color)
        {
            spriteBatch.Draw(texture, new Rectangle((int)(ScreenCenter.X + where.X * Tile.tileSize), (int)(ScreenCenter.Y + where.Y * Tile.tileSize), (int)(where.width * Tile.tileSize), (int)(where.height * Tile.tileSize)), what, color);
        }
        private static Tile[,] Generate(Random rnd, int width, int height)
        {
            Tile[,] pole = new Tile[width, height];
            Tile[,] pole1 = new Tile[width, height];
            for (int x = 0; x < pole.GetLength(0); x++)
            {
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    pole[x, y] = new Tile(1,0);
                    pole1[x, y] = new Tile(1,0);
                }
            }
            for (int x = 0; x < pole.GetLength(0); x++)
            {
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    if (rnd.Next(100) <= 57)
                    {
                        pole[x, y] = new Tile(1,0);
                        pole1[x, y] = new Tile(1,0);
                    }
                    else
                    {
                        pole[x, y] = new Tile(0,0);
                        pole1[x, y] = new Tile(0,0);
                    }
                }
            }
            for (int i = 0; i < 7; i++)
            {
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    for (int x = 0; x < pole.GetLength(0); x++)
                    {
                        int k = 0;
                        for (int yy = y - 1; yy <= y + 1; yy++)
                        {
                            for (int xx = x - 1; xx <= x + 1; xx++)
                            {
                                if (!(xx == x && yy == y) && (xx < 0 || yy < 0 || xx >= pole.GetLength(0) || yy >= pole.GetLength(1) || pole1[xx, yy].type != 0 ))
                                {
                                    k++;
                                }
                            }
                        }
                        if (k >= 5)
                        {
                            pole[x, y] = new Tile(1,0);
                        }
                        else
                        {
                            pole[x, y] = new Tile(0,0);
                        }
                    }
                }
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    for (int x = 0; x < pole.GetLength(0); x++)
                    {
                        pole1[x, y] = pole[x, y];
                    }
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    for (int x = 0; x < pole.GetLength(0); x++)
                    {
                        int k = 0;
                        if (x == 0 || pole1[x - 1, y].type != 0 )
                        {
                            k++;
                        }
                        if (y == 0 || pole1[x, y - 1].type != 0 )
                        {
                            k++;
                        }
                        if (x == pole.GetLength(0) - 1 || pole1[x + 1, y].type != 0 )
                        {
                            k++;
                        }
                        if (y == pole.GetLength(1) - 1 || pole1[x, y + 1].type != 0 )
                        {
                            k++;
                        }
                        if (k <= 1 && pole1[x, y].type != 0 )
                        {
                            pole[x, y] = new Tile(0,0);
                        }
                        else
                        {
                            if (k >= 3 && !(pole1[x, y].type != 0))
                            {
                                pole[x, y] = new Tile(1,0);
                            }
                            else
                            {
                                pole[x, y] = pole1[x, y];
                            }
                        }
                    }
                }
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    for (int x = 0; x < pole.GetLength(0); x++)
                    {
                        pole1[x, y] = new Tile(pole[x, y].type, pole[x, y].count);
                    }
                }
            }
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                pole[i, pole.GetLength(1) - 1] = new Tile(1,0);
                pole[i, 0] = new Tile(1,0);
            }
            for (int i = 0; i < pole.GetLength(1); i++)
            {
                pole[pole.GetLength(0) - 1, i] = new Tile(1,0);
                pole[0, i] = new Tile(1,0);
            }
            return pole;
        }
        public Game1()
        {            
            mouseGameState = new MouseGameState(Mouse.GetState());
            building.Add(new Building(new Color(1,177,227),4,new Vector2(50,50),this));
            building.Add(new Building(new Color(241, 78, 83), 2 , new Vector2(52, 52), this));
            bots = new List<Bot>();
            bots.Add(new Bot(new Color(241, 78, 83), new Vector2(90,90),this));
            IsMouseVisible = true;
            Tile.game = this;
            pole = new TileArray(poleWidth, poleHeight, new Tile(1, 0));
            pole.tiles = Generate(rnd, poleWidth, poleHeight);
            warFog = new BoolArray(poleWidth,poleHeight,true);
            for (int x = 0;x < poleWidth;x++)
            {
                for (int y = 0;y < poleHeight;y++)
                {
                    warFog[x, y] = false;
                }
            }
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 780; //graphics.PreferredBackBufferHeight * 2;
            graphics.PreferredBackBufferWidth = 1279;// graphics.PreferredBackBufferWidth * 2;            
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Building.factoryTexture = Content.Load<Texture2D>("Fabric");
            Building.gearwheelTexture = Content.Load<Texture2D>("gearwheel");
            Building.gunTexture = Content.Load<Texture2D>("Gun");
            Bot.botTexture = Content.Load<Texture2D>("bot");
            Bot.healthTexture = Content.Load<Texture2D>("healthbar");
            Tile.tileTexture = Content.Load<Texture2D>("Simple_Tile");
            Tile.fogTexture = Content.Load<Texture2D>("WarFog");
            objectBarTexture = Content.Load<Texture2D>("BotBar");
            pixelTexture = Content.Load<Texture2D>("Pixel");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            target = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

        }
        protected override void UnloadContent()
        {

        }
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            var mouse = Mouse.GetState();
            mouseGameState.Update(mouse);
            bots[0].position = mouseGameState.screenCenterPosition;
            base.Update(gameTime);
        }
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {           
            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,SamplerState.LinearClamp, DepthStencilState.Default,RasterizerState.CullNone);
            DrawTiles();
            DrawBots();            
            DrawBuildings(gameTime);                       
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Draw(target, new Vector2(0, 0), new Rectangle(0, 0, target.Width, target.Height), Color.White);
            DrawObjectBar(bots[0], target);            
            spriteBatch.End();
            base.Draw(gameTime);
        }
        void DrawTiles()
        {
            for (int x = (int)MathHelper.Max(0,(mouseGameState.screenCenterPosition.X - graphics.PreferredBackBufferWidth/2/Tile.tileSize) - 1); x < (int)MathHelper.Min(poleWidth, mouseGameState.screenCenterPosition.X + graphics.PreferredBackBufferWidth / 2 / Tile.tileSize + 2); x++)
            {
                for (int y = (int)MathHelper.Max(0, (mouseGameState.screenCenterPosition.Y - graphics.PreferredBackBufferHeight / 2 / Tile.tileSize) - 1); y < (int)MathHelper.Min(poleHeight, mouseGameState.screenCenterPosition.Y + graphics.PreferredBackBufferHeight / 2 / Tile.tileSize + 2); y++)
                {
                    pole[x, y].Draw(x, y);
                    pole[x, y].DrawFogOfWar(x, y);
                }
            }
        }
        void DrawBots()
        {
            foreach (var bot in bots)
            {
                bot.Draw();
            }
        }
        void DrawBuildings(GameTime gameTime)
        {
            foreach (var b in building)
            {
                b.Draw(gameTime);
            }
        }
        void DrawObjectBar(GameObject gameObject,RenderTarget2D target)
        {
            int x = target.Width;
            int y = target.Height;
            spriteBatch.Draw(target,new Rectangle(12,581,176,176),new Rectangle/*(x,y,x + 175,y + 175)*/((int)(MathHelper.Max(0,x - y)/2), (int)(MathHelper.Max(0, y - x) / 2), (int)MathHelper.Min(x, y), (int)MathHelper.Min(x, y)),Color.White);
            spriteBatch.Draw(objectBarTexture, new Vector2(0, 570), new Rectangle(0, 0, 1279, 210), Color.White);
            spriteBatch.Draw(pixelTexture,new Rectangle(18,764, (int)(162/(float)gameObject.maxHP * (float)gameObject.HP),8),new Rectangle(0,0,1,1),Color.Red);
        }
    }
}

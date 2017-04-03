using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MineBotGame
{
    public class Game
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
       (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Game()
        {
            gameBots = new List<PlayerController>();
        }

        public void Init()
        {
            rnd = new Random();
            area = GameRoom.Generate(rnd, GameRoom.Walls.All);

            log.Debug("Starting bots");
            for (int i = 0; i < botCount; i++)
            {
                PlayerController bot = new LuaController();
                bot.Id = gameBots.Count;
                bot.Start();
                //bot.Position = area.GetRandomFreeWithWall(rnd);
                //area[bot.Position].bot = bot;
                gameBots.Add(bot);
            }
            log.Debug("Bots started");
            /*bot = new LuaGameBot();
            bot.Position = area.GetRandomFree(rnd);
            bot.Start();
            area[bot.Position].bot = bot;
            gameBots.Add(bot);*/

            onRender += Render;

            if (render > 0)
            {
                log.Debug("Render > 0, so setting console size to max");
                Console.BufferWidth = Console.LargestWindowWidth;
                Console.WindowWidth = Console.WindowWidth;
                Console.BufferHeight = Console.LargestWindowHeight;
                Console.WindowHeight = Console.BufferHeight;
            }
            log.Info("Game initialized");
        }

        public void Run()
        {
            do
            {
                isStopRequested = false;
                isGoingToRestart = false;

                Init();

                startTime = DateTime.Now;
                while (!isStopRequested && !cancellationToken.IsCancellationRequested)
                {

                    DateTime st = DateTime.Now;
                    Update();

                    /*if (render > 0)
                    {
                        Render();
                    }*/
                    while (invokers.Count > 0)
                    {
                        var x = invokers.Dequeue();
                        x.result = x.method.DynamicInvoke(x.pars);
                        x.completeEvent.Set();
                    }
                    tpsCounter++;
                    if ((DateTime.Now - lastTpsReset).TotalSeconds > 1)
                    {
                        lastTpsReset = DateTime.Now;
                        Tps = tpsCounter;
                        tpsCounter = 0;
                    }
                    if ((DateTime.Now - lastStatsUpdate).TotalSeconds > 0.1)
                    {
                        lastStatsUpdate = DateTime.Now;
                        onStatsUpdate?.Invoke(GetStatsInternal());
                    }
                    if ((DateTime.Now - lastRender).TotalSeconds > 1.0 / targetFPS)
                    {
                        lastRender = DateTime.Now;
                        onRender?.Invoke(this);
                    }

                    Thread.Sleep(Math.Max(tickDelay - (int)(DateTime.Now - st).TotalMilliseconds - 1, 0));
                    updateTime = DateTime.Now - st;
                }

                StopGame();
            } while (isGoingToRestart);


            Program.cancelSource.Cancel();
        }

        private GameStats GetStatsInternal()
        {
            return new GameStats(AverageTps, aliveBots, Tick, Tps);
        }

        protected void StopGame()
        {
            log.Info("Stopping game");
            int c = gameBots.Count;
            Task[] tt = new Task[c];
            for (int i = 0; i < c; i++)
            {
                var x = i;
                tt[i] = new Task(() => gameBots[x].Stop());
                tt[i].Start();
            }
            Task.WaitAll(tt);

            tick = 0;
            gameBots.Clear();

            log.Info("Game stopped");
        }

        protected void Update()
        {
            aliveBots = 0;
            for (int i = 0; i < gameBots.Count; i++)
                if (gameBots[i].Alive)
                {
                    aliveBots++;
                    gameBots[i].Update(this);
                }
            tick++;
        }

        protected static void Render(Game g)
        {
            //Console.Clear();
            Console.SetCursorPosition(0, 0);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("T# {0:000000}; TPS {1:0000.00}; Bn: {2}                     \n", g.tick, 1.0 / g.updateTime.TotalSeconds, g.aliveBots);
            if (g.render > 1)
            {
                var area = g.area;
                char[,] chrs = new char[area.Height, area.Width];
                for (int i = 0; i < area.Height; i++)
                {
                    for (int j = 0; j < area.Width; j++)
                    {
                        if (area[j, i].IsObstacle)
                            chrs[i, j] = 'X';
                        else
                            chrs[i, j] = '.';
                    }
                }
                //foreach (var b in g.gameBots)
                   // chrs[(int)b.Position.Y, (int)b.Position.X] = '*';
                for (int i = 0; i < area.Height; i++)
                {
                    for (int j = 0; j < area.Width; j++)
                    {
                        sb.Append(chrs[i, j]);
                    }
                    sb.AppendLine();
                }
            }
            Console.Write(sb.ToString());
            
        }

        public void Stop()
        {
            StopAsync().Wait();
        }

        public void Restart()
        {
            RestartAsync().Wait();
        }

        public Task RestartAsync()
        {
            log.Info("Restart was requested");
            isGoingToRestart = true;
            var x = Invoke(new Action(() =>
            {
                isStopRequested = true;
            }));
            var t = new Task(() => x.completeEvent.WaitOne());
            t.Start();
            return t;
        }

        public Task StopAsync()
        {
            log.Info("Stop was requested");
            var x = Invoke(new Action(() => isStopRequested = true));
            var t = new Task(() => x.completeEvent.WaitOne());
            t.Start();
            return t;
        }

        public Task<GameStats> GetStatsAsync()
        {
            var x = Invoke(new Func<GameStats>(() => 
            {
                return GetStatsInternal();
            }));
            var t = new Task<GameStats>(() =>
            {
                x.completeEvent.WaitOne();
                return (GameStats)x.result;
            });
            t.Start();
            return t;
        }

        public InvokeParams Invoke(Delegate method, params object[] pars)
        {
            var p = new InvokeParams(method, pars);
            invokers.Enqueue(p);
            return p;
        }

        public event Action<GameStats> onStatsUpdate;
        public event Action<Game> onRender;

        Random rnd;
        TimeSpan updateTime = TimeSpan.FromSeconds(1);
        int render = 2;
        int botCount = 0;
        int tick = 0;
        int tickDelay = 10;
        int aliveBots = 0;
        DateTime lastTpsReset = DateTime.Now;
        int tpsCounter = 0;
        DateTime startTime;
        bool isStopRequested = false;
        DateTime lastStatsUpdate = DateTime.Now;
        double targetFPS = 20.0;
        DateTime lastRender = DateTime.Now;
        bool isGoingToRestart = false;

        internal GameRoom area;
        Queue<InvokeParams> invokers = new Queue<InvokeParams>();
        internal List<PlayerController> gameBots;


        public int Tick { get { return tick; } }
        public double AverageTps { get { return tick / (DateTime.Now - startTime).TotalSeconds; } }
        public int Tps { get; private set; }
        public CancellationToken cancellationToken = new CancellationToken();

        public class GameStats
        {
            internal GameStats(double avgTps, int alvBts, int tick, int tps)
            {
                averageTps = avgTps;
                aliveBots = alvBts;
                tickNumber = tick;
                this.tps = tps;
            }

            public double averageTps;
            public int aliveBots, tickNumber, tps;

            public override string ToString()
            {
                return string.Format("TPS: {0:0000} Avg(TPS): {1:0000.0} AliveBots: {2} Tick# {3:00000000}", tps, averageTps, aliveBots, tickNumber);
            }
        }

        public class InvokeParams
        {
            public InvokeParams(Delegate method, object[] pars)
            {
                this.method = method;
                this.pars = pars;
                this.result = null;
                completeEvent = new AutoResetEvent(false);
            }
            internal Delegate method;
            internal object[] pars;
            public object result;
            public AutoResetEvent completeEvent;
        }
    }
}

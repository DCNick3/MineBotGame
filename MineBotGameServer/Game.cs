using MineBotGame.GameObjects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MineBotGame
{
    /// <summary>
    /// Main class, that designed to manage all connections, game objects, players etc.
    /// </summary>
    public class Game
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Game(PlayerController[] players)
        {
            this.players = new Player[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                this.players[i] = new Player(this, i, players[i]);
            }
        }

        public void Init()
        {
            rnd = new Random();
            area = GameArea.Generate(rnd, GameArea.GeneratorParameters.Default);

            RegisterGameObject(Building.NewBuilding(BuildingType.Base, players[0], NewGameObjectId(), new Vector2(5f,5f)));

            onRender -= Render;

            if (render > 0)
            {
                log.Debug("Render > 0, so setting console size to max");
                Console.BufferWidth = Console.LargestWindowWidth;
                Console.WindowWidth = Console.WindowWidth;
                Console.BufferHeight = Console.LargestWindowHeight + 100;
                Console.WindowHeight = Console.BufferHeight - 100;

                onRender += Render;
            }

            log.Debug("Starting controllers");

            for (int i = 0; i < players.Length; i++)
            {
                players[i].Parameters = players[i].Controller.Start(i);
                log.DebugFormat("Player #{0}: {1}", i, players[i].Parameters.ToString());
            }

            log.Info("Game initialized");
        }

        public void Run()
        {
            isStopRequested = false;

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


            Program.cancelSource.Cancel();
        }

        private GameStats GetStatsInternal()
        {
            return new GameStats(AverageTps, 0, Tick, Tps);
        }

        protected void StopGame()
        {
            log.Info("Stopping game");

            for (int i = 0; i < players.Length; i++)
                players[i].Controller.Stop();


            log.Info("Game stopped");
        }

        protected void Update()
        {
            for (int i = 0; i < players.Length; i++)
            {
                var p = players[i];
                p.Update(GameState.ConstructGameState(p, this), this); // TODO: this must be not null... 

                area.RebaseObjects();
            }
            tick++;
        }

        protected static void Render(Game g)
        {
            //Console.Clear();
            Console.SetCursorPosition(0, 0);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("T# {0:000000}; TPS {1:0000.00}; Bn: {2}                     \n", g.tick, 1.0 / g.updateTime.TotalSeconds, 0);
            if (g.render > 1)
            {
                var area = g.area;
                char[,] chrs = new char[area.Height, area.Width];
                for (int i = 0; i < area.Height; i++)
                {
                    for (int j = 0; j < area.Width; j++)
                    {
                        var t = area[i, j];
                        char c = '.';

                        if (t.IsObstacle)
                            c = 'X';
                        if (t.Object is Unit)
                            c = '*';
                        if (t.Object is Building)
                            c = '#';

                        chrs[i, j] = c;
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
        
        public int NewGameObjectId()
        {
            return ++maxId;
        }

        private void RegisterGameObject(GameObject obj)
        {
            area.AddObject(obj);
            obj.OwnerPlayer.Objects[obj.Id] = obj;
            if (obj is Building)
            {
                var b = obj as Building;
                b.OnAdd(b.OwnerPlayer);
            }
        }

        public event Action<GameStats> onStatsUpdate;
        public event Action<Game> onRender;

        // Configutrable parameters

        // Render level (from 0 to 2)
        int render = 2;

        // Various timing configurations
        TimeSpan updateTime = TimeSpan.FromSeconds(1);
        int tick = 0;
        int tickDelay = 0;
        int tpsCounter = 0;
        double targetFPS = 20.0;

        Random rnd;
        DateTime startTime;
        DateTime lastTpsReset = DateTime.Now;
        DateTime lastStatsUpdate = DateTime.Now;
        DateTime lastRender = DateTime.Now;
        bool isStopRequested = false;
        Player[] players;
        int maxId = 0;

        internal GameArea area;
        Queue<InvokeParams> invokers = new Queue<InvokeParams>();


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

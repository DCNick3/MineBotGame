﻿using System.Threading;
using System.Windows.Forms;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace MineBotGame
{
    /// <summary>
    /// Entry class
    /// </summary>
    public class Program
    {
        public static CancellationTokenSource cancelSource;
        public static CancellationToken cancellationToken;
        public static ControlForm controlForm;

        static void ControlFormWorker()
        {
            Application.EnableVisualStyles();
            Application.Run(controlForm);
        }

        static Thread controlFormThread;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
       (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            cancelSource = new CancellationTokenSource();
            cancellationToken = cancelSource.Token;

            PlayerController[] cnt = new PlayerController[2];

            cnt[0] = new TestController();
            cnt[1] = new DummyController();

            Game game = new Game(cnt);

            controlFormThread = new Thread(ControlFormWorker);
            controlForm = new ControlForm(game);
            controlFormThread.Start();
            controlForm.loadEvent.WaitOne();
            

            log.Info("Game starting begin.");
            game.cancellationToken = cancellationToken;
            game.Run();
            log.Info("Goodbye.");
        }
    }
}

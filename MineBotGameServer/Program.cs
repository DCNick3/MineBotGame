using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace MineBotGame
{

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


            Game game = new Game();

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

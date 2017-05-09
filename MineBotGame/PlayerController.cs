using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MineBotGame
{
    public abstract class PlayerController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
       (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public PlayerController()
        {
            Alive = true;
            cancelSource = new CancellationTokenSource();
            cancelToken = cancelSource.Token;
        }

        Thread botThread;

        protected ManualResetEvent botToProgramEvent = new ManualResetEvent(false);
        protected ManualResetEvent programToBotEvent = new ManualResetEvent(false);
        protected PlayerAction botToProgram = null;
        protected ControllerActionResult programToBot = null;
        
        protected CancellationToken cancelToken;
        private CancellationTokenSource cancelSource;

        protected abstract void BotWorker();
        
        public bool Alive { get; private set; }
        public int Id { get; internal set; }

        public void Start()
        {
            log.DebugFormat("Starting bot# {0} worker.", Id);
            botThread = new Thread(BotWorker);
            botThread.Start();
        }

        public void Update(Game game)
        {
            if (!botToProgramEvent.WaitOne(2000))
            {
                if (botThread.IsAlive)
                {
                    log.WarnFormat("Bot# {0} running time exceeded 2 s; it will be killed", Id);

                    botThread.Abort();
                }
                Alive = false;
                return;
            }
            botToProgramEvent.Reset();

            switch (botToProgram.type)
            {
                case PlayerAction.Type.Idle:
                    break;

                default:
                    throw new NotImplementedException();
            }
            
            programToBotEvent.Set();
        }

        public void Stop()
        {
            //cancelSource.Cancel();
            //if (!botThread.Join(500))
                botThread.Abort();
        }
    }

    public class PlayerAction
    {
        /*
         Actien can be described with Type-code and Vector (parameter)
             */

        public enum Type
        {
            Idle = 0,
        }

        public Type type;
        public Vector2 vector;

        public int TickLength
        {
            get
            {
                switch (type)
                {
                    default:
                        return 0;
                }
            }
        }

        public void Serialize(Stream str)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream str)
        {
            throw new NotImplementedException();
        }
    }

    public class ControllerActionResult
    {
        public bool IsSucceed { get; set; }
        public int ErrorCode { get; set; }

        public enum Error
        {
        }

        public void Serialize(Stream str)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream str)
        {
            throw new NotImplementedException();
        }
    }
}

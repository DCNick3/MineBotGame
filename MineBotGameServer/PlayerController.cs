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
        {}

        public void Start()
        {
        }

        public void Update(Game game)
        {
        }

        public void Stop()
        {
        }
    }
}

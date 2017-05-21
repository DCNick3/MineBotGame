using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MineBotGame.PlayerActions;


namespace MineBotGame
{
    public abstract class PlayerController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
       (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public PlayerController()
        {}

        private Queue<PlayerAction> actions = new Queue<PlayerAction>();
        private List<PlayerActionResult> results = new List<PlayerActionResult>();

        /// <summary>
        /// Number of actions in internal queue
        /// </summary>
        public int ActionCount { get { return actions.Count; } }

        /// <summary>
        /// Pops action from the beginning of internal queue 
        /// </summary>
        /// <returns>Popped action if there is one or null</returns>
        public PlayerAction PopAction()
        {
            if (actions.Count != 0)
                return actions.Dequeue();
            else
                return null;
        }

        /// <summary>
        /// Pushes action to the end of internal queue
        /// </summary>
        /// <param name="action">Action to push</param>
        protected void PushAction(PlayerAction action)
        {
            actions.Enqueue(action);
        }

        public void PushResult(PlayerActionResult res)
        {
            results.Add(res);
        }

        protected PlayerActionResult[] PopResults()
        {
            var r = results.ToArray();
            results.Clear();
            return r;
        }

        /// <summary>
        /// Called when <see cref="PlayerController"/> is initialized
        /// </summary>
        public abstract PlayerParameters Start(int playerId);

        /// <summary>
        /// Called each game tick. Controller must process game state and determine what to do (push it with <see cref="PlayerController.PushAction(PlayerAction)"/>) 
        /// </summary>
        /// <param name="game">Game state, that is available to this player</param>
        public abstract void Update(GameStateDelta game);

        /// <summary>
        /// Called when <see cref="PlayerController"/> is going down. It must free all used resources in this method.
        /// </summary>
        public abstract void Stop();
    }
}

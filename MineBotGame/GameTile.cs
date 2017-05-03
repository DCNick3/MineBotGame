using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame
{
    public class GameTile
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
       (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GameTile()
        {

        }

        public GameTile(bool isObstacle) : this()
        {
            IsObstacle = isObstacle;
        }

        bool isObstacle;
        public virtual bool IsObstacle
        {
            get
            {
                return isObstacle || bot != null;
            }
            protected set
            {
                isObstacle = value;
            }
        }
        public PlayerController bot = null;

        public virtual new Type GetType()
        {
            if (bot != null)
                return Type.Bot;
            if (IsObstacle)
                return Type.Obstacle;
            else
                return Type.None;
        }

        public enum Type
        {
            None = 0,
            Obstacle = 1,
            Ore = 2,
            Bot = 3,
            Building = 4,
        }
    }

    public class GameTileOre : GameTile
    {
        public GameTileOre(GameResource resource, int capacity) : base(true)
        {
            Capacity = capacity;
            Resource = resource;
        }

        public GameResource Resource { get; protected set; }
        public int Capacity { get; set; }

        public override Type GetType()
        {
            return Type.Ore;
        }
    }
}

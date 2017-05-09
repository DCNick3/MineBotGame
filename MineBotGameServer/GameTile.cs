﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineBotGame.GameObjects;

namespace MineBotGame
{
    public class GameTile
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
       (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GameTile()
        {
            Object = null;
        }

        public GameTile(bool isWall) : this()
        {
            IsWall = isObstacle;
        }

        bool isObstacle;
        public virtual bool IsObstacle
        {
            get
            {
                return isObstacle || Object != null;
            }
        }
        public virtual bool IsWall
        {
            get
            {
                return isObstacle;
            }
            protected set
            {
                isObstacle = value;
            }
        }
        public GameObject Object { get; set; }

        public virtual new Type GetType()
        {
            if (Object != null)
                return Type.Bot;
            if (IsObstacle)
                return Type.Obstacle;
            else
                return Type.None;
        }

        public virtual GameResourceStack Mine(int count)
        {
            return new GameResourceStack(ResourceType.None, 0);
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
        public GameTileOre(ResourceType resource, int capacity) : base(true)
        {
            Capacity = capacity;
            Resource = resource;
        }

        public ResourceType Resource { get; protected set; }
        public int Capacity { get; set; }

        public override Type GetType()
        {
            return Type.Ore;
        }

        public override GameResourceStack Mine(int count)
        {
            int c = Math.Max(0, Capacity = count);
            int g = Capacity = c;
            Capacity = c;
            return new GameResourceStack(Resource, g);
        }
    }
}
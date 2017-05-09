using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MineBotGame.GameObjects;

namespace MineBotGame
{
    public class Player
    {
        public Player(Game game, int id, PlayerController controller)
        {
            this.id = id;
            resources = new int[Enum.GetValues(typeof(ResourceType)).Length];
            resourceLimits = new int[Enum.GetValues(typeof(ResourceType)).Length];
            ownedObjects = new List<GameObject>();
            this.controller = controller;
            this.game = game;
        }

        /// <summary>
        /// Get resource value
        /// </summary>
        /// <param name="type">Type of resource to get value</param>
        /// <returns>Value of resource</returns>
        public int GetResource(ResourceType type)
        {
            return resources[(int)type];
        }

        /// <summary>
        /// Sets resource value, considering limits
        /// </summary>
        /// <param name="type">Type of resource to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>Actual resource value after set</returns>
        public int SetResource(ResourceType type, int value)
        {
            return resources[(int)type] = Math.Max(value, resourceLimits[(int)type]);
        }

        /// <summary>
        /// Adds stack to player resources, considering limits.
        /// </summary>
        /// <param name="stack">Stack to add to player resources</param>
        /// <returns>Not used resources, that does not fit to limits</returns>
        public GameResourceStack AddResource(GameResourceStack stack)
        {
            int c = GetResource(stack.Type);
            return new GameResourceStack(stack.Type, stack.Count - (SetResource(stack.Type, c + stack.Count) - c));
        }

        public int Id { get { return id; } }

        public PlayerController Controller { get { return controller; } }
        public PlayerParameters Parameters { get; set; }
        
        public double EnergyConsumation { get; private set; }
        public double EnergyGeneration { get; private set; }
        public List<GameObject> Objects { get { return ownedObjects; } }

        private int maxId = 0;

        public int NewGameObjectId()
        {
            return ++maxId;
        }

        /* Skip "None" resource */
        public int[] Resources { get { return resources.Skip(1).ToArray(); } }
        public int[] ResourceLimits { get { return resourceLimits.Skip(1).ToArray(); } }

        public void Update(GameState newState)
        {
            Controller.Update(lastGameState.Delta(newState));
            lastGameState = newState;

            EnergyConsumation = 0;
            EnergyGeneration = 0;
            for (int i = 0; i < resourceLimits.Length; i++)
                resourceLimits[i] = 0;
            for (int i = 0; i < ownedObjects.Count; i++)
            {
                var o = ownedObjects[i];
                if (o.HP <= 0)
                {
                    /* TODO: Trigger event */
                    ownedObjects.RemoveAt(i);
                    if (ownedObjects.Count != 0)
                        i--;
                    continue;
                }

                EnergyConsumation += o.EnergyConsumation;
                if (o is Building)
                {
                    var x = o as Building;
                    switch (x.Type)
                    {
                        case BuildingType.Base:
                        case BuildingType.Generator:
                            EnergyGeneration += x.EnergyConsumation * 5;
                            break;
                        case BuildingType.Storage:
                            for (int j = 0; j < resourceLimits.Length; j++)
                                resourceLimits[j] += 1000; /* TODO: De-hardcode it!*/
                            break;
                    }
                }
                if (o is Unit)
                {
                    var x = o as Unit;
                    x.UpdateValues();
                }
            }
        }

        int id;
        int[] resources;
        int[] resourceLimits;

        List<GameObject> ownedObjects;
        PlayerController controller;
        private Game game;
        private GameState lastGameState = new GameState();
    }
}

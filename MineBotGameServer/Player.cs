using MineBotGame.GameObjects;
using MineBotGame.PlayerActions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MineBotGame
{
    public class Player
    {
        public Player(Game game, int id, PlayerController controller)
        {
            this.id = id;
            resources = new ResourceTotality();
            resourceLimits = new ResourceTotality();
            ownedObjects = new Dictionary<int, GameObject>();
            this.controller = controller;
            this.game = game;

            Resources.AddAll(10);
        }

        /// <summary>
        /// Get resource value
        /// </summary>
        /// <param name="type">Type of resource to get value</param>
        /// <returns>Value of resource</returns>
        public int GetResource(ResourceType type)
        {
            return resources[type];
        }

        /// <summary>
        /// Sets resource value, considering limits
        /// </summary>
        /// <param name="type">Type of resource to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>Actual resource value after set</returns>
        public int SetResource(ResourceType type, int value)
        {
            return resources[type] = Math.Max(value, resourceLimits[type]);
        }

        /// <summary>
        /// Adds stack to player resources, considering limits.
        /// </summary>
        /// <param name="stack">Stack to add to player resources</param>
        /// <returns>Not used resources, that does not fit to limits</returns>
        public ResourceStack AddResource(ResourceStack stack)
        {
            int c = GetResource(stack.Type);
            return new ResourceStack(stack.Type, stack.Count - (SetResource(stack.Type, c + stack.Count) - c));
        }

        public int Id { get { return id; } }

        public PlayerController Controller { get { return controller; } }
        public PlayerParameters Parameters { get; set; }
        
        public double EnergyConsumation { get; set; }
        public double EnergyGeneration { get; set; }
        public Dictionary<int, GameObject> Objects { get { return ownedObjects; } }

        
        public ResourceTotality Resources { get { return resources; } }
        public ResourceTotality ResourceLimits { get { return resourceLimits; } }

        private ActionError ProcessAction(PlayerAction ac)
        {
            switch (ac.ActionType)
            {
                case PlayerActionType.StartResearch:
                    {
                        var a = ac as PlayerActionOperation;
                        bool isGlobal = (a.Type & 0x80000000) != 0;
                        int type = a.Type & 0x7FFFFFFF;
                        int msk = (isGlobal ? (int)GlobalResearch.All : (int)LocalResearch.All);
                        if ((type & msk) != type)
                            return ActionError.ImpossibleAction;
                        GameObject o;
                        if (!Objects.TryGetValue(a.Id, out o))
                            return ActionError.GameObjectDoesNotExists;
                        if (!(o is Building))
                            return ActionError.ImpossibleAction;
                        var b = o as Building;
                        if (isGlobal)
                        {
                            return b.DoGlobalResearch((GlobalResearch)type);
                        }
                        else
                        {
                            return b.DoLocalResearch((LocalResearch)type);
                        }
                    }
                case PlayerActionType.Idle:
                    return ActionError.Succeed;
                case PlayerActionType.CancelBuildingAction:
                    {
                        var a = ac as PlayerActionCancel;
                        GameObject o;
                        if (!Objects.TryGetValue(a.Id, out o))
                            return ActionError.GameObjectDoesNotExists;
                        if (!(o is Building))
                            return ActionError.ImpossibleAction;
                        var b = o as Building;
                        return b.CancelOperation(a.QueueIndex);
                    }
            }
            return ActionError.ImpossibleAction;
        }

        public bool CheckResources(ResourceStack st)
        {
            return CheckResources(new ResourceTotality().Add(st));
        }

        public bool CheckResources(ResourceTotality res)
        {
            if (Resources >= res)
                return true;
            else
                return false;
        }

        public bool CheckEnergy(int energy)
        {
            if (EnergyGeneration - EnergyConsumation >= energy && EnergyConsumation + energy >= 0)
                return true;
            else
                return false;
        }

        public void UtilizeResources(ResourceTotality res)
        {
            if (!CheckResources(res))
                throw new Exception();
            resources = Resources - res;
        }

        public void UtilizeEnergy(int energy)
        {
            if (!CheckEnergy(energy))
                throw new Exception();
            EnergyConsumation += energy;
        }

        public void Update(GameState newState, Game game)
        {
            Controller.Update(lastGameState.Delta(newState));
            lastGameState = newState;

            while (Controller.ActionCount != 0)
            {
                var a = Controller.PopAction();
                var r = ProcessAction(a);
                Controller.PushResult(new PlayerActionResult(a.ActionId, r));
            }

            //EnergyConsumation = 0;
            //EnergyGeneration = 0;
            /*foreach (var r in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
                resourceLimits[r] = 0;*/
            for (int i = 0; i < ownedObjects.Count; i++)
            {
                var o = ownedObjects.ElementAt(i).Value;
                o.Update();
                if (o.HP <= 0)
                {
                    /* TODO: Trigger event */
                    ownedObjects.Remove(o.Id);
                    if (ownedObjects.Count != 0)
                        i--;
                    continue;
                }

                //EnergyConsumation += o.EnergyConsumation;
                /*if (o is Building)
                {
                    var x = o as Building;
                    switch (x.Type)
                    {
                        case BuildingType.Base:
                        case BuildingType.Generator:
                            EnergyGeneration += x.EnergyConsumation * 5;
                            break;
                        case BuildingType.Storage:
                            foreach (var r in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
                                resourceLimits[r] += 1000; // TODO: De-hardcode it!
                            break;
                    }
                }*/  //TODO: Make it work with decorator
                if (o is Unit)
                {
                    var x = o as Unit;
                    x.UpdateValues();
                }
            }
        }

        int id;
        ResourceTotality resources;
        ResourceTotality resourceLimits;

        Dictionary<int, GameObject> ownedObjects;
        PlayerController controller;
        private Game game;
        private GameState lastGameState = new GameState();
    }
}

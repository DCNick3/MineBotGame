using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;
using MineBotGame.GameObjects;

namespace MineBotGame
{
    public class GameState
    {
        public GameState()
        {

        }

        public int PlayerId { get; set; }
        public int[] Resources { get; set; }
        public int[] ResourceLimits { get; set; }
        public double EnergyConsumation { get; set; }
        public double EnergyGeneration { get; set; }
        public GameObject[] GameObjects { get; set; }

        public GameStateDelta Delta(GameState newGameState)
        {
            return new GameStateDelta()
            {
                PlayerId = PlayerId,
                Resources = Resources,
                ResourceLimits = ResourceLimits,
                EnergyConsumation = EnergyConsumation,
                EnergyGeneration = EnergyGeneration,
                RemovedObjects = GameObjects.Select((_) => _.Id).Where((_) => !newGameState.GameObjects.Any((__) => __.Id == _)).ToArray(),
                NewObjects = newGameState.GameObjects.Where((_) => !GameObjects.Any((__) => __.Id == _.Id)).ToArray(),
            };
        }

        public static GameState ConstructGameState(Player player, Game game)
        {
            List<GameObject> objs = new List<GameObject>();
            objs.AddRange(player.Objects.Select((_) => _.Clone())); // Add current player's object
            objs.AddRange(game.area.Objects.Where((_) => _.OwnerPlayer.Id != player.Id && player.Objects.Any((__) => __.IsVisible(_.Center)))); // Add visible objects of other players

            return new GameState() { EnergyConsumation = player.EnergyConsumation, EnergyGeneration = player.EnergyConsumation,
                GameObjects = objs.ToArray(), PlayerId = player.Id, ResourceLimits = player.ResourceLimits, Resources = player.Resources };
        }
    }

    public class GameStateDelta
    {
        public int PlayerId { get; set; }
        public int[] Resources { get; set; }
        public int[] ResourceLimits { get; set; }
        public double EnergyConsumation { get; set; }
        public double EnergyGeneration { get; set; }

        public int[] RemovedObjects { get; set; }
        public GameObject[] NewObjects { get; set; }

        public void Serialize(Stream str)
        {
            BinaryWriter bw = new BinaryWriter(str);
            bw.Write(PlayerId);
            bw.WriteInts(Resources);
            bw.WriteInts(ResourceLimits);
            bw.Write(EnergyGeneration);
            bw.Write(EnergyConsumation);

            bw.WriteInts(RemovedObjects);

        }
    }
}
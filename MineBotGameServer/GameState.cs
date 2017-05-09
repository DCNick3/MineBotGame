using System;
using System.Collections.Generic;
using System.Linq;
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
        public GameObject[] GameObjects { get; set; }
        public double EnergyConsumation { get; set; }
        public double EnergyGeneration { get; set; }

        public GameStateDelta Delta(GameState newGameState)
        {
            return new GameStateDelta();
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

    }
}
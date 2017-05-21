using MineBotGame.GameObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MineBotGame
{
    public class GameState
    {
        public GameState()
        {
            Resources = new ResourceTotality();
            ResourceLimits = new ResourceTotality();
            GameObjects = new GameObject[0];
        }

        public int PlayerId { get; set; }
        public ResourceTotality Resources { get; set; }
        public ResourceTotality ResourceLimits { get; set; }
        public double EnergyConsumation { get; set; }
        public double EnergyGeneration { get; set; }
        public GameObject[] GameObjects { get; set; }

        public GameStateDelta Delta(GameState newGameState)
        {
            return new GameStateDelta()
            {
                PlayerId = newGameState.PlayerId,
                Resources = newGameState.Resources.ToArray(),
                ResourceLimits = newGameState.ResourceLimits.ToArray(),
                EnergyConsumation = newGameState.EnergyConsumation,
                EnergyGeneration = newGameState.EnergyGeneration,
                Objects = newGameState.GameObjects,

                //RemovedObjects = GameObjects.Select((_) => _.Id).Where((_) => !newGameState.GameObjects.Any((__) => __.Id == _)).ToArray(),
                //NewObjects = newGameState.GameObjects.Where((_) => !GameObjects.Any((__) => __.Id == _.Id)).ToArray(),
            };
        }

        public static GameState ConstructGameState(Player player, Game game)
        {
            List<GameObject> objs = new List<GameObject>();
            objs.AddRange(player.Objects.Select((_) => _.Value/*.Clone()*/)); // Add current player's object
            objs.AddRange(game.area.Objects.Where((_) => _.OwnerPlayer.Id != player.Id && 
            player.Objects.Any((__) => __.Value.IsVisible(_.Center)))/*.Select((_) => _.Clone())*/); // Add visible objects of other players

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
        
        public GameObject[] Objects { get; set; }

        public void Serialize(Stream str)
        {
            BinaryWriter bw = new BinaryWriter(str);
            bw.Write(PlayerId);
            bw.WriteInts(Resources);
            bw.WriteInts(ResourceLimits);
            bw.Write(EnergyGeneration);
            bw.Write(EnergyConsumation);

            bw.Write(Objects.Length);
            for (int i = 0; i < Objects.Length; i++)
            {
                Objects[i].Serialize(str);
            }
        }
    }
}
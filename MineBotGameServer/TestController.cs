using MineBotGame.GameObjects;
using MineBotGame.PlayerActions;
using System.Linq;
using System.Numerics;

namespace MineBotGame
{
    /// <summary>
    /// Class, that used only in debug tests... Represents locally-controlled <see cref="PlayerController"/>
    /// </summary>
    public class TestController : PlayerController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override PlayerParameters Start(int playerId)
        {
            this.playerId = playerId;
            return new PlayerParameters()
            {
                Nickname = "Mr. Test",
                Motto = "Not a bug, but a feature",
                Color = new Vector3(),
            };
        }

        public override void Stop()
        {
        }

        int playerId;
        int tick = 0;

        bool wasA = false;

        public override void Update(GameStateDelta game)
        {
            var r = PopResults();

            Building[] bases = game.Objects.Where((_) => _.OwnerPlayer.Id == playerId && _ is Building && (_ as Building).Type == BuildingType.Base).Cast<Building>().ToArray();
            if (!wasA)
            {
                PushAction(new PlayerActionOperation(PlayerActionType.StartResearch, 1, bases[0].Id, (int)LocalResearch.TestResearch));
                wasA = true;
            }

            tick++;
        }
    }
}

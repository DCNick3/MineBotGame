using MineBotGame.PlayerActions;
using System.Numerics;

namespace MineBotGame
{
    /// <summary>
    /// Class, that represents the most simple <see cref="PlayerController"/>
    /// </summary>
    public class DummyController : PlayerController
    {
        public override PlayerParameters Start(int playerId)
        {
            return new PlayerParameters() { Color = new Vector3(0f, 0f, 0f), Motto = "Dummy is not stupid!", Nickname = "Dummy" };
        }

        public override void Stop()
        { 
        }

        private int actionId = 0;

        public override void Update(GameStateDelta game)
        {
            PopResults();
            PushAction(new PlayerAction(PlayerActionType.Idle, ++actionId));
        }
    }
}

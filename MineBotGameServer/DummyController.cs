using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using MineBotGame.PlayerActions;

namespace MineBotGame
{
    public class DummyController : PlayerController
    {
        public override PlayerParameters Start()
        {
            return new PlayerParameters() { Color = new Vector3(0f, 0f, 0f), Motto = "Dummy is not stupid!", Nickname = "Dummy" };
        }

        public override void Stop()
        { 
        }

        public override void Update(GameStateDelta game)
        {
            PushAction(new PlayerAction(PlayerActionType.Idle));
        }
    }
}

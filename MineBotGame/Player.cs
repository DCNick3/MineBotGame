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
        int[] resources;
        List<GameObject> ownedObjects;
        PlayerController controller;
        Game game;
    }
}

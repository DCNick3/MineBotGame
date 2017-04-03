using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame
{
    public enum GameResource
    {
        Iron = 1,
        Gold = 2,
        Uranium = 3,
        Sand = 4,
        Water = 5,
        Energium = 6,
    }

    public static class GameResourceHelper
    {
        public static string GetDescription(GameResource res)
        {
            switch (res)
            {
                case GameResource.Iron:
                    return "Frequent and wide-used resource. Requires in almost all operations. Does not need any special equipment for mining and using.";

                default:
                    return "Not implemented description\r\n" + res.ToString();
            }
        }
    }
}

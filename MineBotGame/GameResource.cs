using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame
{
    public static class GameResourceHelper
    {
        public static string GetDescription(ResourceType res)
        {
            switch (res)
            {
                case ResourceType.Iron:
                    return "Frequent and wide-used resource. Requires in almost all operations. Does not need any special equipment for mining and using.";

                default:
                    return "Not implemented description\r\n" + res.ToString();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame
{
    public class PlayerActionResult
    {
        public PlayerActionResult(int actionId, ActionError error)
        {
            ActionId = actionId;
            Error = error;
        }

        public ActionError Error { get; private set; }
        public int ActionId { get; private set; }

        public void Serialize(Stream str)
        {
            BinaryWriter bw = new BinaryWriter(str);

            bw.Write(ActionId);
            bw.Write((int)Error);
        }
    }
}

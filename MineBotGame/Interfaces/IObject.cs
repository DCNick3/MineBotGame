using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame
{
    interface IObject
    {
        int HP { get; }
        int Defence { get; }
        int Id { get; }

        void SelfDestruction();
    }
}

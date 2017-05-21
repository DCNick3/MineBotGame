using System.Numerics;

namespace MineBotGame
{
    public class PlayerParameters
    {
        public string Nickname { get; set; }
        public string Motto { get; set; }
        public Vector3 Color { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Motto, Nickname);
        }
    }
}
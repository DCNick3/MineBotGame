namespace MineBotGame
{
    public struct GameResourceStack
    {
        public GameResourceStack(ResourceType type, int count)
        {
            Type = type;
            Count = count;
        }

        public GameResourceStack(ResourceType type) : this(type, 1)
        { }

        public ResourceType Type { get; set; }
        public int Count { get; set; }

        public bool IsEmpty
        {
            get
            {
                return Type == ResourceType.None || Count == 0;
            }
        }
    }
}
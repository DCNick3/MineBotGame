namespace MineBotGame
{
    /// <summary>
    /// Class, that represents some number of one-type resource
    /// </summary>
    public struct ResourceStack
    {
        public ResourceStack(ResourceType type, int count)
        {
            Type = type;
            Count = count;
        }

        public ResourceStack(ResourceType type) : this(type, 1)
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
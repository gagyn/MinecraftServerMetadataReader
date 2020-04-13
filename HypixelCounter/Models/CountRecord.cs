using HypixelCounter.Integrations;

namespace HypixelCounter.Models
{
    public class CountRecord : Entity
    {
        public int PlayersCount { get; set; }
        public int OnlinePlayers { get; set; }
        public int MaxSlots { get; set; }
    }
}

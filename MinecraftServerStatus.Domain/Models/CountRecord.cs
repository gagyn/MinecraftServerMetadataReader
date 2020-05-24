using MinecraftServerStatus.Integrations.MongoDB;

namespace MinecraftServerStatus.Domain.Models
{
    public class CountRecord : Entity
    {
        public int OnlinePlayers { get; set; }
        public int InQueuePlayers { get; set; }
        public int MaxSlots { get; set; }
        public string? ServerAddress { get; set; }
    }
}

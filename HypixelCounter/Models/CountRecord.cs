﻿using MinecraftServerStatusDomain.Integrations;

namespace MinecraftServerStatusDomain.Models
{
    public class CountRecord : Entity
    {
        public int OnlinePlayers { get; set; }
        public int InQueuePlayers { get; set; }
        public int MaxSlots { get; set; }
    }
}

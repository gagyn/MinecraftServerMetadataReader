using System;

namespace HypixelCounter.Integrations
{
    public class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    }
}

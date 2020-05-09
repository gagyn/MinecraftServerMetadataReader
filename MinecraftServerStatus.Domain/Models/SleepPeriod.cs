using MinecraftServerStatus.Commons;
using MinecraftServerStatus.Integrations.MongoDB;

namespace MinecraftServerStatus.Domain.Models
{
    public class SleepPeriod : Entity
    {
        public Period Period { get; set; }

        public SleepPeriod(Period period)
        {
            Period = period;
        }
    }
}

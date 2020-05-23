using MinecraftServerStatus.Commons;
using MinecraftServerStatus.Integrations.MongoDB;

namespace MinecraftServerStatus.Domain.Models
{
    public class Settings : Entity
    {
        public Period SleepPeriod { get; private set; }
        public bool IsRunning { get; private set; }

        public Settings(Period sleepPeriod, bool isRunning)
        {
            SleepPeriod = sleepPeriod;
            IsRunning = isRunning;
        }
    }
}

using System.Collections.Generic;
using MinecraftServerStatus.Commons;

namespace MinecraftServerStatus.Domain.Models
{
    public class Settings
    {
        public Period SleepPeriod { get; private set; }
        public IEnumerable<string> ServersAddresses { get; private set; }
        public bool IsRunning { get; private set; }

        public Settings(Period sleepPeriod, IEnumerable<string> serversAddresses)
        {
            SleepPeriod = sleepPeriod;
            ServersAddresses = serversAddresses;
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using MinecraftServerStatus.Commons;
using MinecraftServerStatus.Domain.Models;
using MinecraftServerStatus.Integrations.MongoDB;

namespace MinecraftServerStatus.Domain.Services
{
    public class ConfigureSleepPeriodService
    {
        private const Period DEFAULT_PERIOD = Period.Minutes15;
        private readonly SessionFactory _sessionFactory;

        public ConfigureSleepPeriodService(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Period GetSleepPeriod()
        {
            var session = _sessionFactory.Create();
            var sleepPeriod = session.Get<SleepPeriod>().FirstOrDefault();
            return sleepPeriod?.Period ?? DEFAULT_PERIOD;
        }

        public async Task SetSleepPeriod(Period period)
        {
            var session = _sessionFactory.Create();
            await session.ReplaceAsync(new SleepPeriod(period));
        }
    }
}

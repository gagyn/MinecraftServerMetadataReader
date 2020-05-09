using System.Linq;
using System.Threading.Tasks;
using MinecraftServerStatus.Commons;
using MinecraftServerStatus.Domain.Models;
using MinecraftServerStatus.Integrations.MongoDB;

namespace MinecraftServerStatus.Domain.Services
{
    public class ConfigurateSleepPeriodService
    {
        private const Period DEFAULT_PERIOD = Period.Minutes15;
        private readonly SessionFactory _sessionFactory;

        public ConfigurateSleepPeriodService(SessionFactory sessionFactory)
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
            var possibleSleepPeriod = session.Get<SleepPeriod>().FirstOrDefault();
            if (possibleSleepPeriod != null)
            {
                await session.DeleteAsync(possibleSleepPeriod);
            }

            await session.AddAsync(new SleepPeriod(period));
        }
    }
}

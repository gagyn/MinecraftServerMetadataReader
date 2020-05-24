using System.Linq;
using System.Threading.Tasks;
using MinecraftServerStatus.Commons;
using MinecraftServerStatus.Domain.Models;
using MinecraftServerStatus.Integrations.MongoDB;

namespace MinecraftServerStatus.Domain.Services
{
    public class ConfigureSettingsService
    {
        private const Period DEFAULT_PERIOD = Period.Minutes15;
        private readonly SessionFactory _sessionFactory;

        public ConfigureSettingsService(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task InitSettings()
        {
            var session = _sessionFactory.Create();
            var areSettingsExisting = session.Get<Settings>().Any();
            if (!areSettingsExisting)
            {
                await session.AddAsync(new Settings(DEFAULT_PERIOD, isRunning: false));
            }
        }

        public bool ShouldStartAsRunning() => GetSettings().IsRunning;
        public Period GetSleepPeriod() => GetSettings().SleepPeriod;
        public async Task SetAsRunning() => await ChangeRunning(true);
        public async Task SetAsStopped() => await ChangeRunning(false);

        public async Task SetSleepPeriod(Period period)
        {
            var newSettings = new Settings(period, GetSettings().IsRunning);
            var session = _sessionFactory.Create();
            await session.ReplaceByTypeAsync(newSettings);
        }

        private Settings GetSettings()
        {
            var session = _sessionFactory.Create();
            return session.Get<Settings>().First();
        }

        private async Task ChangeRunning(bool isRunning)
        {
            var newSettings = new Settings(GetSettings().SleepPeriod, isRunning);
            var session = _sessionFactory.Create();
            await session.ReplaceByTypeAsync(newSettings);
        }
    }
}

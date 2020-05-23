using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MinecraftServerStatus.Commons;
using MinecraftServerStatus.Domain.Models;
using MinecraftServerStatus.Domain.Services;

namespace MinecraftServerStatus.Controller.Controllers
{
    public class StatisticsCounterController
    {
        public Period SleepPeriod
        {
            get => _configureSettingsService.GetSleepPeriod();
            set => _configureSettingsService.SetSleepPeriod(value).Wait();
        }

        private CancellationTokenSource _cancellationToken;
        private readonly StatusSaverService _statusSaverService;
        private readonly PlayersCounterService _playersCounterService;
        private readonly ConfigureSettingsService _configureSettingsService;
        private readonly List<string> _serverAddresses;

        public StatisticsCounterController(StatusSaverService statusSaverService, PlayersCounterService playersCounterService, ConfigureSettingsService configureSettingsService)
        {
            _statusSaverService = statusSaverService;
            _playersCounterService = playersCounterService;
            _configureSettingsService = configureSettingsService;
            _serverAddresses = new List<string>();
        }

        public void AddServer(string serverAddress) => _serverAddresses.Add(serverAddress);
        public void RemoveServer(string serverAddress) => _serverAddresses.Remove(serverAddress);
        public IList<string> GetServers => _serverAddresses;

        public async Task Run()
        {
            var isNowRunning = _cancellationToken != null && !_cancellationToken.IsCancellationRequested;
            if (isNowRunning)
            {
                return;
            }
            _cancellationToken = new CancellationTokenSource();
            await RunUntilCanceledAsync();
        }

        public void Stop()
        {
            _cancellationToken?.Cancel();
        }

        private async Task RunUntilCanceledAsync()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var sleepTime = GetSleepTime(SleepPeriod);
                await Task.Delay(sleepTime, _cancellationToken.Token);

                var countRecords = GetCountRecords();
                countRecords.ToList().ForEach(async x => await _statusSaverService.Write(x));
                Console.WriteLine($"Wrote at {DateTime.Now}");
            }
        }

        private IEnumerable<CountRecord> GetCountRecords()
        {
            var countRecords = _serverAddresses.Select(x =>
            {
                var isSuccess = _playersCounterService.TryToGetCounts(x, out var record);
                return (isSuccess, record);
            });
            return countRecords.Where(x => x.isSuccess).Select(x => x.record);
        }

        private TimeSpan GetSleepTime(Period sleepPeriod)
        {
            var now = DateTime.Now;
            var next = now.Date;
            next = next.AddHours(now.Hour);
            var howManyPeriodsPassInThisHour = now.Minute / (int)sleepPeriod;
            next = next.AddMinutes((howManyPeriodsPassInThisHour + 1) * (int)sleepPeriod);
            return next - now;
        }
    }
}

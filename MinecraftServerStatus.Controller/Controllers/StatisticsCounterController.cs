using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MinecraftServerStatus.Commons;
using MinecraftServerStatus.Domain.Models;
using MinecraftServerStatus.Domain.Services;
using MongoDB.Driver;

namespace MinecraftServerStatus.Controller.Controllers
{
    public class StatisticsCounterController
    {
        public Period SleepPeriod
        {
            get => _configureSettingsService.GetSleepPeriod();
            set => _configureSettingsService.SetSleepPeriod(value).Wait();
        }

        private CancellationTokenSource _cancellationTokenSource;
        private readonly StatusSaverService _statusSaverService;
        private readonly PlayersCounterService _playersCounterService;
        private readonly ConfigureSettingsService _configureSettingsService;
        private readonly ScannedServersService _scannedServersService;

        public StatisticsCounterController(StatusSaverService statusSaverService, PlayersCounterService playersCounterService, ConfigureSettingsService configureSettingsService, ScannedServersService scannedServersService)
        {
            _statusSaverService = statusSaverService;
            _playersCounterService = playersCounterService;
            _configureSettingsService = configureSettingsService;
            _scannedServersService = scannedServersService;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSource.Cancel();
            configureSettingsService.InitSettings().Wait();
        }

        public bool ShouldStartAsRunning() => _configureSettingsService.ShouldStartAsRunning();
        public async Task AddServer(string serverAddress) => await _scannedServersService.AddServer(serverAddress);
        public async Task RemoveServer(string serverAddress) => await _scannedServersService.RemoveServerAsync(serverAddress);
        public IList<string> GetServers() => _scannedServersService.Servers;
        public bool IsNowRunning() => !_cancellationTokenSource.IsCancellationRequested;

        public async Task Run()
        {
            if (IsNowRunning())
            {
                return;
            }
            _cancellationTokenSource = new CancellationTokenSource();
            await _configureSettingsService.SetAsRunning();
            _ = RunUntilCanceledAsync();
        }

        public async Task Stop()
        {
            await _configureSettingsService.SetAsStopped();
            _cancellationTokenSource.Cancel();
        }

        private async Task RunUntilCanceledAsync()
        {
            while (_cancellationTokenSource!.IsCancellationRequested == false)
            {
                var sleepTime = GetSleepTime(SleepPeriod);
                await Task.Delay(sleepTime, _cancellationTokenSource.Token);

                var countRecords = GetCountRecords();
                await countRecords.ForEachAsync(async x => await _statusSaverService.Write(x));
                Console.WriteLine($"Wrote at {DateTime.Now}");
            }
        }

        private async IAsyncEnumerable<CountRecord> GetCountRecords()
        {
            var tasks = _scannedServersService.Servers
                .Select(x => _playersCounterService.TryToGetCountRecord(x))
                .ToList();

            while (tasks.Count > 0)
            {
                var firstFinished = await Task.WhenAny(tasks);
                tasks.Remove(firstFinished);

                var countRecord = await firstFinished;
                if (countRecord != null)
                {
                    yield return countRecord;
                }
            }
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

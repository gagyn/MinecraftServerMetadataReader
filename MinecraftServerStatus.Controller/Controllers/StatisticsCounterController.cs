using System;
using System.Threading;
using System.Threading.Tasks;
using MinecraftServerStatus.Commons;
using MinecraftServerStatus.Domain.Services;

namespace MinecraftServerStatus.Controller.Controllers
{
    public class StatisticsCounterController
    {
        public Period SleepPeriod
        {
            get => _configurateSleepPeriodService.GetSleepPeriod();
            set => _configurateSleepPeriodService.SetSleepPeriod(value).Wait();
        }

        private CancellationTokenSource _cancellationToken;
        private readonly StatusWriterToBaseService _statusWriterToBaseService;
        private readonly ServerPlayersCounterService _serverPlayersCounterService;
        private readonly ConfigurateSleepPeriodService _configurateSleepPeriodService;

        public StatisticsCounterController(StatusWriterToBaseService statusWriterToBaseService, ServerPlayersCounterService serverPlayersCounterService, ConfigurateSleepPeriodService configurateSleepPeriodService)
        {
            _statusWriterToBaseService = statusWriterToBaseService;
            _serverPlayersCounterService = serverPlayersCounterService;
            _configurateSleepPeriodService = configurateSleepPeriodService;
        }

        public async Task Run()
        {
            var isNowRunning = _cancellationToken != null && !_cancellationToken.IsCancellationRequested;
            if (isNowRunning)
            {
                return;
            }

            _cancellationToken = new CancellationTokenSource();
            while (!_cancellationToken.IsCancellationRequested)
            {
                var sleepTime = GetSleepTime(SleepPeriod);
                await Task.Delay(sleepTime, _cancellationToken.Token);

                var triesLeft = 5;
                while (triesLeft > 0)
                {
                    var (onlinePlayers, slots, inQueue) = GetCounts();
                    if (onlinePlayers == 0)
                    {
                        triesLeft--;
                        var delay = 10000 * (1.0 / triesLeft);
                        await Task.Delay((int)delay);
                        continue;
                    }

                    await _statusWriterToBaseService.WriteToBase(onlinePlayers, inQueue, slots);
                    Console.WriteLine($"{DateTime.Now}: {onlinePlayers} in queue: {inQueue} slots: {slots}");
                    break;
                }
            }
        }

        public void Stop()
        {
            _cancellationToken?.Cancel();
        }

        private TimeSpan GetSleepTime(Period sleepPeriod)
        {
            var now = DateTime.Now;
            var next = now.Date;
            next = next.AddHours(now.Hour);
            var howManyPeriodsPassInThisHour = now.Minute / (int) sleepPeriod;
            next = next.AddMinutes((howManyPeriodsPassInThisHour + 1) * (int) sleepPeriod);
            return next - now;
        }

        private (int onlinePlayers, int slots, int inQueue) GetCounts()
        {
            var (onlinePlayers, slots) = _serverPlayersCounterService.GetRealCount();
            var inQueue = onlinePlayers - slots;
            if (inQueue < 0)
            {
                inQueue = 0;
            }
            return (onlinePlayers, slots, inQueue);
        }
    }
}

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
            get => _configureSleepPeriodService.GetSleepPeriod();
            set => _configureSleepPeriodService.SetSleepPeriod(value).Wait();
        }

        private CancellationTokenSource _cancellationToken;
        private readonly StatusWriterToBaseService _statusWriterToBaseService;
        private readonly ServerPlayersCounterService _serverPlayersCounterService;
        private readonly ConfigureSleepPeriodService _configureSleepPeriodService;

        public StatisticsCounterController(StatusWriterToBaseService statusWriterToBaseService, ServerPlayersCounterService serverPlayersCounterService, ConfigureSleepPeriodService configureSleepPeriodService)
        {
            _statusWriterToBaseService = statusWriterToBaseService;
            _serverPlayersCounterService = serverPlayersCounterService;
            _configureSleepPeriodService = configureSleepPeriodService;
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
                    var success = TryToGetCounts(out var onlinePlayers, out var slots, out var inQueue);
                    if (!success)
                    {
                        triesLeft--;
                        await Task.Delay(2000);
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

        private bool TryToGetCounts(out int onlinePlayers, out int slots, out int inQueue)
        {
            try
            {
                (onlinePlayers, slots, inQueue) = GetCounts();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                (onlinePlayers, slots, inQueue) = (-1, -1, -1);
                return false;
            }
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

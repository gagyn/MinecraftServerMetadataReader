using System;
using System.Threading.Tasks;
using HypixelCounter.Services;
using HypixelCounterServer.Common;
using HypixelCounterServer.Service;

namespace HypixelCounterServer.Controllers
{
    public class StatisticsCounterController
    {
        private readonly StatusWriterToBaseService _statusWriterToBaseService;
        private readonly ServerPlayersCounterService _serverPlayersCounterService;
        private readonly NotificationService _notificationService;

        public StatisticsCounterController(StatusWriterToBaseService statusWriterToBaseService, ServerPlayersCounterService serverPlayersCounterService, NotificationService notificationService)
        {
            _statusWriterToBaseService = statusWriterToBaseService;
            _serverPlayersCounterService = serverPlayersCounterService;
            _notificationService = notificationService;
        }

        public async Task Run()
        {
            while (true)
            {
                var sleepTime = GetSleepTime(Period.Minutes15);
                await Task.Delay(sleepTime);

                var (onlinePlayers, slots) = _serverPlayersCounterService.GetRealCount();
                var inQueue = onlinePlayers - slots;
                if (inQueue < 0)
                {
                    inQueue = 0;
                }

                await _statusWriterToBaseService.WriteToBase(onlinePlayers, inQueue, slots);

                if (DateTime.Now.Minute == 0 && DateTime.Now.Hour > 11)
                {
                    //await _notificationService.SendMail(onlinePlayers, inQueue, slots);
                }

                Console.WriteLine($"{DateTime.Now}: {onlinePlayers} in queue: {inQueue} slots: {slots}");
            }
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
    }
}

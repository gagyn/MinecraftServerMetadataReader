using System;
using System.Threading.Tasks;
using HypixelCounter.Services;
using HypixelCounterServer.Service;

namespace HypixelCounter
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
            const int sleepTime = 1000 * 60 * 15;
            while (true)
            {
                var count = _serverPlayersCounterService.GetCount();
                var (onlinePlayers, slots) = _serverPlayersCounterService.GetRealCount();
                if (count != -1)
                {
                    await _statusWriterToBaseService.WriteToBase(count, onlinePlayers, slots);
                    await _notificationService.SendMail(count, onlinePlayers, slots);
                }

                Console.WriteLine($"{DateTime.Now}: {count}");
                await Task.Delay(sleepTime);
            }
        }
    }
}

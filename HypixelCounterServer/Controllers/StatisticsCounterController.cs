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
            const int sleepTime = 1000 * 60 * 30;
            while (true)
            {
                var (onlinePlayers, slots) = _serverPlayersCounterService.GetRealCount();
                var inQueue = onlinePlayers - slots;
                if (inQueue < 0)
                {
                    inQueue = 0;
                }

                await _statusWriterToBaseService.WriteToBase(onlinePlayers, inQueue, slots);
                await _notificationService.SendMail(onlinePlayers, inQueue, slots);

                Console.WriteLine($"{DateTime.Now}: {onlinePlayers} in queue: {inQueue} slots: {slots}");
                await Task.Delay(sleepTime);
            }
        }
    }
}

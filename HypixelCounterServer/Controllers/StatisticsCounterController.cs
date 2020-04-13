using System.Threading.Tasks;
using HypixelCounter.Services;

namespace HypixelCounter
{
    public class StatisticsCounterController
    {
        private readonly StatusWriterToBaseService _statusWriterToBaseService;
        private readonly ServerPlayersCounterService _serverPlayersCounterService;

        public StatisticsCounterController(StatusWriterToBaseService statusWriterToBaseService, ServerPlayersCounterService serverPlayersCounterService)
        {
            _statusWriterToBaseService = statusWriterToBaseService;
            _serverPlayersCounterService = serverPlayersCounterService;
        }

        public async Task Run()
        {
            const int sleepTime = 1000 * 60 * 15;
            while (true)
            {
                var count = _serverPlayersCounterService.GetCount();
                if (count != -1)
                {
                    await _statusWriterToBaseService.WriteToBase(count);
                }
                await Task.Delay(sleepTime);
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MinecraftServerStatus.API.Models;
using MinecraftServerStatus.Commons;
using MinecraftServerStatus.Controller.Controllers;

namespace MinecraftServerStatus.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsCounterController _statisticsCounterController;

        public StatisticsController(StatisticsCounterController statisticsCounterController)
        {
            _statisticsCounterController = statisticsCounterController;
        }

        [HttpGet]
        public void StartSavingCounts()
        {
            _ = _statisticsCounterController.Run();
        }

        [HttpGet]
        public void StopSavingCounts()
        {
            _ = _statisticsCounterController.Stop();
        }

        [HttpPost]
        public async Task SetSleepPeriod([FromBody] SetSleepPeriodRequest request)
        {
            var minutes = request.Minutes;
            if (!Enum.IsDefined(typeof(Period), minutes))
            {
                throw new ArgumentException("The value is not correct for Period");
            }

            var period = (Period)minutes;
            await _statisticsCounterController.Stop();
            _statisticsCounterController.SleepPeriod = period;
            await _statisticsCounterController.Run();
        }

        [HttpGet]
        public GetSleepPeriodResponse GetSleepPeriod()
        {
            var period = _statisticsCounterController.SleepPeriod;
            return new GetSleepPeriodResponse { SleepPeriod = period };
        }

        [HttpGet]
        public GetScannedServersResponse GetScannedServers()
        {
            return new GetScannedServersResponse
            {
                ServersAddresses = _statisticsCounterController.GetServers()
            };
        }

        [HttpPost]
        public async Task AddNewServerAsync([FromBody] AddNewServerRequest request)
        {
            await _statisticsCounterController.AddServer(request.ServerAddress);
        }
        
        [HttpPost]
        public async Task RemoveServer([FromBody] RemoveServerRequest request)
        {
            await _statisticsCounterController.RemoveServer(request.ServerAddress);
        }

        
    }
}

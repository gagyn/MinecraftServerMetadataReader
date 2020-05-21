using System;
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
            _statisticsCounterController.Stop();
        }

        [HttpPost]
        public void SetSleepPeriod([FromBody] SetSleepPeriodRequest request)
        {
            var minutes = request.Minutes;
            if (!Enum.IsDefined(typeof(Period), minutes))
            {
                throw new ArgumentException("The value is not correct for Period");
            }

            var period = (Period)minutes;
            _statisticsCounterController.Stop();
            _statisticsCounterController.SleepPeriod = period;
            _ = _statisticsCounterController.Run();
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
            throw new NotImplementedException();
        }

        [HttpPost]
        public void AddNewServer([FromBody] AddNewServerRequest request)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public void RemoveServer([FromBody] RemoveServerRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

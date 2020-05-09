using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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


    }
}

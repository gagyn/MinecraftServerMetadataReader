using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using HypixelCounter;
using HypixelCounter.Services;
using Newtonsoft.Json;

namespace HypixelCounterServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string configPath = "config.json";
            var configuration = JsonConvert.DeserializeObject<MongoConfiguration>(File.ReadAllText(configPath));
            var container = new AutofacServerBuilder().Build(configuration);

            var controller = container.Resolve<StatisticsCounterController>();
            await controller.Run();
        }
    }
}

using System.IO;
using System.Threading.Tasks;
using Autofac;
using MinecraftServerStatusDomain;
using MinecraftServerStatusHandler.Controllers;
using MinecraftServerStatusHandler.Services;
using Newtonsoft.Json;

namespace MinecraftServerStatusHandler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string configPath = "config.json";
            var configuration = JsonConvert.DeserializeObject<AppConfiguration>(File.ReadAllText(configPath));
            var container = new AutofacServerBuilder().Build(configuration);

            var controller = container.Resolve<StatisticsCounterController>();
            await controller.Run();
        }
    }
}

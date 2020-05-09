using System.IO;
using System.Threading.Tasks;
using Autofac;
using MinecraftServerStatus.Controller.Controllers;
using MinecraftServerStatus.Controller.Services;
using MinecraftServerStatus.Domain;
using Newtonsoft.Json;

namespace MinecraftServerStatus.Controller
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string configPath = "config.json";
            var configuration = JsonConvert.DeserializeObject<AppConfiguration>(File.ReadAllText(configPath));
            var container = new AutofacControllerBuilder().Build(configuration);

            var controller = container.Resolve<StatisticsCounterController>();
            await controller.Run();
        }
    }
}

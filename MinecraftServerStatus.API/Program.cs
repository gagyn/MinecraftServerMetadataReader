using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinecraftServerStatus.Controller.Controllers;
using MinecraftServerStatus.IoC;

namespace MinecraftServerStatus.API
{
    public class Program
    {
        private static readonly IConfiguration _configuration;

        static Program()
        {
            _configuration = GetConfiguration();
        }

        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            using (var scope = webHost.Services.CreateScope())
            {
                var controller = scope.ServiceProvider.GetRequiredService<StatisticsCounterController>();
                if (controller.ShouldStartAsRunning())
                {
                    await controller.Run();
                }
            }
            await webHost.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(ConfigureAutofac))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls(_configuration["Url"]);
                });

        private static void ConfigureAutofac(ContainerBuilder builder)
        {
            var assemblies = new AssembliesFinder().GetAllReferencedAssemblies(Assembly.GetEntryAssembly(), nameof(MinecraftServerStatus));
            var solutionAutofacBuilder = new AutofacBuilder(builder).BuildBasicTypes(_configuration);
            assemblies.ForEach(x =>
            {
                solutionAutofacBuilder
                    .BuildAdditionalControllers(x)
                    .BuildAdditionalServices(x);
            });
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
#if RELEASE
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#else
                .AddJsonFile("appsettings.Development.json", optional: true)
#endif
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinecraftServerStatus.IoC;
using Newtonsoft.Json;

namespace MinecraftServerStatus.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(ConfigureAutofac))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls("https://192.168.0.100:5001");
                });

        private static void ConfigureAutofac(ContainerBuilder builder)
        {
            var configuration = GetConfiguration();
            var assemblies = new AssembliesFinder().GetAllReferencedAssemblies(Assembly.GetEntryAssembly(), nameof(MinecraftServerStatus));
            var solutionAutofacBuilder = new AutofacBuilder(builder).BuildBasicTypes(configuration);
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

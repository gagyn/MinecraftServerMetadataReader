using System.Reflection;
using Autofac;
using MinecraftServerStatusDomain.Integrations;
using MongoDB.Driver;

namespace MinecraftServerStatusDomain.Services
{
    public class AutofacBuilder
    {
        private readonly ContainerBuilder _builder;

        public AutofacBuilder()
        {
            this._builder = new ContainerBuilder();
        }

        public void BuildCounterServices(AppConfiguration configuration)
        {
            RegisterAppConfiguration(configuration);
            RegisterMongoDatabase(configuration);
            RegisterServices(Assembly.GetExecutingAssembly());
            RegisterControllers(Assembly.GetExecutingAssembly());
        }

        public void BuildAdditionalServices(Assembly assembly)
        {
            RegisterServices(assembly);
        }

        public void BuildAdditionalControllers(Assembly assembly)
        {
            RegisterControllers(assembly);
        }

        public IContainer GetContainer()
        {
            return _builder.Build();
        }
        private void RegisterAppConfiguration(AppConfiguration configuration)
        {
            _builder.RegisterInstance(configuration)
                .SingleInstance()
                .As<AppConfiguration>();
        }

        private void RegisterMongoDatabase(AppConfiguration configuration)
        {
            var database = new MongoClient(configuration.MongoConnectionString).GetDatabase("hypixelCounter");
            var sessionFactory = new SessionFactory(database);
            _builder.RegisterInstance(sessionFactory)
                .SingleInstance()
                .As<SessionFactory>();
        }

        private void RegisterServices(Assembly assembly)
        {
            _builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.Name.EndsWith("Service"))
                .PreserveExistingDefaults()
                .InstancePerLifetimeScope();
        }
        
        private void RegisterControllers(Assembly assembly)
        {
            _builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.Name.EndsWith("Controller"))
                .PreserveExistingDefaults()
                .InstancePerLifetimeScope();
        }
    }
}

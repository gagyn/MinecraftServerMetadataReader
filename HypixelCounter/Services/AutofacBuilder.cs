using System.Reflection;
using Autofac;
using MongoDB.Driver;

namespace HypixelCounter.Services
{
    public class AutofacBuilder
    {
        private readonly ContainerBuilder _builder;

        public AutofacBuilder()
        {
            this._builder = new ContainerBuilder();
        }

        public void BuildCounterServices(MongoConfiguration configuration)
        {
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

        private void RegisterMongoDatabase(MongoConfiguration configuration)
        {
            var database = new MongoClient(configuration.MongoConnectionString);
            _builder.RegisterInstance(database)
                .As<IMongoDatabase>()
                .InstancePerLifetimeScope();
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

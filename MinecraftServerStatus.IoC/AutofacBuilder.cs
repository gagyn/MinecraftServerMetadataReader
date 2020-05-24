using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using MinecraftServerStatus.Integrations.MongoDB;
using MongoDB.Driver;

namespace MinecraftServerStatus.IoC
{
    public class AutofacBuilder
    {
        private readonly ContainerBuilder _builder;

        public AutofacBuilder()
        {
            this._builder = new ContainerBuilder();
        }

        public AutofacBuilder(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public AutofacBuilder BuildBasicTypes(IConfiguration configuration)
        {
            RegisterMongoDatabase(configuration);
            return this;
        }

        public AutofacBuilder BuildAdditionalServices(Assembly assembly)
        {
            RegisterServices(assembly);
            return this;
        }

        public AutofacBuilder BuildAdditionalControllers(Assembly assembly)
        {
            RegisterControllers(assembly);
            return this;
        }

        public IContainer GetContainer()
        {
            return _builder.Build();
        }

        private void RegisterMongoDatabase(IConfiguration configuration)
        {
            var database = new MongoClient(configuration["ConnectionStrings:Mongo"]).GetDatabase("McServersCounter"); //todo: rename db
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
                .InstancePerLifetimeScope()
                .SingleInstance();
        }
        
        private void RegisterControllers(Assembly assembly)
        {
            _builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.Name.EndsWith("Controller"))
                .PreserveExistingDefaults()
                .InstancePerLifetimeScope()
                .SingleInstance();
        }
    }
}

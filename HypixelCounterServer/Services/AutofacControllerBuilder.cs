using System.Reflection;
using Autofac;
using MinecraftServerStatus.Domain;
using MinecraftServerStatus.Domain.Services;

namespace MinecraftServerStatus.Controller.Services
{
    public class AutofacControllerBuilder
    {
        private readonly AutofacBuilder _builder;

        public AutofacControllerBuilder()
        {
            this._builder = new AutofacBuilder();
        }

        public IContainer Build(AppConfiguration appConfiguration)
        {
            _builder.BuildSolutionServices(appConfiguration);
            var thisAssembly = Assembly.GetExecutingAssembly();
            _builder.BuildAdditionalControllers(thisAssembly);
            _builder.BuildAdditionalServices(thisAssembly);
            return _builder.GetContainer();
        }
    }
}

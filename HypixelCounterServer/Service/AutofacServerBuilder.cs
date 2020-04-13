using System.Reflection;
using Autofac;
using HypixelCounter.Services;

namespace HypixelCounterServer.Service
{
    public class AutofacServerBuilder
    {
        private readonly AutofacBuilder _builder;

        public AutofacServerBuilder()
        {
            this._builder = new AutofacBuilder();
        }

        public IContainer Build(AppConfiguration appConfiguration)
        {
            _builder.BuildCounterServices(appConfiguration);
            var thisAssembly = Assembly.GetExecutingAssembly();
            _builder.BuildAdditionalControllers(thisAssembly);
            _builder.BuildAdditionalServices(thisAssembly);
            return _builder.GetContainer();
        }
    }
}

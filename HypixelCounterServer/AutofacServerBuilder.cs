using System.Reflection;
using Autofac;
using HypixelCounter.Services;

namespace HypixelCounterServer
{
    public class AutofacServerBuilder
    {
        private readonly AutofacBuilder _builder;

        public AutofacServerBuilder()
        {
            this._builder = new AutofacBuilder();
        }

        public IContainer Build(MongoConfiguration mongoConfiguration)
        {
            _builder.BuildCounterServices(mongoConfiguration);
            var thisAssembly = Assembly.GetExecutingAssembly();
            _builder.BuildAdditionalControllers(thisAssembly);
            _builder.BuildAdditionalServices(thisAssembly);
            return _builder.GetContainer();
        }
    }
}

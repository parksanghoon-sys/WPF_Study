using Custom.Wpf.Global.Event;
using Custom.Wpf.Global.Location;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Collections.Generic;
using Unity;

namespace Custom.Wpf.Global.Controls
{
    public abstract class CustomApplication : PrismApplication
    {
        private List<IModule> _modules = new List<IModule>();
        public CustomApplication AddInversionModule<T>() where T : IModule, new()
        {
            IModule module = new T();
            _modules.Add(module);

            return this;
        }
        public CustomApplication AddWireDataContext<T>() where T : ViewModelLocationScenario, new()
        {
            ViewModelLocationScenario scenario = new T();
            _ = scenario.Publish();

            return this;
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            foreach (var model in _modules)
                moduleCatalog.AddModule(model.GetType());
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(typeof(IEventHub), EventAggregatorBuilder.Get(containerRegistry.GetContainer().Resolve<IEventAggregator>()));
        }
        public static T GetService<T>()
        {
            if (CustomApplication.Current is CustomApplication app)
                return app.Container.Resolve<T>();
            return default(T);
        }
    }
}

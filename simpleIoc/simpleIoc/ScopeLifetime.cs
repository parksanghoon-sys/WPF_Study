using simpleIoc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleIoc
{
    public class ScopeLifetime : ObjectCache, ILifetime
    {
        private readonly ContainerLifetime _parentLifetime;
        public ScopeLifetime(ContainerLifetime containerLifetime)
        {
            _parentLifetime = containerLifetime;
        }
        public object? GetService(Type serviceType)
        {
            return _parentLifetime.GetFactory(serviceType)(this);
        }

        public object GetServiceAsSingleTon(Type type, Func<ILifetime, object> factory)
        {
            return _parentLifetime.GetServiceAsSingleTon(type, factory);
        }

        public object GetServicePerScope(Type type, Func<ILifetime, object> factory)
        {
            return GetCached(type, factory, this);
        }
    }
}

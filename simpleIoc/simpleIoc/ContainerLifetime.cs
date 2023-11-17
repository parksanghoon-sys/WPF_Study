using simpleIoc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleIoc
{
    public class ContainerLifetime : ObjectCache, ILifetime
    {
        public Func<Type, Func<ILifetime, object>> GetFactory { get; private set; }
        public ContainerLifetime(Func<Type, Func<ILifetime, object>> factory)
        {
            GetFactory = factory;
        }
        public object? GetService(Type serviceType)
        {
            return GetFactory(serviceType)(this);
        }

        public object GetServiceAsSingleTon(Type typpe, Func<ILifetime, object> factory)
        {
            return GetCached(typpe, factory, this);
        }

        public object GetServicePerScope(Type typpe, Func<ILifetime, object> factory)
        {
            return GetServiceAsSingleTon(typpe, factory);
        }
    }
}

using simpleIoc.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace simpleIoc
{
    public abstract class ObjectCache
    {
        private readonly ConcurrentDictionary<Type, object> _instanceCache = new();
        protected object GetCached(Type type, Func<ILifetime,object> factory, ILifetime lifetime)
            => _instanceCache.GetOrAdd(type, _=>factory(lifetime));
        public void Dispose()
        {
            foreach (var obj in _instanceCache.Values)
                (obj as IDisposable)?.Dispose();
        }
        
    }
}

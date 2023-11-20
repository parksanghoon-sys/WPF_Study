using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfOptionPatton.Services
{
    internal class Factory<T> : IFactory<T> where T : class
    {
        private readonly static ObjectFactory _objectFactory;
        private readonly IServiceProvider _serviceProvider;
        static Factory()
        {
            _objectFactory = ActivatorUtilities.CreateFactory(typeof(T), Array.Empty<Type>());
        }
        public Factory(IServiceProvider service)
        {
            _serviceProvider = service;
        }
        public T New()
        {
            return (T)_objectFactory(_serviceProvider, Array.Empty<object>());
        }
    }
}

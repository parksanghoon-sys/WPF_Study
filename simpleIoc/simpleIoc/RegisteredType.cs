using simpleIoc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleIoc
{
    public class RegisteredType : IRegisteredType
    {
        private readonly Type _type;
        private readonly Action<Func<ILifetime, object>> _registerFactory;
        private readonly Func<ILifetime, object> _factory;

        public RegisteredType(Type type, Action<Func<ILifetime, object>> registerFactory, Func<ILifetime, object> factory)
        {
            _type = type;
            _registerFactory = registerFactory;
            _factory = factory;

            registerFactory(_factory);
        }

        public void AsSingleton()
        {
            _registerFactory(lifetime => lifetime.GetServiceAsSingleTon(_type, _factory));
        }

        public void PerScope()
        {
            _registerFactory(lifetime => lifetime.GetServicePerScope(_type, _factory));
        }
    }
}
using simpleIoc.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace simpleIoc
{
    public class Container : IScope
    {
        private readonly Dictionary<Type,Func<ILifetime,object>> _registerTypes = new();
        private readonly ContainerLifetime? _lifetime;

        public Container()
        {
            _lifetime = new ContainerLifetime(t => _registerTypes[t]);
        }
        public IRegisteredType Registering(Type @interface, Func<object> factory)
            => RegisterType(@interface, _ => factory());

        public IRegisteredType Registering(Type itemType, Type implementation)
        {
            return RegisterType(itemType, FactoryFromType(implementation));
        }        
        private IRegisteredType RegisterType(Type itemType, Func<ILifetime, object> factory)
             => new RegisteredType(itemType, f => _registerTypes[itemType] = f, factory);

        public object GetService(Type type)
        {
            Func<ILifetime, object> registeredType;
            if (!_registerTypes.TryGetValue(type, out registeredType))
                return null;
            return registeredType(_lifetime);
        }
        public IScope CreateScope()
        {
            return new ScopeLifetime(_lifetime);
        }
        private static Func<ILifetime, object> FactoryFromType(Type itemType)
        {
            var constructors = itemType.GetConstructors();
            if (constructors.Length == 0)
                constructors = itemType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            var constructor = constructors.First();

            var arg = Expression.Parameter(typeof(ILifetime));

            return (Func<ILifetime, object>)Expression.Lambda(
           Expression.New(constructor, constructor.GetParameters().Select(
               param =>
               {
                   var resolve = new Func<ILifetime, object>(
                       lifetime => lifetime.GetService(param.ParameterType));
                   return Expression.Convert(
                       Expression.Call(Expression.Constant(resolve.Target), resolve.Method, arg),
                       param.ParameterType);
               })),
           arg).Compile();
        }

        public void Dispose()
        {
            _lifetime.Dispose();
        }
       
    }
}
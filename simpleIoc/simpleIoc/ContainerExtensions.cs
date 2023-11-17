using simpleIoc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleIoc
{
    public static class ContainerExtensions
    {

        public static IRegisteredType Register<T>(this Container container, Type type)
            => container.Registering(typeof(T), type);


        public static IRegisteredType Register<TInterface, TImplementation>(this Container container)
            where TImplementation : TInterface
            => container.Registering(typeof(TInterface), typeof(TImplementation));


        public static IRegisteredType Register<T>(this Container container, Func<T> factory)
            => container.Registering(typeof(T), () => factory());


        public static IRegisteredType Register<T>(this Container container)
            => container.Registering(typeof(T), typeof(T));


        public static T Resolve<T>(this IScope scope) => (T)scope.GetService(typeof(T));
    }
}

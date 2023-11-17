using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleIoc.Interfaces
{
    public interface ILifetime : IScope
    {
        object GetServiceAsSingleTon(Type typpe, Func<ILifetime, object> factory);
        object GetServicePerScope(Type typpe, Func<ILifetime, object> factory);
    }
}

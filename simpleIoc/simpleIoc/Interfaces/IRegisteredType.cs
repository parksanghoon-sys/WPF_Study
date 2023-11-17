using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleIoc.Interfaces
{
    public interface IRegisteredType
    {
        void AsSingleton();
        void PerScope();
    }
}

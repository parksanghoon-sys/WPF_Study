using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfOptionPatton.Services
{
    internal interface IFactory<T> where T : class
    {
        T New();
    }
}

using Log.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclularGage.Main.Common
{

    internal interface IServiceTest
    {
        void Test();
    }
    internal class ServiceTest : IServiceTest
    {
        public void Test()
        {
            //ExTrace.Print("Test");
        }
    }
}

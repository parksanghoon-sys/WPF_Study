using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfDINavigation.Servieces
{
    public interface INavigationService
    {
        void Navigate(NaviType naviType);
    }
}

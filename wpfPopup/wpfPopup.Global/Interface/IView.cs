using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfPopup.Local.Interface
{
    public interface IView
    {
    }
    public interface IMainView : IView
    {
        public bool? ShowPopupWindow();
    }
}

using System.Windows;
using System.Windows.Controls;

namespace wpfControls.Utilites
{
    public static class UIElementUtils
    {
        public static void BringToFront(this UIElement uIElement)
        {
            Panel.SetZIndex(uIElement, int.MaxValue);
        }
        public static void SendToBack(this UIElement uIElement)
        {
            Panel.SetZIndex(uIElement, int.MinValue);
        }
    }
}

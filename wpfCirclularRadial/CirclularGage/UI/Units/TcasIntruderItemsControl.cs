using System.Windows;
using System.Windows.Controls;

namespace CirclularGage.Location.UI.Units

{
    public class TcasIntruderItemsControl :  ItemsControl
    {
        static TcasIntruderItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TcasIntruderItemsControl), new FrameworkPropertyMetadata(typeof(TcasIntruderItemsControl)));
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TcasIntruderItem();
        }      
    }   
}

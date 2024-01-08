using System.Windows;
using System.Windows.Controls;

namespace CirclularGage.Location.UI.Units
{
    public class TcasIntruderSettingListBox : ListBox
    {
        static TcasIntruderSettingListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TcasIntruderSettingListBox), new FrameworkPropertyMetadata(typeof(TcasIntruderSettingListBox)));
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TcasIntruderSettingListItem();
        }
    }
}

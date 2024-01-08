using System.Windows;
using System.Windows.Controls;

namespace CirclularGage.Location.UI.Units
{
    public class TcasIntruderSettingListItem : ListBoxItem
    {
        static TcasIntruderSettingListItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TcasIntruderSettingListItem), new FrameworkPropertyMetadata(typeof(TcasIntruderSettingListItem)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CirclularGage.Location.UI.Units

{
    public class TcasIntruderItemsControl :  ListView
    {
        static TcasIntruderItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TcasIntruderItemsControl), new FrameworkPropertyMetadata(typeof(TcasIntruderItemsControl)));
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TcasIntruderItem();
        }
        //    protected override bool IsItemItsOwnContainerOverride(object item)
        //    {
        //        return item is CustomRectContentPresenter;
        //    }
        //}

        //public class CustomRectContentPresenter : ContentPresenter
        //{
        //    public CustomRectContentPresenter()
        //    {
        //        ContentTemplateSelector = new CustomRectTemplateSelector();
        //    }
        //}

        //public class CustomRectTemplateSelector : DataTemplateSelector
        //{
        //    public override DataTemplate SelectTemplate(object item, DependencyObject container)
        //    {
        //        return null;
        //    }
        //}
    }

    internal class TcasIntruderItem : ListViewItem
    {
        static TcasIntruderItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TcasIntruderItem), new FrameworkPropertyMetadata(typeof(TcasIntruderItem)));
        }
    }
}

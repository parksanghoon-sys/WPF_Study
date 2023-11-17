using Custom.Wpf.Global.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace wpfVerticalTabControl.UI.Units
{
    public class TabContent : ListBox
    {
        static TabContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabContent), new FrameworkPropertyMetadata(typeof(TabContent)));
        }
        private AnimationScrollViewerExtention contentScroll;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            contentScroll = GetTemplateChild("PART_ContentScroll") as AnimationScrollViewerExtention;
        }

        public void ModeScroll(double targetVerticalOffset)
        {
            Storyboard sb = new();
            ValueItem aniItem = new();
            aniItem.TargetName = contentScroll.Name;
            aniItem.Property = new PropertyPath(AnimationScrollViewerExtention.CurrentVerticalOffsetProperty);
            aniItem.From = contentScroll.VerticalOffset;
            aniItem.To = targetVerticalOffset;
            aniItem.Duration = new Duration(new System.TimeSpan(0, 0, 0, 0, 500));
            aniItem.Mode = Custom.Wpf.Global.Enums.EasingFunctionBaseMode.ExponentialEaseIn;

            sb.Children.Add(aniItem);

            sb.Begin(contentScroll);
        }
    }
}
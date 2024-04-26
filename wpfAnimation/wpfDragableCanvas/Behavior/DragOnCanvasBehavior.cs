using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace wpfDragableCanvas.Behavior
{
    public class DragOnCanvasBehavior : Behavior<UIElement>
    {


        public UIElement BaseUIElement
        {
            get { return (UIElement)GetValue(BaseUIElementProperty); }
            set { SetValue(BaseUIElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BaseUIElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BaseUIElementProperty =
            DependencyProperty.Register("BaseUIElement", typeof(UIElement), typeof(DragOnCanvasBehavior), new PropertyMetadata(null));


        public UIElement Target
        {
            get { return (UIElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(UIElement), typeof(DragOnCanvasBehavior), new PropertyMetadata(null));
        protected override void OnAttached()
        {
            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            base.OnAttached();
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            base.OnDetaching();
        }
        private Point _initMousePositons;
        private Point _initItemPositons;
        private bool _onDrag = false;
        private void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(_onDrag == false)
            {
                var captureBase = this.BaseUIElement ?? this.AssociatedObject;
                captureBase.MouseMove += CaptureBase_MouseMove;
                captureBase.MouseLeftButtonUp += CaptureBase_MouseLeftButtonUp;

                UIElement target = Target ?? this.AssociatedObject;

                _onDrag = true;
                _initMousePositons = e.GetPosition(captureBase);
                double x = Canvas.GetLeft(target);
                double y = Canvas.GetTop(target);

                x = double.IsNaN(x) ? 0 : x;
                y = double.IsNaN(y) ? 0 : y;

                _initItemPositons = new Point(x, y);
                captureBase.CaptureMouse();
            }
        }

        private void CaptureBase_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this._onDrag = false;

            UIElement captureBase = (this.BaseUIElement ?? this.AssociatedObject);
            captureBase.ReleaseMouseCapture();

            captureBase.MouseMove -= CaptureBase_MouseMove;
            captureBase.MouseLeftButtonUp -= CaptureBase_MouseLeftButtonUp;        
                
        }

        private void CaptureBase_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && this._onDrag == true)
            {
                UIElement captureBase = (this.BaseUIElement ?? this.AssociatedObject);
                UIElement target = (this.Target ?? this.AssociatedObject);

                Vector delta = e.GetPosition(captureBase) - this._initItemPositons;
                Canvas.SetLeft(target, this._initItemPositons.X + delta.X);
                Canvas.SetTop(target, this._initItemPositons.Y + delta.Y);
            }
        }
    }
}

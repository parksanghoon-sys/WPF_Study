using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Threading;
using wpfNotifyIcon.Interop;
using Point = wpfNotifyIcon.Interop.Point;

namespace wpfNotifyIcon
{
    public partial class TaskbarIcon : FrameworkElement, IDisposable
    {
        private readonly object lockObject = new object();
        #region Members
        private NotifyIconData _iconData;
        private readonly WindowMessageSink _messageSink;
        /// <summary>
        /// An action that is being invoked if the
        /// <see cref="singleClickTimer"/> fires.
        /// </summary>
        private Action _singleClickTimerAction;
        private readonly Timer _singleClickTimer;
        private readonly Timer _balloonCloseTimer;
        private int DoubleClickWaitTime => NoLeftClickDelay ? 0 : WinApi.GetDoubleClickTime();
        

        /// <summary>
        /// Checks whether a non-tooltip popup is currently opened.
        /// </summary>
        private bool IsPopupOpen
        {
            get
            {
                var popup = TrayPopupResolved;
                var menu = ContextMenu;
                var balloon = CustomBalloon;

                return popup != null && popup.IsOpen ||
                       menu != null && menu.IsOpen ||
                       balloon != null && balloon.IsOpen;
            }
        }
        #endregion
        public bool IsTaskbarIconCreated { get; private set; }
        public delegate Point GetCustomPopupPosition();
        public GetCustomPopupPosition CustomPopupPosition { get; set; }
        /// <summary>
        /// Returns the location of the system tray
        /// </summary>
        /// <returns>Point</returns>
        public Point GetPopupTrayPosition()
        {
            return TrayInfo.GetTrayLocation();
        }
        /// <summary>
        /// Set to true as soon as <c>Dispose</c> has been invoked.
        /// </summary>
        public bool IsDisposed { get; private set; }
        public TaskbarIcon()
        {
            _messageSink = Util.IsDesignMode ? WindowMessageSink.CreateEmpty() : new WindowMessageSink(NotifyIconVersion.Win95);

            _iconData = NotifyIconData.CreateDefault(_messageSink.MessageWindowHandle);

            CreateTaskbarIcon();

            _messageSink.MouseEventReceived += OnMouseEvent;
            _messageSink.ContextMenuReceived += ShowContextMenu;
            _messageSink.TaskbarCreated += OnTaskbarCreated;
            _messageSink.ChangeToolTipStateRequest += OnToolTipChange;
            //_messageSink.BalloonToolTipChanged += OnBalloonToolTipChanged;

            _singleClickTimer = new Timer(DoSingleClickAction);
            _balloonCloseTimer = new Timer(CloseBalloonCallback);

            if (Application.Current != null)
            {
                Application.Current.Exit += OnExit;
            }
        }
        public void ShowCustomBalloon(UIElement balloon, PopupAnimation animation, int? timeout)
        {
            var dispatcher = this.GetDispatcher();
            if (!dispatcher.CheckAccess())
            {
                var action = new Action(() => ShowCustomBalloon(balloon, animation, timeout));
                dispatcher.Invoke(DispatcherPriority.Normal, action);
                return;
            }
            if (balloon == null) throw new ArgumentNullException(nameof(balloon));
            if (timeout.HasValue && timeout < 500)
            {
                string msg = "Invalid timeout of {0} milliseconds. Timeout must be at least 500 ms";
                msg = string.Format(msg, timeout);
                throw new ArgumentOutOfRangeException(nameof(timeout), msg);
            }

            EnsureNotDisposed();

            // make sure we don't have an open balloon
            lock (lockObject)
            {
                CloseBalloon();
            }

            // create an invisible popup that hosts the UIElement
            Popup popup = new Popup
            {
                AllowsTransparency = true
            };

            // provide the popup with the taskbar icon's data context
            UpdateDataContext(popup, null, DataContext);

            // don't animate by default - developers can use attached events or override
            popup.PopupAnimation = animation;

            // in case the balloon is cleaned up through routed events, the
            // control didn't remove the balloon from its parent popup when
            // if was closed the last time - just make sure it doesn't have
            // a parent that is a popup
            var parent = LogicalTreeHelper.GetParent(balloon) as Popup;
            if (parent != null) parent.Child = null;

            if (parent != null)
            {
                string msg = "Cannot display control [{0}] in a new balloon popup - that control already has a parent. You may consider creating new balloons every time you want to show one.";
                msg = string.Format(msg, balloon);
                throw new InvalidOperationException(msg);
            }

            popup.Child = balloon;

            //don't set the PlacementTarget as it causes the popup to become hidden if the
            //TaskbarIcon's parent is hidden, too...
            //popup.PlacementTarget = this;

            popup.Placement = PopupPlacement;
            popup.StaysOpen = true;


            Point position = CustomPopupPosition != null ? CustomPopupPosition() : GetPopupTrayPosition();
            popup.HorizontalOffset = position.X - 1;
            popup.VerticalOffset = position.Y - 1;

            //store reference
            lock (lockObject)
            {
                SetCustomBalloon(popup);
            }

            // assign this instance as an attached property
            SetParentTaskbarIcon(balloon, this);

            // fire attached event
            RaiseBalloonShowingEvent(balloon, this);

            // display item
            popup.IsOpen = true;

            if (timeout.HasValue)
            {
                // register timer to close the popup
                _balloonCloseTimer.Change(timeout.Value, Timeout.Infinite);
            }
        }
        /// <summary>
        /// Hides a balloon ToolTip, if any is displayed.
        /// </summary>
        public void HideBalloonTip()
        {
            EnsureNotDisposed();

            // reset balloon by just setting the info to an empty string
            _iconData.BalloonText = _iconData.BalloonTitle = string.Empty;
            Util.WriteIconData(ref _iconData, NotifyCommand.Modify, IconDataMembers.Info);
        }
        public void ShowTrayPopup()
        {
            if (IsDisposed) return;

            var args = RaisePreviewTrayPopupOpenEvent();
            if (args.Handled) return;

            if (TrayPopup == null) return;

        }
        private void CloseBalloonCallback(object? state)
        {
            if (IsDisposed) return;

            // switch to UI thread
            Action action = CloseBalloon;
            this.GetDispatcher().Invoke(action);
        }
        public void CloseBalloon()
        {
            if(IsDisposed ) return;

            Dispatcher dispatcher = this.GetDispatcher();
            if(dispatcher.CheckAccess() == false)
            {
                Action action = CloseBalloon;
                dispatcher.Invoke(DispatcherPriority.Normal, action);
            }
            lock(lockObject)
            {
                _balloonCloseTimer.Change(Timeout.Infinite, Timeout.Infinite);

                Popup popup = CustomBalloon;
                if (popup == null) return;

                UIElement element = popup.Child;

                RoutedEventArgs eventArgs = RaiseBalloonClosingEvent(element, this);
                if (!eventArgs.Handled)
                {
                    popup.IsOpen = false;
                    popup.Child = null;
                    if (element != null) SetParentTaskbarIcon(element, null);
                }
                SetCustomBalloon(null);
            }
        }
        private void DoSingleClickAction(object? state)
        {
            if (IsDisposed) return;

            // run action
            Action action = _singleClickTimerAction;
            if (action != null)
            {
                // cleanup action
                _singleClickTimerAction = null;

                // switch to UI thread
                this.GetDispatcher().Invoke(action);
            }
        }

        //private void OnBalloonToolTipChanged(bool visible)
        //{
        //    if (visible)
        //    {
        //        RaiseTrayBalloonTipShownEvent();
        //    }
        //    else
        //    {
        //        RaiseTrayBalloonTipClosedEvent();
        //    }
        //}

        private void OnToolTipChange(bool visible)
        {
            if (TrayToolTipResolved == null) return;
            if(visible)
            {
                if (IsPopupOpen)
                    return;

                var args = RaisePreviewTrayToolTipOpenEvent();
                if (args.Handled) return;

                TrayToolTipResolved.IsOpen = true;

                // raise attached event first
                if (TrayToolTip != null) RaiseToolTipOpenedEvent(TrayToolTip);

                // bubble routed event
                RaiseTrayToolTipOpenEvent();
            }
            else
            {
                var args = RaisePreviewTrayToolTipCloseEvent();
                if (args.Handled) return;

                // raise attached event first
                if (TrayToolTip != null) RaiseToolTipCloseEvent(TrayToolTip);

                TrayToolTipResolved.IsOpen = false;

                // bubble event
                RaiseTrayToolTipCloseEvent();
            }

        }

        private void OnTaskbarCreated()
        {
            RemoveTaskbarIcon();
            CreateTaskbarIcon();
        }

        private void RemoveTaskbarIcon()
        {
            lock (lockObject)
            {
                // make sure we didn't schedule a creation

                if (!IsTaskbarIconCreated)
                {
                    return;
                }

                Util.WriteIconData(ref _iconData, NotifyCommand.Delete, IconDataMembers.Message);
                IsTaskbarIconCreated = false;
            }
        }

      
        private void ShowContextMenu(Interop.Point point)
        {
            if(IsDisposed) return;
            var args = RaisePreviewTrayContextMenuOpenEvent();

            if(args.Handled) return;

            if(ContextMenu == null) return;

            ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.AbsolutePoint;
            ContextMenu.HorizontalOffset = point.X;
            ContextMenu.VerticalOffset = point.Y;
            ContextMenu.IsOpen = true;

            IntPtr handle = IntPtr.Zero;
            HwndSource source = (HwndSource)PresentationSource.FromVisual(ContextMenu);

            if(source != null) 
                handle = source.Handle;

            if (handle == IntPtr.Zero) handle = _messageSink.MessageWindowHandle;

            WinApi.SetForegroundWindow(handle);

            // bubble event
            RaiseTrayContextMenuOpenEvent();
        }

        private void OnMouseEvent(MouseEvent me)
        {
            if (IsDisposed) return;

            switch (me)
            {               
                case MouseEvent.IconRightMouseDown:
                    RaiseTrayRightMouseDownEvent();
                    break;
                case MouseEvent.IconLeftMouseDown:
                    RaiseTrayLeftMouseDownEvent();
                    break;
                case MouseEvent.IconRightMouseUp:
                    RaiseTrayRightMouseUpEvent();
                    break;
                case MouseEvent.IconLeftMouseUp:
                    RaiseTrayLeftMouseUpEvent();
                    break;
                case MouseEvent.IconDoubleClick:
                    // cancel single click timer
                    _singleClickTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    // bubble event
                    RaiseTrayMouseDoubleClickEvent();
                    break;
                case MouseEvent.MouseMove:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(me), "Missing handler for mouse event flag: " + me);
            }
            var cursorPoint = WinApi.GetCursorPosition(_messageSink.Version);
            bool isLeftClickCommandInvoked = false;

            if(me.IsMatch(PopupActivation))
            {
                if(me == MouseEvent.IconLeftMouseUp)
                {
                    _singleClickTimerAction = () =>
                    {
                        LeftClickCommand.ExecuteIfEnabled(LeftClickCommandParameter, LeftClickCommandTarget ?? this);
                        ShowTrayPopup();
                    };
                    _singleClickTimer.Change(DoubleClickWaitTime, Timeout.Infinite);
                    isLeftClickCommandInvoked = true;
                }
                else
                {
                    ShowTrayPopup();
                }
            }
            // show context menu, if requested
            if (me.IsMatch(MenuActivation))
            {
                if (me == MouseEvent.IconLeftMouseUp)
                {
                    // show context menu once we are sure it's not a double click
                    _singleClickTimerAction = () =>
                    {
                        LeftClickCommand.ExecuteIfEnabled(LeftClickCommandParameter, LeftClickCommandTarget ?? this);
                        ShowContextMenu(cursorPoint);
                    };
                    _singleClickTimer.Change(DoubleClickWaitTime, Timeout.Infinite);
                    isLeftClickCommandInvoked = true;
                }
                else
                {
                    // show context menu immediately
                    ShowContextMenu(cursorPoint);
                }
            }

            // make sure the left click command is invoked on mouse clicks
            if (me == MouseEvent.IconLeftMouseUp && !isLeftClickCommandInvoked)
            {
                // show context menu once we are sure it's not a double click
                _singleClickTimerAction =
                    () =>
                    {
                        LeftClickCommand.ExecuteIfEnabled(LeftClickCommandParameter, LeftClickCommandTarget ?? this);
                    };
                _singleClickTimer.Change(DoubleClickWaitTime, Timeout.Infinite);
            }

        }

        private void CreateTaskbarIcon()
        {
            lock(lockObject)
            {                
                if (IsTaskbarIconCreated) return;

                const IconDataMembers iconDataMembers = IconDataMembers.Message | IconDataMembers.Icon | IconDataMembers.Tip;
                var status = Util.WriteIconData(ref _iconData, NotifyCommand.Add, iconDataMembers);
                
                if (status == false) return;

                SetVersion();
                _messageSink.Version = (NotifyIconVersion)_iconData.VersionOrTimeout;

                IsTaskbarIconCreated = true;
            }
        }

        private void SetVersion()
        {
            _iconData.VersionOrTimeout = (uint)NotifyIconVersion.Vista;
            bool status = WinApi.Shell_NotifyIcon(NotifyCommand.SetVersion, ref _iconData);

            if (!status)
            {
                _iconData.VersionOrTimeout = (uint)NotifyIconVersion.Win2000;
                status = Util.WriteIconData(ref _iconData, NotifyCommand.SetVersion);
            }

            if (!status)
            {
                _iconData.VersionOrTimeout = (uint)NotifyIconVersion.Win95;
                status = Util.WriteIconData(ref _iconData, NotifyCommand.SetVersion);
            }

            if (!status)
            {
                Debug.Fail("Could not set version");
            }
        }
        private void CreatePopup()
        {
            Popup popup = TrayPopup as Popup;
            if (popup == null && TrayPopup != null)
            {
                // create an invisible popup that hosts the UIElement
                popup = new Popup
                {
                    AllowsTransparency = true,
                    // don't animate by default - developers can use attached events or override
                    PopupAnimation = PopupAnimation.None,
                    // the CreateRootPopup method outputs binding errors in the debug window because
                    // it tries to bind to "Popup-specific" properties in case they are provided by the child.
                    // We don't need that so just assign the control as the child.
                    Child = TrayPopup,
                    // do *not* set the placement target, as it causes the popup to become hidden if the
                    // TaskbarIcon's parent is hidden, too. At runtime, the parent can be resolved through
                    // the ParentTaskbarIcon attached dependency property:
                    // PlacementTarget = this;

                    Placement = PopupPlacement,
                    StaysOpen = false
                };
            }

            // the popup explicitly gets the DataContext of this instance.
            // If there is no DataContext, the TaskbarIcon assigns itself
            if (popup != null)
            {
                UpdateDataContext(popup, null, DataContext);
            }

            // store a reference to the used tooltip
            SetTrayPopupResolved(popup);
        }
       

        private void UpdateDataContext(FrameworkElement target, object oldDataContextValue, object newDataContextValue)
        {
            //if there is no target or it's data context is determined through a binding
            //of its own, keep it
            if (target == null || target.IsDataContextDataBound()) return;

            //if the target's data context is the NotifyIcon's old DataContext or the NotifyIcon itself,
            //update it
            if (ReferenceEquals(this, target.DataContext) || Equals(oldDataContextValue, target.DataContext))
            {
                //assign own data context, if available. If there is no data
                //context at all, assign NotifyIcon itself.
                target.DataContext = newDataContextValue ?? this;
            }
        }

        #region ToolTips

        /// <summary>
        /// Creates a <see cref="ToolTip"/> control that either
        /// wraps the currently set <see cref="TrayToolTip"/>
        /// control or the <see cref="ToolTipText"/> string.<br/>
        /// If <see cref="TrayToolTip"/> itself is already
        /// a <see cref="ToolTip"/> instance, it will be used directly.
        /// </summary>
        /// <remarks>We use a <see cref="ToolTip"/> rather than
        /// <see cref="Popup"/> because there was no way to prevent a
        /// popup from causing cyclic open/close commands if it was
        /// placed under the mouse. ToolTip internally uses a Popup of
        /// its own, but takes advance of Popup's internal <see cref="UIElement.IsHitTestVisible"/>
        /// property which prevents this issue.</remarks>
        private void CreateCustomToolTip()
        {
            // check if the item itself is a tooltip
            ToolTip tt = TrayToolTip as ToolTip;

            if (tt == null && TrayToolTip != null)
            {
                // create an invisible wrapper tooltip that hosts the UIElement
                tt = new ToolTip
                {
                    Placement = PlacementMode.Mouse,
                    // do *not* set the placement target, as it causes the popup to become hidden if the
                    // TaskbarIcon's parent is hidden, too. At runtime, the parent can be resolved through
                    // the ParentTaskbarIcon attached dependency property:
                    // PlacementTarget = this;

                    // make sure the tooltip is invisible
                    HasDropShadow = false,
                    BorderThickness = new Thickness(0),
                    Background = System.Windows.Media.Brushes.Transparent,
                    // setting the
                    StaysOpen = true,
                    Content = TrayToolTip
                };
            }
            else if (tt == null && !string.IsNullOrEmpty(ToolTipText))
            {
                // create a simple tooltip for the ToolTipText string
                tt = new ToolTip
                {
                    Content = ToolTipText
                };
            }

            // the tooltip explicitly gets the DataContext of this instance.
            // If there is no DataContext, the TaskbarIcon assigns itself
            if (tt != null)
            {
                UpdateDataContext(tt, null, DataContext);
            }

            // store a reference to the used tooltip
            SetTrayToolTipResolved(tt);
        }


        /// <summary>
        /// Sets tooltip settings for the class depending on defined
        /// dependency properties and OS support.
        /// </summary>
        private void WriteToolTipSettings()
        {
            const IconDataMembers flags = IconDataMembers.Tip;
            _iconData.ToolTipText = ToolTipText;

            if (_messageSink.Version == NotifyIconVersion.Vista)
            {
                // we need to set a tooltip text to get tooltip events from the
                // taskbar icon
                if (string.IsNullOrEmpty(_iconData.ToolTipText) && TrayToolTipResolved != null)
                {
                    // if we have not tooltip text but a custom tooltip, we
                    // need to set a dummy value (we're displaying the ToolTip control, not the string)
                    _iconData.ToolTipText = "ToolTip";
                }
            }

            // update the tooltip text
            Util.WriteIconData(ref _iconData, NotifyCommand.Modify, flags);
        }

        #endregion
        #region Dispose / Exit
        /// <summary>
        /// Checks if the object has been disposed and
        /// raises a <see cref="ObjectDisposedException"/> in case
        /// the <see cref="IsDisposed"/> flag is true.
        /// </summary>
        private void EnsureNotDisposed()
        {
            if (IsDisposed) throw new ObjectDisposedException(Name ?? GetType().FullName);
        }


        /// <summary>
        /// Disposes the class if the application exits.
        /// </summary>
        private void OnExit(object sender, EventArgs e)
        {
            Dispose();
        }


        /// <summary>
        /// This destructor will run only if the <see cref="Dispose()"/>
        /// method does not get called. This gives this base class the
        /// opportunity to finalize.
        /// <para>
        /// Important: Do not provide destructor in types derived from this class.
        /// </para>
        /// </summary>
        ~TaskbarIcon()
        {
            Dispose(false);
        }


        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <remarks>This method is not virtual by design. Derived classes
        /// should override <see cref="Dispose(bool)"/>.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Closes the tray and releases all resources.
        /// </summary>
        /// <summary>
        /// <c>Dispose(bool disposing)</c> executes in two distinct scenarios.
        /// If disposing equals <c>true</c>, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// </summary>
        /// <param name="disposing">If disposing equals <c>false</c>, the method
        /// has been called by the runtime from inside the finalizer and you
        /// should not reference other objects. Only unmanaged resources can
        /// be disposed.</param>
        /// <remarks>Check the <see cref="IsDisposed"/> property to determine whether
        /// the method has already been called.</remarks>
        private void Dispose(bool disposing)
        {
            // don't do anything if the component is already disposed
            if (IsDisposed || !disposing) return;

            lock (lockObject)
            {
                IsDisposed = true;

                // de-register application event listener
                if (Application.Current != null)
                {
                    Application.Current.Exit -= OnExit;
                }

                // stop timers
                _singleClickTimer.Dispose();
                _balloonCloseTimer.Dispose();

                // dispose message sink
                _messageSink.Dispose();

                // remove icon
                RemoveTaskbarIcon();
            }
        }

        #endregion
    }
}

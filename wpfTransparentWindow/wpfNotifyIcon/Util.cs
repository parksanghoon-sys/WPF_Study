using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Threading;
using wpfNotifyIcon.Interop;

namespace wpfNotifyIcon
{
    internal static class Util
    {
        public static readonly object _syncRoot = new object();
        private static readonly bool _isDesignMode;
        public static bool IsDesignMode
        {
            get => _isDesignMode;
        }
        static Util()
        {
            _isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty,typeof(FrameworkElement)).Metadata.DefaultValue;
        }
        public static Window CreateHelperWindow()
        {
            return new Window
            {
                Width = 0,
                Height = 0,
                ShowInTaskbar = false,
                WindowStyle = WindowStyle.None,
                Opacity = 0
            };
        }
        public static bool WriteIconData(ref NotifyIconData data, NotifyCommand command)
        {
            return WriteIconData(ref data, command, data.ValidMembers);
        }

        public static bool WriteIconData(ref NotifyIconData data, NotifyCommand command, IconDataMembers flags)
        {
            //do nothing if in design mode
            if (IsDesignMode) return true;

            data.ValidMembers = flags;
            lock (_syncRoot)
            {
                return WinApi.Shell_NotifyIcon(command, ref data);
            }
        }
        public static BalloonFlags GetBalloonFlag(this BalloonIcon icon)
        {
            switch (icon)
            {
                case BalloonIcon.None:
                    return BalloonFlags.None;
                case BalloonIcon.Info:
                    return BalloonFlags.Info;
                case BalloonIcon.Warning:
                    return BalloonFlags.Warning;
                case BalloonIcon.Error:
                    return BalloonFlags.Error;
                default:
                    throw new ArgumentOutOfRangeException("icon");
            }
        }
        public static Icon ToIcon(this ImageSource imageSource)
        {
            if (imageSource == null) return null;

            Uri uri = new Uri(imageSource.ToString());
            StreamResourceInfo streamInfo = Application.GetResourceStream(uri);

            if (streamInfo == null)
            {
                string msg = "The supplied image source '{0}' could not be resolved.";
                msg = string.Format(msg, imageSource);
                throw new ArgumentException(msg);
            }

            Interop.Size iconSize = SystemInfo.SmallIconSize;
            return new Icon(streamInfo.Stream, new System.Drawing.Size(iconSize.Width, iconSize.Height));
        }
        #region evaluate listings

        /// <summary>
        /// Checks a list of candidates for equality to a given
        /// reference value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The evaluated value.</param>
        /// <param name="candidates">A liste of possible values that are
        /// regarded valid.</param>
        /// <returns>True if one of the submitted <paramref name="candidates"/>
        /// matches the evaluated value. If the <paramref name="candidates"/>
        /// parameter itself is null, too, the method returns false as well,
        /// which allows to check with null values, too.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="candidates"/>
        /// is a null reference.</exception>
        public static bool Is<T>(this T value, params T[] candidates)
        {
            if (candidates == null) return false;

            foreach (var t in candidates)
            {
                if (value.Equals(t)) return true;
            }

            return false;
        }

        #endregion

        #region match MouseEvent to PopupActivation

        /// <summary>
        /// Checks if a given <see cref="PopupActivationMode"/> is a match for
        /// an effectively pressed mouse button.
        /// </summary>
        public static bool IsMatch(this MouseEvent me, PopupActivationMode activationMode)
        {
            switch (activationMode)
            {
                case PopupActivationMode.LeftClick:
                    return me == MouseEvent.IconLeftMouseUp;
                case PopupActivationMode.RightClick:
                    return me == MouseEvent.IconRightMouseUp;
                case PopupActivationMode.LeftOrRightClick:
                    return me.Is(MouseEvent.IconLeftMouseUp, MouseEvent.IconRightMouseUp);
                case PopupActivationMode.LeftOrDoubleClick:
                    return me.Is(MouseEvent.IconLeftMouseUp, MouseEvent.IconDoubleClick);
                case PopupActivationMode.DoubleClick:
                    return me.Is(MouseEvent.IconDoubleClick);
                case PopupActivationMode.MiddleClick:
                    return me == MouseEvent.IconMiddleMouseUp;
                case PopupActivationMode.All:
                    //return true for everything except mouse movements
                    return me != MouseEvent.MouseMove;
                case PopupActivationMode.None:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException("activationMode");
            }
        }

        #endregion

        #region execute command

        /// <summary>
        /// Executes a given command if its <see cref="ICommand.CanExecute"/> method
        /// indicates it can run.
        /// </summary>
        /// <param name="command">The command to be executed, or a null reference.</param>
        /// <param name="commandParameter">An optional parameter that is associated with
        /// the command.</param>
        /// <param name="target">The target element on which to raise the command.</param>
        public static void ExecuteIfEnabled(this ICommand command, object commandParameter, IInputElement target)
        {
            if (command == null) return;

            RoutedCommand rc = command as RoutedCommand;
            if (rc != null)
            {
                //routed commands work on a target
                if (rc.CanExecute(commandParameter, target)) rc.Execute(commandParameter, target);
            }
            else if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }

        #endregion

        /// <summary>
        /// Returns a dispatcher for multi-threaded scenarios
        /// </summary>
        /// <returns>Dispatcher</returns>
        internal static Dispatcher GetDispatcher(this DispatcherObject source)
        {
            //use the application's dispatcher by default
            if (Application.Current != null) return Application.Current.Dispatcher;

            //fallback for WinForms environments
            if (source.Dispatcher != null) return source.Dispatcher;

            // ultimately use the thread's dispatcher
            return Dispatcher.CurrentDispatcher;
        }


        /// <summary>
        /// Checks whether the <see cref="FrameworkElement.DataContextProperty"/>
        ///  is bound or not.
        /// </summary>
        /// <param name="element">The element to be checked.</param>
        /// <returns>True if the data context property is being managed by a
        /// binding expression.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="element"/>
        /// is a null reference.</exception>
        public static bool IsDataContextDataBound(this FrameworkElement element)
        {
            if (element == null) throw new ArgumentNullException("element");
            return element.GetBindingExpression(FrameworkElement.DataContextProperty) != null;
        }
    }
}

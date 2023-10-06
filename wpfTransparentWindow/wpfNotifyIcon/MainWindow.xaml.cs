using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace wpfNotifyIcon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WM_APP = 0x8000;
        private const int MYMESSAGE = WM_APP + 1;
        private const int WM_SYSCOMMAND = 0x112;
        private NOTIFYICONDATA _notifyIcon;
        private IntPtr _contextMenu;
        
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Minimized;

            this.Loaded += (s, e) =>
            {
                _notifyIcon = CreateNotifyIcon();
                _contextMenu = CreateContextMenu();
                ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
            };

            this.Closed += (s, e) =>
            {
                Shell_NotifyIcon(2 /* NIM_DELETE */, ref _notifyIcon);
                DestroyMenu(_contextMenu);
                ComponentDispatcher.ThreadPreprocessMessage -= ComponentDispatcher_ThreadPreprocessMessage;
            };
        }
        [DllImport("shell32.dll")]
        static extern bool Shell_NotifyIcon(uint dwMessage, ref NOTIFYICONDATA lpData);

        [DllImport("user32.dll")]
        public static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll")]
        public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        public static extern bool DestroyMenu(IntPtr hMenu);
        [DllImport("user32.dll")]
        public static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool TrackPopupMenuEx(IntPtr hmenu, uint fuFlags,
           int x, int y, IntPtr hwnd, IntPtr lptpm);
        [StructLayout(LayoutKind.Sequential)]
        struct NOTIFYICONDATA
        {
            public uint cbSize;
            public IntPtr hWnd;
            public uint uID;
            public uint uFlags;
            public uint uCallbackMessage;
            public IntPtr hIcon;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szTip;

            // Other fields not used in this example...
        }
        private NOTIFYICONDATA CreateNotifyIcon()
        {
            string uriPath = "pack://application:,,,/wpfNotifyIcon;component/ex.ico";
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ex.ico");

            Stream iconStream = App.GetResourceStream(new Uri(uriPath)).Stream;
            
            var nid = new NOTIFYICONDATA();
            nid.cbSize = (uint)Marshal.SizeOf(nid);
            nid.hWnd = new WindowInteropHelper(this).Handle;
            nid.uCallbackMessage = WM_SYSCOMMAND;
            nid.uFlags = 0x00000001;
            nid.szTip = "TEST";
            nid.uCallbackMessage = MYMESSAGE;

            var tempFilePath = Path.GetTempFileName();
            using (var fileStream = File.OpenWrite(tempFilePath))
            {
                iconStream.CopyTo(fileStream);
            }
            using (var icon = new Icon(tempFilePath))
                nid.hIcon = icon.Handle;

            // Add icon to system tray
            var test = Shell_NotifyIcon(0 /* NIM_ADD */, ref nid);

            //nid.hIcon = LoadImage(IntPtr.Zero, "icon.ico", 1 /* IMAGE_ICON */, 0, 0,1 /* LR_LOADFROMFILE */);
            // Add icon to system tray
            //Shell_NotifyIcon(0 /* NIM_ADD */, ref nid);

            return nid;
        }
        private void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message == MYMESSAGE)
            {
                if ((int)msg.lParam == 0x0201 /* WM_LBUTTONDOWN */ || (int)msg.lParam == 0x0204 /* WM_RBUTTONDOWN */)
                {
                    ShowContextMenu(msg.hwnd);
                }
            }
        }

        private void ShowContextMenu(IntPtr handle)
        {
            SetForegroundWindow(handle); // To make sure the menu closes properly when clicking outside of it

            TrackPopupMenuEx(_contextMenu, 0x0000 /* TPM_LEFTALIGN */, (int)SystemParameters.VirtualScreenLeft, (int)SystemParameters.VirtualScreenTop, handle, IntPtr.Zero);
        }

        private IntPtr CreateContextMenu()
        {
            var menu = CreatePopupMenu();

            AppendMenu(menu, 0x0000 /* MF_STRING */, 1 /* ID */, "Option 1");
            AppendMenu(menu, 0x0000 /* MF_STRING */, 2 /* ID */, "Option 2");

            return menu;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace wpfNotifyIcon.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Size
    {
        /// <summary>
        /// Width.
        /// </summary>
        public int Width;
        /// <summary>
        /// Height.
        /// </summary>
        public int Height;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace wpfNotifyIcon.Interop
{
    /// <summary>
    /// 비관리 코드로 내보낼 때 개체의 레이아웃을 제어
    /// 관리되지 않는 메모리로 내보낼 때 표시되는 순서대로 배치된다 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y;
    }
}

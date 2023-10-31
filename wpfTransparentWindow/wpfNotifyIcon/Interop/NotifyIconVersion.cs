﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfNotifyIcon.Interop
{
    public enum NotifyIconVersion
    {
        /// <summary>
        /// Default behavior (legacy Win95). Expects
        /// a <see cref="NotifyIconData"/> size of 488.
        /// </summary>
        Win95 = 0x0,

        /// <summary>
        /// Behavior representing Win2000 an higher. Expects
        /// a <see cref="NotifyIconData"/> size of 504.
        /// </summary>
        Win2000 = 0x3,

        /// <summary>
        /// Extended tooltip support, which is available for Vista and later.
        /// Detailed information about what the different versions do, can be found <a href="https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyicona">here</a>
        /// </summary>
        Vista = 0x4
    }
}

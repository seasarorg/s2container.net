#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System.Runtime.InteropServices;
using HWND = System.IntPtr;

namespace Seasar.Extension.UI
{
    /// <summary>
    /// Win32 API を使用する為のファサードクラス
    /// </summary>
    public class Win32
    {
        private Win32() {}
        
        // see. WINUSER.H
        public const int WM_USER    = 0x0400;
        public const int WM_PAINT   = 0x000F;
        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;

        public const int EM_GETSCROLLPOS = (WM_USER + 221);
        public const int EM_SETSCROLLPOS = (WM_USER + 222);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT 
        {
            public int x;
            public int y;
        }

        [DllImport("user32")] public static extern int SendMessage(HWND hwnd, int wMsg, int wParam, HWND lParam);
        [DllImport("user32")] public static extern int PostMessage(HWND hwnd, int wMsg, int wParam, int lParam);
    }
}

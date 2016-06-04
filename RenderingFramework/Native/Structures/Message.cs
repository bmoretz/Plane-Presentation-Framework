using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RenderingFramework.Native
{
    /// <summary>
    /// Windows Message
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    internal struct Message
    {
        public IntPtr hWnd;
        public WindowMessage msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public Point p;
    }
}
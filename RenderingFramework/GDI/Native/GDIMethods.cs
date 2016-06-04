using System;
using System.Runtime.InteropServices;
using System.Security;

namespace RenderingFramework.GDI.Native
{
    class GDIMethods
    {
        #region Definitons

        const string DLL_Gdi32 = "gdi32.dll";

        #endregion

        #region Windows GDI Calls

        [SuppressUnmanagedCodeSecurity]
        [DllImport( DLL_Gdi32 )]
        public static extern IntPtr CreateCompatibleDC( IntPtr hDC );

        [SuppressUnmanagedCodeSecurity]
        [DllImport( DLL_Gdi32 )]
        public static extern IntPtr SelectObject( IntPtr hDC, IntPtr hObject );

        [SuppressUnmanagedCodeSecurity]
        [DllImport( DLL_Gdi32 )]
        public static extern bool DeleteObject( IntPtr hObject );

        [SuppressUnmanagedCodeSecurity]
        [DllImport( DLL_Gdi32 )]
        public static extern bool BitBlt( IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop );

        #endregion
    }
}

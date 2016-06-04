using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace RenderingFramework.Native
{
    /// <summary>
    /// Win32 Native Methods
    /// </summary>
    internal class NativeMethods
    {
        #region Definitons

        const string DLL_MultiMedia = "winmm.dll";
        const string DLL_Kernel32 = "kernel32";
        const string DLL_User32 = "User32.dll";

        #endregion

        #region Windows API Calls

        [SuppressUnmanagedCodeSecurity]
        [DllImport( DLL_MultiMedia )]
        public static extern IntPtr timeBeginPeriod( uint period );

        [SuppressUnmanagedCodeSecurity]
        [DllImport( DLL_MultiMedia )]
        public static extern IntPtr timeEndPeriod( uint period );

        [SuppressUnmanagedCodeSecurity]
        [DllImport( DLL_Kernel32 )]
        public static extern bool QueryPerformanceFrequency( ref long PerformanceFrequency );

        [SuppressUnmanagedCodeSecurity]
        [DllImport( DLL_Kernel32 )]
        public static extern bool QueryPerformanceCounter( ref long PerformanceCount );

        [SuppressUnmanagedCodeSecurity]
        [DllImport( DLL_User32, CharSet = CharSet.Auto )]
        public static extern short GetAsyncKeyState( uint key );

        [SuppressUnmanagedCodeSecurity]
        [DllImport( DLL_User32, CharSet = CharSet.Auto )]
        public static extern bool PeekMessage( out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags );

        #endregion

        #region Utility Methods

        /// <summary>Returns the low word </summary>
        internal static short LoWord( uint l )
        {
            return ( short ) ( l & 0xffff );
        }

        /// <summary>Returns the high word </summary>
        internal static short HiWord( uint l )
        {
            return ( short ) ( l >> 16 );
        }

        /// <summary>Makes two shorts into a long</summary>
        internal static uint MakeUInt32( short l, short r )
        {
            return ( uint ) ( ( l & 0xffff ) | ( ( r & 0xffff ) << 16 ) );
        }

        /// <summary>Is this key down right now</summary>
        internal static bool IsKeyDown( System.Windows.Forms.Keys key )
        {
            return ( GetAsyncKeyState( ( int ) Keys.ShiftKey ) & 0x8000 ) != 0;
        }

        #endregion
    }
}
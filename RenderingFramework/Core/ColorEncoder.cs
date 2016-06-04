using System.Drawing;

namespace RenderingFramework.Core
{
    internal static class ColorEncoder
    {
        #region 332

        internal static byte Format332FromColor( Color color )
        {
            return ( byte )
            (
                ( ( color.R >> 5 ) << 5 ) |
                ( ( color.G >> 5 ) << 2 ) |
                ( color.B >> 6 )
            );
        }

        internal static Color FormatColorFrom332( byte color )
        {
            return Color.FromArgb
            (
                ( color & 0xE0 ),
                ( ( color & 0x1C ) << 3 ),
                ( ( color & 0x03 ) << 6 )
            );
        }

        #endregion

        #region 565

        internal static ushort Format565FromColor( Color color )
        {
            return ( ushort )
            (
                ( ( color.R >> 3 ) << 11 ) +
                ( ( color.G >> 2 ) << 5 ) +
                ( color.B >> 3 )
            );
        }

        internal static Color FormatColorFrom565( ushort color )
        {
            return Color.FromArgb
            (
                ( ( color & 0xF800 ) >> 8 ),
                ( ( color & 0x07E0 ) >> 3 ),
                ( ( color & 0x001F ) << 3 )
            );
        }

        #endregion

        #region 888

        internal static uint Format888FromColor(Color color)
        {
            return (uint)
            (
                ( color.A << 24 ) +
                ( color.R << 16 ) +
                ( color.G << 8 ) +
                ( color.B << 0 )
            );
        }

        internal static Color FormatColorFrom888( uint color )
        {
            return Color.FromArgb( ( int ) color );
        }

        #endregion
    }
}
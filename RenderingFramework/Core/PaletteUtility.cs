using System.Drawing;
using System.Drawing.Imaging;

namespace RenderingFramework.Core
{
    internal static class PaletteUtility
    {
        #region Definitions

        static readonly int[] BlueValues = { 0, 85, 170, 255 };
        static readonly int[] RedValues = { 0, 36, 73, 108, 144, 181, 216, 255 };
        static readonly int[] GreenValues = { 0, 36, 73, 108, 144, 181, 216, 255 };

        #endregion

        #region Fields

        static Color[] trueColorPalette;

        static Color[] TrueColorPalette
        {
            get
            {
                if( trueColorPalette == null )
                    trueColorPalette = GetTrueColorPalette();

                return trueColorPalette;
            }
        }

        #endregion

        #region Public Interface

        public static ColorPalette CreateTrueColorPalette( ColorPalette palette )
        {
            for( int index = 0; index < palette.Entries.Length; index++ )
                palette.Entries[ index ] = TrueColorPalette[ index ];

            return palette;
        }

        public static ColorPalette CreateMonochromePalette( ColorPalette palette )
        {
            for( int index = 0; index < palette.Entries.Length; index++ )
                palette.Entries[ index ] = Color.FromArgb( index, index, index );

            return palette;
        }

        #endregion

        #region Internal

        static Color[] GetTrueColorPalette()
        {
            Color[] palette = new Color[ 256 ];
            int colorMapIndex = 0;

            for( int redIndex = 0; redIndex < 8; redIndex++ )
            {
                for( int greenIndex = 0; greenIndex < 8; greenIndex++ )
                {
                    for( int blueIndex = 0; blueIndex < 4; blueIndex++ )
                    {
                        palette[ colorMapIndex ] = Color.FromArgb
                        (
                            RedValues[ redIndex ],
                            GreenValues[ greenIndex ],
                            BlueValues[ blueIndex ]
                        );

                        colorMapIndex++;
                    }
                }
            }

            return palette;
        }

        #endregion
    }
}

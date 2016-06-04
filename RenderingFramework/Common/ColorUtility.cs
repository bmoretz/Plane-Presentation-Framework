using System.Collections.Generic;
using System.Drawing;

namespace RenderingFramework.Common
{
    public static class ColorUtility
    {
        #region Definitions

        const int PaletteSize = 256;

        static readonly int[] BlueValues = { 0, 85, 170, 255 };
        static readonly int[] RedValues = { 0, 36, 73, 108, 144, 181, 216, 255 };
        static readonly int[] GreenValues = { 0, 36, 73, 108, 144, 181, 216, 255 };

        #endregion

        #region Public Interface

        public static Color[] CreateTrueColor()
        {
            Color[] palette = new Color[ PaletteSize ];
            int colorMapIndex = 0;

            for ( int redIndex = 0; redIndex < 8; redIndex++ )
            {
                for ( int greenIndex = 0; greenIndex < 8; greenIndex++ )
                {
                    for ( int blueIndex = 0; blueIndex < 4; blueIndex++ )
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

        public static Color[] CreateMonochrome()
        {
            return CreateGradient( PaletteSize, Color.Black, Color.White );
        }

        public static Color[] CreateGradient( int size, params Color[] colors )
        {
            List<Color> palette = new List<Color>( size );

            int colorSpan = colors.Length - 1;

            if ( colors.Length > 0 )
            {
                int lastPadding = size % colorSpan;

                int stepSize = size / colorSpan;

                for ( int index = 0; index < colorSpan; index++ )
                {
                    palette.AddRange
                    (
                        CreateGradient
                        (
                            colors[ index ],
                            colors[ index + 1 ],
                            index == colorSpan - 1 ?
                                stepSize + lastPadding : stepSize
                        )
                    );
                }
            }

            return palette.ToArray();
        }

        #endregion

        #region Internal

        static IEnumerable<Color> CreateGradient( Color start, Color end, int steps )
        {
            for ( int i = 0; i < steps; i++ )
            {
                yield return Color.FromArgb
                (
                    start.R + ( i * ( end.R - start.R ) / steps ),
                    start.G + ( i * ( end.G - start.G ) / steps ),
                    start.B + ( i * ( end.B - start.B ) / steps )
                );
            }
        }

        #endregion
    }
}

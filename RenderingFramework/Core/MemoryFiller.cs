using System;
using System.Runtime.InteropServices;

using Color = System.Drawing.Color;

namespace RenderingFramework.Core
{
    internal sealed unsafe class MemoryFiller
    {
        #region Constructor

        public MemoryFiller( SurfaceDescription surfaceDesc )
        {
            surface = surfaceDesc;
        }

        #endregion

        #region Fields
        
        SurfaceDescription surface;

        #endregion

        #region Public Interface

        public void Fill( Color color )
        {
            if ( color == Color.Black )
            {
                ClearBuffer();
            }
            else
            {
                switch( surface.PixelSize )
                {
                    case 1:
                    {
                        FillSurface332( color );
                    } break;

                    case 2:
                    {
                        FillSurface565( color );
                    } break;

                    case 4:
                    {
                        FillSurface888( color );
                    } break;

                    default:
                        break;
                }
            }
        }

        #endregion

        #region Internal

        void ClearBuffer()
        {
            byte[] colorData =
                new byte[ Math.Abs( surface.Stride * surface.PixelSize ) * surface.Height ];

            Marshal.Copy( colorData, 0, surface.Memory, colorData.Length );
        }

        void FillSurface332( Color color )
        {
            byte pixelData =
                ColorEncoder.Format332FromColor( color );

            ulong pixelOctet = BitConverter.ToUInt64
            (
                new byte[]
                {
                    pixelData, pixelData, pixelData, pixelData, 
                    pixelData, pixelData, pixelData, pixelData
                }, 0
            );

            for ( int index = 0; index < ( surface.Height * ( surface.Stride >> 3 ) ); index++ )
            {
                *( ( ( ulong* ) surface.Memory ) + index ) = pixelOctet;
            }
        }

        void FillSurface565( Color color )
        {
            ushort pixelData =
                ColorEncoder.Format565FromColor( color );

            byte[] pixelBytes = BitConverter.GetBytes( pixelData );

            ulong pixelQuad = BitConverter.ToUInt64
            (
                new byte[]
                {
                    pixelBytes[ 0 ], pixelBytes[ 1 ], pixelBytes[ 0 ], pixelBytes[ 1 ], 
                    pixelBytes[ 0 ], pixelBytes[ 1 ], pixelBytes[ 0 ], pixelBytes[ 1 ]
                }, 0
            );

            for ( int index = 0; index < ( surface.Height * ( surface.Stride >> 2 ) ); index++ )
            {
                *( ( ( ulong* ) surface.Memory ) + index ) = pixelQuad;
            }
        }

        void FillSurface888( Color color )
        {
            uint pixelData =
                ColorEncoder.Format888FromColor( color );

            byte[] pixelBytes = BitConverter.GetBytes( pixelData );

            ulong pixelQuad = BitConverter.ToUInt64
            (
                new byte[]
                {
                    pixelBytes[ 0 ], pixelBytes[ 1 ], pixelBytes[ 2 ], pixelBytes[ 3 ],
                    pixelBytes[ 0 ], pixelBytes[ 1 ], pixelBytes[ 2 ], pixelBytes[ 3 ]
                }, 0
            );

            for ( int index = 0; index < ( surface.Height * ( surface.Stride >> 1 ) ); index++ )
            {
                *( ( ( ulong* ) surface.Memory ) + index ) = pixelQuad;
            }
        }

        #endregion
    }
}

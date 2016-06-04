using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

using RenderingFramework.Common;
using RenderingFramework.GDI;
using RenderingFramework.Interfaces;
using System.Reflection;

namespace Gradient
{
    static class Gradient
    {
        #region Definitions

        const string ApplicationName = "Gradient";

        #endregion

        #region Fields

        static int PreviousClientSize;
        
        static int[] PixelBuffer;

        static Color[] ColorSet = new Color[] 
        {
            Color.Black, Color.Teal, Color.Turquoise, Color.White
        };

        #endregion

        #region Entry Point

        [STAThread]
        static int Main()
        {
            using ( RenderingMemory engine = new RenderingMemory( ApplicationName ) )
            {
                engine.SetFrameLimit( 60 );
                engine.SetRender( Render );

                engine.SetColorFormat( ColorFormat.True );
                engine.BackgroundColor = Color.Black;
                engine.ClearEveryFrame = false;

                engine.Start();
            }

            return 0;
        }

        #endregion

        #region Render

        static void Render( IRenderSurface display )
        {
            InitializeGradient( display );

            DisplayGradient( display );
        }

        #endregion

        #region Internal

        static void InitializeGradient( IRenderSurface display )
        {
            if ( PreviousClientSize != display.Stride + display.Height )
            {
                PreviousClientSize = display.Stride + display.Height;
                PixelBuffer = new int[ display.Height * display.Width ];

                Color[] gradient = ColorUtility.CreateGradient( display.Width, ColorSet );

                int[] initialLine = new int[ display.Stride ];

                for( int index = 0; index < display.Stride; index++ )
                {
                    Color current = gradient[ index ];

                    initialLine[ index ] = BitConverter.ToInt32
                    (
                        new byte[] { current.B, current.G, current.R, 0 }, 0
                    );
                }
                
                unsafe
                {
                    for( int line = 0; line < display.Height; line++ )
                    {
                        fixed ( int* ptr = &PixelBuffer[ display.Stride * line ] )
                        {
                            Marshal.Copy( initialLine, 0, ( IntPtr ) ptr, initialLine.Length );
                        }
                    }
                }
            }
        }

        static void DisplayGradient( IRenderSurface display )
        {
            Marshal.Copy( PixelBuffer, 0, display.Surface, PixelBuffer.Length );
        }

        #endregion
    }
}
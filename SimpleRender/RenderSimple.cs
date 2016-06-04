using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using RenderingFramework;

namespace RenderSimple
{
    static class RenderSimple
    {
        #region Definitions

        const string ApplicationName = "Render Simple";

        #endregion

        #region Fields

        static double position = 0;
        static Pen Outliner;

        #endregion

        #region Constructor

        static RenderSimple()
        {
            Outliner = new Pen( Brushes.Aqua, 4.5f );
        }

        #endregion

        #region Entry Point

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main()
        {
            using ( RenderingLight engine = new RenderingLight( ApplicationName ) )
            {
                engine.EnableFrameLimiter( true );
                engine.SetRender( Render );
                engine.BackgroundColor = Color.Black;
                engine.Start();
            }

            return 0;
        }

        #endregion

        #region Render

        static void Render( Graphics display )
        {
            int width = ( int ) display.VisibleClipBounds.Width;
            int height = ( int ) display.VisibleClipBounds.Height;

            double waveAmplitude = height * .125; // Wave Amplitude is 25% total screen height ( 12.5% above and below the origin )
            double waveFrequency = width * .025; // Wave Frequency is 25% of the screen width ( 4 total wave cycles, 2 full waves rendered )

            using ( GraphicsPath wavePath = new GraphicsPath( FillMode.Winding ) )
            {
                Point wavePoint;

                for ( int index = 0; index < width; index++ )
                {
                    double sineValue = Math.Sin( ( index - position ) / ( waveFrequency * ( Math.PI ) ) );
                    wavePoint = new Point() { X = index, Y = ( int ) ( waveAmplitude * sineValue ) + ( height / 2 ) };
                    wavePath.AddLine( wavePoint, wavePoint );
                }

                position += 5;

                wavePath.Widen( Outliner );

                display.DrawPath( Pens.DodgerBlue, wavePath );
            }
        }

        #endregion
    }
}
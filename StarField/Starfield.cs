using System;
using System.Collections.Generic;
using System.Drawing;

using RenderingFramework.Common;
using RenderingFramework.GDI;
using RenderingFramework.Interfaces;

using StarField.Types;

namespace StarField
{
    static class Starfield
    {
        #region Definitions

        const string ApplicationName = "Starfield";

        const int MaxStarCount = 1500;

        #endregion

        #region Constructor

        static Starfield()
        {
            InitializeStarfield();
        }

        #endregion

        #region Entry Point

        [STAThread]
        static int Main()
        {
            using ( RenderingMemory engine = new RenderingMemory( ApplicationName ) )
            {
                engine.SetFrameLimit( 60 );
                engine.SetRender( Render );

                engine.SetColorFormat( ColorFormat.Real );
                engine.Palette = ColorUtility.CreateMonochrome();
                engine.BackgroundColor = Color.Black;

                engine.ClearEveryFrame = true;

                engine.Start();
            }

            return 0;
        }

        #endregion

        #region Fields

        static List<Star> starfield;

        #endregion

        #region Render

        static void Render( IRenderSurface display )
        {
            int middleHeight = display.Height / 2;
            int middleWidth = display.Width / 2;

            foreach( Star star in starfield )
            {
                display.PlotPixelSafe
                (
                    new Point()
                    {
                        X = ( int ) ( ( star.X / star.Z ) + middleWidth ),
                        Y = ( int ) ( ( star.Y / star.Z ) + middleHeight )
                    },
                    star.Color
                );

                star.Move();
            }
        }

        #endregion

        #region Internal

        static void InitializeStarfield()
        {
            starfield = new List<Star>( MaxStarCount );

            for ( int index = 0; index < MaxStarCount; index++ )
            {
                starfield.Add
                (
                    new Star()
                );
            }
        }

        #endregion
    }
}
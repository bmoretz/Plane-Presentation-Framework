using System;
using System.Drawing;
using System.Drawing.Imaging;

using RenderingFramework.Core;
using RenderingFramework.Interfaces;

namespace RenderingFramework.GDI
{
    public sealed class RenderingMemory
        : RenderingGDI
    {
        #region Fields

        Action<IRenderSurface> render;

        #endregion

        #region Constructor

        public RenderingMemory( string applicationName )
            : base( applicationName )
        {
            base.SetRender( MemoryRender );
        }

        #endregion

        #region Public Interface

        public void SetRender( Action<IRenderSurface> routine )
        {
            render = routine;
        }

        #endregion

        #region Internal

        void MemoryRender( Bitmap buffer )
        {
            BitmapData bmpData = default( BitmapData );

            try
            {
                bmpData = buffer.LockBits
                (
                    new Rectangle()
                    {
                        Width = buffer.Width,
                        Height = buffer.Height
                    },
                    ImageLockMode.ReadWrite,
                    buffer.PixelFormat
                );

                SurfaceDescription surfaceDesc = 
                    new SurfaceDescription( bmpData.Width, bmpData.Height, pixelSize, ( ushort )( bmpData.Stride / pixelSize ), bmpData.Scan0 );

                if( ClearEveryFrame )
                {
                    new MemoryFiller( surfaceDesc ).Fill( BackgroundColor );
                }

                render( new DisplayController( surfaceDesc ) );
            } 
            finally
            {
                if( bmpData != null )
                    buffer.UnlockBits( bmpData );
            }
        }

        #endregion
    }
}

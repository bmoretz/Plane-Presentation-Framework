using System;
using System.Drawing;
using System.Drawing.Imaging;

using RenderingFramework.Core;
using RenderingFramework.GDI.Native;

namespace RenderingFramework.GDI
{
    public enum ColorFormat { Real, High, True }

    public abstract class RenderingGDI
        : RenderingCore, IDisposable
    {
        #region Fields

        public Color BackgroundColor = Color.Black;
        public bool ClearEveryFrame = false;
        public Color[] Palette;

        protected ColorFormat ColorFormat = ColorFormat.High;
        protected PixelFormat pixelFormat = PixelFormat.Format16bppRgb565;
        protected ushort pixelSize = 2;

        bool disposed = false;
        Action<Bitmap> render;
        Bitmap buffer;
        ColorPalette colorPalette;

        #endregion

        #region Constructor

        public RenderingGDI( string applicationName )
            : base( applicationName )
        { }

        #endregion

        #region Internals

        void CreateBuffer()
        {
            if( buffer != null )
            {
                buffer.Dispose();
                buffer = null;
            }

            if( FrameworkForm.ClientSize.Width == 0 || FrameworkForm.ClientSize.Height == 0 )
                return;

            buffer = new Bitmap( FrameworkForm.ClientRectangle.Width, FrameworkForm.ClientSize.Height, pixelFormat );

            if( pixelSize == 1 && Palette != null )
            {
                if( colorPalette == null )
                {
                    ColorPalette palette = buffer.Palette;

                    for ( int index = 0; index < palette.Entries.Length; index++ )
                        palette.Entries[ index ] = Palette[ index ];

                    colorPalette = palette;
                }

                buffer.Palette = colorPalette;
            }
        }

        #endregion

        #region Event Handlers

        protected override void CreateDevice()
        {
            CreateBuffer();
        }

        protected override void ResetDevice()
        {
            CreateBuffer();
        }
        
        #endregion

        #region Public Interface

        public void SetColorFormat( ColorFormat colorFormat )
        {
            switch( colorFormat )
            {
                case ColorFormat.Real:
                    { 
                        pixelFormat = PixelFormat.Format8bppIndexed;
                        pixelSize = 1;
                    } break;

                case ColorFormat.High:
                    {
                        pixelFormat = PixelFormat.Format16bppRgb565;
                        pixelSize = 2;
                    } break;

                case ColorFormat.True: 
                    {
                        pixelFormat = PixelFormat.Format32bppRgb;
                        pixelSize = 4;
                    } break;
            }

            ColorFormat = colorFormat;
        }

        #endregion

        #region Internal

        protected override void Render()
        {
            if( buffer == null ) return;

            using( Graphics display = Graphics.FromHwnd( FrameworkForm.Handle ) )
            {
                IntPtr hDC = display.GetHdc();
                IntPtr hMemDC = GDIMethods.CreateCompatibleDC( hDC );
                IntPtr bufferDC = buffer.GetHbitmap();

                GDIMethods.SelectObject( hMemDC, bufferDC );

                render( buffer );

                GDIMethods.BitBlt
                (
                    hDC,
                    0, 0,
                    FrameworkForm.Width, FrameworkForm.Height,
                    hMemDC,
                    0, 0,
                    TernaryRasterOperations.SRCCOPY
                );

                GDIMethods.DeleteObject( bufferDC );
                GDIMethods.DeleteObject( hMemDC );

                display.ReleaseHdc( hDC );
            }
        }

        protected void SetRender( Action<Bitmap> routine )
        {
            render = routine;
        }
 
        #endregion

        #region Dispose

        public override void Dispose( bool disposing )
        {
            if( !disposing )
            {
                if( !disposed )
                {
                    if( buffer != null )
                    {
                        buffer.Dispose();
                    }
                }

                disposed = true;
            }

            base.Dispose( disposing );
        }

        #endregion
    }
}
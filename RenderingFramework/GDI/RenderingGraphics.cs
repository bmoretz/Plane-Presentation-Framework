using System;
using System.Drawing;

namespace RenderingFramework.GDI
{
    public sealed class RenderingGraphics :
        RenderingGDI
    {
        #region Fields

        bool disposed = false;
        Action<Graphics> render;
        Brush backgroundBrush;

        #endregion

        #region Constructor

        public RenderingGraphics( string applicationName )
            : base( applicationName )
        {
            base.SetRender( GraphicsRender );
        }

        #endregion

        #region Public Interface

        public void SetRender( Action<Graphics> routine )
        {
            render = routine;
        }

        #endregion

        #region Internal

        protected override void CreateDevice()
        {
            if ( backgroundBrush != null )
                backgroundBrush.Dispose();

            backgroundBrush =
                new SolidBrush( BackgroundColor );

            base.CreateDevice();
        }

        void GraphicsRender( Bitmap buffer )
        {
            using( Graphics screen = Graphics.FromImage( buffer ) )
            {
                if ( ClearEveryFrame )
                {
                    screen.FillRectangle( backgroundBrush, 0, 0, buffer.Width, buffer.Height );
                }

                render( screen );
            }
        }

        #endregion

        #region Dispose

        public override void Dispose( bool disposing )
        {
            if ( !disposing )
            {
                if ( !disposed )
                {
                    if ( backgroundBrush != null )
                    {
                        backgroundBrush.Dispose();
                    }
                }

                disposed = true;
            }

            base.Dispose( disposing );
        }

        #endregion
    }
}

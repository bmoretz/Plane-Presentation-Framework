using System;
using System.Drawing;

using RenderingFramework.Core;

namespace RenderingFramework
{
    public sealed class RenderingLight
        : RenderingCore, IDisposable
    {
        #region Fields

        bool disposed = false;

        Action<Graphics> render;

        #endregion

        #region Constructor

        public RenderingLight( string applicationName )
            : base( applicationName ) { }

        #endregion

        #region Properties

        public Color BackgroundColor = Color.Blue;

        #endregion

        #region Event Handlers

        protected override void CreateDevice() {}

        protected override void ResetDevice() {}
        
        #endregion

        #region Public Interface

        protected override void Render()
        {
            using( Graphics display = Graphics.FromHwnd( FrameworkForm.Handle ) )
            {
                using( Brush background = new SolidBrush( BackgroundColor ) )
                {
                    display.FillRectangle
                    (
                        background, 
                        new Rectangle() { X = 0, Y = 0, Width = FrameworkForm.Width, Height = FrameworkForm.Height } 
                    );

                    render.Invoke( display );
                }
            }
        }

        public void SetRender( Action<Graphics> routine )
        {
            render = routine;
        }
 
        #endregion

        #region Dispose

        public override void Dispose( bool disposing )
        {
            if( !disposing )
            {
                if( !disposed ) { }

                disposed = true;
            }

            base.Dispose( disposing );
        }

        #endregion
    }
}
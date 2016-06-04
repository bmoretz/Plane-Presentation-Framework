using System;
using System.Drawing;
using System.Windows.Forms;

using RenderingFramework.Core;
using RenderingFramework.Interfaces;
using RenderingFramework.Native;

namespace RenderingFramework.Forms
{
    using FormMessage = System.Windows.Forms.Message;
    using WindowMessage = Native.Message;
    using WM = Native.WindowMessage;

    internal class GraphicsForm
        : Form
    {
        #region Definitions

        protected static Color DefaultWindowColor = Color.Black;

        #endregion

        #region Properties

        WindowMessage msg;
        /// <summary>Checks to see if the application is still idle</summary>
        bool CanRender
        {
            get
            {
                return !NativeMethods.PeekMessage( out msg, IntPtr.Zero, 0, 0, 0 );
            }
        }

        #endregion

        #region Event Callbacks

        public Action ClientResizeEvent;

        #endregion

        #region Fields

        IRenderCallback RenderCallback;
        EventHandler onApplicationIdleEvent;

        #endregion

        #region Constructor

        public GraphicsForm( string applicationName, int windowWidth, int windowHeight, string windowIcon )
        {
            ConfigureWindow( this, applicationName, windowWidth, windowHeight, windowIcon );

            onApplicationIdleEvent = 
                new EventHandler( OnApplicationIdle );
        }

        #endregion

        #region Events

        protected void OnApplicationIdle( object sender, EventArgs e )
        {
            while ( CanRender )
            {
                RenderCallback.Render();
            }
        }

        protected override void OnClientSizeChanged( EventArgs e )
        {
            if ( ClientResizeEvent != null )
            {
                ClientResizeEvent.Invoke();
            }
        }

        protected override void OnKeyDown( KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Escape )
            {
                this.Close();
            }
        }

        protected override void OnLoad( EventArgs e )
        {
            Application.Idle += onApplicationIdleEvent;

            base.OnLoad( e );
        }

        protected override void OnClosed( EventArgs e )
        {
            Application.Idle -= onApplicationIdleEvent;

            base.OnClosed( e );
        }

        protected override void WndProc( ref FormMessage m )
        {
            switch ( m.Msg )
            {
                case ( int ) WM.NcHitTest:
                    {
                        base.WndProc( ref m );

                        if ( m.Result == ( ( IntPtr ) HitTest.Client ) )
                        {
                            m.Result = ( IntPtr ) HitTest.Caption;
                        }

                    } break;

                default:
                    {
                        base.WndProc( ref m );
                    } break;
            }
        }

        #endregion

        #region Form Related

        public void ConfigureWindow( Form window, string applicationName,
            int windowWidth, int windowHeight, string windowIcon )
        {
            window.Name = applicationName;
            window.Text = applicationName;
            window.BackColor = DefaultWindowColor;
            window.StartPosition = FormStartPosition.CenterScreen;
            window.FormBorderStyle = FormBorderStyle.Sizable;
            window.ClientSize = new Size( windowWidth, windowHeight );
            window.Icon = ResourceUtility.GetIconResource( windowIcon );
        }

        public void BeginProcessing( IRenderCallback renderCallback )
        {
            RenderCallback = renderCallback;
 
            Application.Run( this );
        }

        #endregion
    }
}
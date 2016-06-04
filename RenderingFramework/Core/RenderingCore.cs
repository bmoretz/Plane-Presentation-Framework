using System;
using System.Windows.Forms;

using RenderingFramework.Forms;
using RenderingFramework.Interfaces;

namespace RenderingFramework.Core
{
    abstract public class RenderingCore :
        IRenderCallback, IDisposable
    {
        #region Definitions

        protected const int DefaultWindowWidth = 640;
        protected const int DefaultWindowHeight = 480;

        protected const string DefaultWindowIcon = "default.ico";

        #endregion

        #region Fields

        GraphicsForm renderingWnd;
        FrameController frameController;

        bool disposed = false;
        string windowText;

        #endregion

        #region Properties

        protected Form FrameworkForm
        {
            get
            {
                return renderingWnd;
            }
        }

        protected int FramesPerSecond
        {
            get
            {
                return frameController.FramesPerSecond;
            }
        }

        #endregion

        #region Constructor

        public RenderingCore( string applicationName, 
            int windowWidth = DefaultWindowWidth,
            int windowHeight = DefaultWindowHeight,
            string windowIcon = DefaultWindowIcon )
        {
            windowText = applicationName;

            CreateWindow( applicationName, windowWidth, windowHeight, windowIcon );

            FrameworkForm.KeyPress += new KeyPressEventHandler( KeyPressEvent );
            frameController = new FrameController();
        }
        
        #endregion

        #region Internal

        void CreateWindow( string applicationName, int windowWidth, int windowHeight, string windowIcon )
        {
            renderingWnd = new GraphicsForm( applicationName, windowWidth, windowHeight, windowIcon );

            renderingWnd.ClientResizeEvent += ResetDevice;
        }

        protected void DisplayFrameRate()
        {            
            renderingWnd.Text = String.Concat
            (
                windowText, " - FPS: ", 
                frameController.FramesPerSecond
            );
        }

        #endregion

        #region Event Hooks

        abstract protected void CreateDevice();
        abstract protected void ResetDevice();

        void KeyPressEvent( Object sender, KeyPressEventArgs e )
        {
            switch ( e.KeyChar )
            {
                case '+':
                    {
                        frameController.FrameLimit =
                            ++frameController.FrameLimit;
                    } break;

                case '-':
                    {
                        frameController.FrameLimit =
                            --frameController.FrameLimit;
                    } break;

                case ( char ) Keys.Enter:
                    {
                        frameController.Enabled = 
                            !frameController.Enabled;
                    } break;

                default:
                    break;
            }
        }

        #endregion

        #region Public Interface

        public void SetFrameLimit( uint frameLimit )
        {
            frameController.FrameLimit = frameLimit;
        }

        public void EnableFrameLimiter( bool enabled )
        {
            frameController.Enabled = enabled;
        }

        public virtual void Start()
        {
            CreateDevice();

            renderingWnd.BeginProcessing( this );
            frameController.Start();
        }

        #endregion

        #region IRenderCallback Members

        protected abstract void Render();

        void IRenderCallback.Render()
        {
            frameController.PreRender();

            Render();

            frameController.PostRender();

            DisplayFrameRate();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public virtual void Dispose( bool disposing )
        {
            if( !disposed )
            {
                if( !disposing )
                {
                    frameController.Stop();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        #endregion
    }
}
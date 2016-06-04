using System;
using System.Drawing;

using DirectDrawWrapper;

using RenderingFramework.Core;
using RenderingFramework.Interfaces;

namespace RenderingFramework.DirectDraw
{
    public enum ColorFormat { Real = 1, High = 2, True = 4 }

    public unsafe class RenderingDirectDraw
        : RenderingCore
    {
        #region Definitions

        const string DirectXIcon = "DirectX.ico";

        #endregion

        #region Constructor

        public RenderingDirectDraw( string applicationName )
            : base( applicationName: applicationName, windowIcon: DirectXIcon )
        {
            videoMemory = new VideoMemory( FrameworkForm.Handle );
        }

        #endregion

        #region Fields

        public bool ClearEveryFrame = false;
        public Color BackgroundColor = default( Color );

        bool disposed = false;
        ColorFormat colorFormat = ColorFormat.High;

        VideoMemory videoMemory;
        Action<IRenderSurface> render;

        #endregion

        #region Public Interface

        public void SetRender( Action<IRenderSurface> routine )
        {
            render = routine;
        }

        public void SetColorFormat( ColorFormat format )
        {
            videoMemory.SetColorFormat( ( PixelSize ) format );

            colorFormat = format;
        }

        #endregion

        #region Overrides

        public override void Start()
        {
            switch ( colorFormat )
            {
                case ColorFormat.Real:
                    {
                        videoMemory.SetBackgroundColor( ColorEncoder.Format332FromColor( BackgroundColor ) );
                    } break;

                case ColorFormat.High:
                    {
                        videoMemory.SetBackgroundColor( ColorEncoder.Format565FromColor( BackgroundColor ) );
                    } break;

                case ColorFormat.True:
                    {
                        videoMemory.SetBackgroundColor( ColorEncoder.Format888FromColor( BackgroundColor ) );
                    } break;

                default:
                    break;
            }

            base.Start();
        }

        protected override void CreateDevice()
        {
            videoMemory.CreateDevice();
        }

        protected override void ResetDevice()
        {
            videoMemory.ResetDevice();
        }

        protected override void Render()
        {
            if( videoMemory.ReadyFrame( ClearEveryFrame ) )
            {   
                render
                (
                    new DisplayController( GetSurfaceDescription( videoMemory ) )
                );

                videoMemory.RenderFrame();
            }
        }

        SurfaceDescription GetSurfaceDescription( VideoMemory videoMemory )
        {
            return new SurfaceDescription
            (
                videoMemory.Width,
                videoMemory.Height,
                ( ushort ) colorFormat,
                ( ushort ) ( videoMemory.Stride * ( int ) colorFormat ), // Adjust for Bytes
                videoMemory.VideoMemoryPtr
            );
        }

        #endregion

        #region Dispose

        public override void Dispose( bool disposing )
        {
            if ( !disposing )
            {
                if ( !disposed )
                {
                    if ( videoMemory != null )
                    {
                        videoMemory.Release();
                    }
                }

                disposed = true;
            }

            base.Dispose( disposing );
        }

        #endregion
    }
}
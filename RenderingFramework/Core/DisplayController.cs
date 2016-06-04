using System;

using Point = System.Drawing.Point;
using Color = System.Drawing.Color;

using RenderingFramework.Interfaces;

namespace RenderingFramework.Core
{
    internal unsafe sealed class DisplayController
        : IRenderSurface
    {
        #region Constructor

        public DisplayController( SurfaceDescription surfaceDesc )
        {
            surface = surfaceDesc;

            if ( PreviousHeight != surface.Height || PreviousStride != surface.Stride )
            {
                PreviousHeight = surface.Height; PreviousStride = surface.Stride;

                YValues = new int[ surface.Height ];

                for ( int y = 0; y < surface.Height; y++ )
                {
                    YValues[ y ] = y * surface.Stride;
                }
            }

            if ( !Initialized )
            {
                Initialize( surface.PixelSize );
            }
        }

        #endregion

        #region Fields

        static int PreviousHeight = 0, PreviousStride = 0;
        static int[] YValues;
        static bool Initialized = false;

        SurfaceDescription surface;

        Action<Point, Color> PlotPixel;
        Action<Point, Color> PlotPixelSafe;

        Func<Point, Color> GetPixel;
        Func<Point, Color> GetPixelSafe;

        #endregion

        #region IRenderSurface Implementation

        void IRenderSurface.PlotPixel( Point position, Color color )
        {
            PlotPixel( position, color );
        }

        void IRenderSurface.PlotPixelSafe( Point position, Color color )
        {
            PlotPixelSafe( position, color );
        }

        Color IRenderSurface.GetPixel( Point position )
        {
            return GetPixel( position );
        }

        Color IRenderSurface.GetPixelSafe( Point position )
        {
            return GetPixelSafe( position );
        }

        int IRenderSurface.Height
        {
            get { return surface.Height; }
        }

        int IRenderSurface.Width
        {
            get { return surface.Width; }
        }

        IntPtr IRenderSurface.Surface
        {
            get { return surface.Memory; }
        }

        int IRenderSurface.Stride
        {
            get { return surface.Stride; }
        }

        int[] IRenderSurface.YValues
        {
            get { return YValues; }
        }

        #endregion

        #region Internal

        void Initialize( uint pixelSize )
        {
            switch ( pixelSize )
            {
                case 1:
                    {
                        PlotPixel = PlotPixelFast332;
                        PlotPixelSafe = PlotPixelSafe332;

                        GetPixel = GetPixelFast332;
                        GetPixelSafe = GetPixelSafe332;
                    } break;

                case 2:
                    {
                        PlotPixel = PlotPixelFast565;
                        PlotPixelSafe = PlotPixelSafe565;

                        GetPixel = GetPixelSafe565;
                        GetPixelSafe = GetPixelSafe565;
                    } break;

                case 4:
                    {
                        PlotPixel = PlotPixelFast888;
                        PlotPixelSafe = PlotPixelSafe888;

                        GetPixel = GetPixelSafe888;
                        GetPixelSafe = GetPixelSafe888;
                    } break;

                default:
                    throw new NotSupportedException();
            }
        }

        bool IsValidPosition( Point position )
        {
            return ( ( position.X > 0 && position.X < surface.Width ) && ( position.Y > 0 && position.Y < surface.Height ) );
        }

        #endregion

        #region 8 BPP

        void PlotPixelFast332( Point position, Color color )
        {
            *( ( byte * )( surface.Memory ) + YValues[ position.Y ] + position.X ) =
               ColorEncoder.Format332FromColor( color );
        }

        void PlotPixelSafe332( Point position, Color color )
        {
            if ( IsValidPosition( position ) )
            {
                PlotPixelFast332( position, color );
            }
        }

        Color GetPixelFast332 (Point position )
        {
            return ColorEncoder.FormatColorFrom332
            (
                *( ( byte * )( surface.Memory ) + YValues[ position.Y ] + position.X )
            );
        }

        Color GetPixelSafe332( Point position )
        {
            return IsValidPosition( position ) ?
                GetPixelFast332( position ) :
                default( Color );
        }

        #endregion

        #region 16 BPP

        public void PlotPixelFast565( Point position, Color color )
        {
            *( ( ushort * )( surface.Memory ) + YValues[ position.Y ] + position.X ) =
                ColorEncoder.Format565FromColor( color );
        }

        public void PlotPixelSafe565( Point position, Color color )
        {
            if( IsValidPosition( position ) )
            {
                PlotPixelFast565( position, color );
            }
        }

        public Color GetPixelFast565( Point position )
        {
            return ColorEncoder.FormatColorFrom565
            (
                *( ( ushort* )( surface.Memory ) + YValues[ position.Y ] + position.X )
            );
        }

        public Color GetPixelSafe565( Point position )
        {
            return IsValidPosition( position ) ?
                GetPixelFast565( position ) :
                default( Color );
        }

        #endregion

        #region 32 BPP

        public void PlotPixelFast888( Point position, Color color )
        {
            *( ( uint* )( surface.Memory ) + YValues[ position.Y ] + position.X ) =
                ColorEncoder.Format888FromColor( color );
        }

        public void PlotPixelSafe888(Point position, Color color)
        {
            if( IsValidPosition( position ) )
            {
                PlotPixelFast888( position, color );
            }
        }

        public Color GetPixelFast888(Point position)
        {
            return ColorEncoder.FormatColorFrom888
            (
                *( ( uint* )( surface.Memory ) + YValues[ position.Y ] + position.X )
            );
        }

        public Color GetPixelSafe888(Point position)
        {
            return IsValidPosition( position ) ?
                GetPixelFast888( position ) :
                default( Color );
        }

        #endregion
    }
}
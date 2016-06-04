using System;
using System.Drawing;

namespace RenderingFramework.Interfaces
{
    public interface IRenderSurface
    {
        void PlotPixel( Point position, Color color );
        void PlotPixelSafe( Point position, Color color );

        Color GetPixel( Point position );
        Color GetPixelSafe( Point position );

        int Width { get; }
        int Height { get; }

        IntPtr Surface { get; }
        int Stride { get; }
        int[] YValues { get; }
    }
}
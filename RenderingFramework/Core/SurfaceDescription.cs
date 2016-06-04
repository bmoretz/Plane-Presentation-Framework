using System;

namespace RenderingFramework.Core
{
    internal struct SurfaceDescription
    {
        #region Constructor

        public SurfaceDescription( int width, int height, ushort pixelSize, ushort stride, IntPtr memory )
        {
            Width = width;
            Height = height;

            PixelSize = pixelSize;
            Stride = stride;

            Memory = memory;
        }

        #endregion

        #region Fields

        public readonly int Width;
        public readonly int Height;

        public readonly ushort Stride;
        public readonly ushort PixelSize;

        public readonly IntPtr Memory;

        #endregion
    }
}

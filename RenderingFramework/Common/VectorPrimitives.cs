using System;
using System.Collections.Generic;
using System.Drawing;

namespace RenderingFramework.Common
{
    public static class VectorPrimitives
    {
        #region Shapes

        public static IEnumerable<Point> RenderLine( Point begin, Point end )
        {
            int x0 = begin.X, y0 = begin.Y;
            int x1 = end.X, y1 = end.Y;

            bool steep = Math.Abs( y1 - y0 ) > Math.Abs( x1 - x0 );

            if ( steep )
            {
                Swap<int>( ref x0, ref y0 );
                Swap<int>( ref x1, ref y1 );

                yield return new Point( y0, x0 );
            }

            int deltax = Math.Abs( x1 - x0 );
            int deltay = Math.Abs( y1 - y0 );

            int error = 0, x = x0, y = y0;
            int xstep = x0 < x1 ? 1 : -1, ystep = y0 < y1 ? 1 : -1;

            while ( x != x1 )
            {
                x += xstep;
                error += deltay;

                if ( ( error * 2 ) > deltax )
                {
                    y += ystep;
                    error -= deltax;
                }

                yield return steep ? new Point( y, x ) : new Point( x, y );
            }
        }

        #endregion

        #region Internal

        static void Swap<T>( ref T lhs, ref T rhs )
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        #endregion
    }
}

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RenderingFramework.Core
{
    public static class ResourceUtility
    {
        #region Resource Related

        public static Bitmap GetImageResource( string resourceName, 
            Assembly assembly = null )
        {
            Bitmap bitmapResource = default( Bitmap );

            using ( Stream resourceStream = GetEmbeddedResourceStream( resourceName, assembly ) )
            {
                if ( resourceStream.CanRead )
                {
                    bitmapResource = new Bitmap( resourceStream );
                }
            }

            return bitmapResource;
        }

        public static Icon GetIconResource( string resourceName,
           Assembly assembly = null )
        {
            Icon iconResource = default( Icon );

            using ( Stream resourceStream = GetEmbeddedResourceStream( resourceName, assembly ) )
            {
                if ( resourceStream.CanRead )
                {
                    iconResource = new Icon( resourceStream );
                }
            }

            return iconResource;
        }

        #endregion

        #region Internal

        static Stream GetEmbeddedResourceStream( string resourceName, Assembly assembly = null )
        {
            Assembly current = assembly ?? Assembly.GetExecutingAssembly();

            return current.GetManifestResourceStream
            (
                current.GetManifestResourceNames().First
                (
                    resource => resource.EndsWith
                        ( resourceName, StringComparison.InvariantCultureIgnoreCase )
                )
            );
        }

        #endregion
    }
}

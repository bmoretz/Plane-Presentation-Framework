using System;
using System.Drawing;

namespace StarField.Types
{
    class Star
    {
        #region Definitions

        const Single Speed = 0.125f;

        const int SpreadX = 1500;
        const int SpreadY = 1500;
        const int SpreadZ = 15;

        #endregion

        #region Fields

        static Random randomizer;

        #endregion

        #region Constructors

        static Star()
        {
            randomizer =
                new Random( ( int ) DateTime.Now.Ticks );
        }

        public Star()
        {
            Initialize();
        }

        #endregion

        #region Fields

        double x, y, z;

        public int X { get { return ( int ) x; } }
        public int Y { get { return ( int ) y; } }
        public int Z { get { return ( int ) Math.Ceiling( z ); } }
        
        public Color Color 
        { 
            get 
            {
                int starColor = 
                    ( int ) Math.Ceiling( ( 1 - ( z / SpreadZ ) ) * 255 );

                return Color.FromArgb( starColor, starColor, starColor );
            } 
        }

        #endregion

        #region Public Interface

        public void Move()
        {
            z -= Speed;

            if( z <= 0 )
            {
                Initialize();
            }
        }

        #endregion

        #region Internal

        void Initialize()
        {
            x = ( randomizer.NextDouble() - .5f ) * SpreadX;
            y = ( randomizer.NextDouble() - .5f ) * SpreadY;
            z = ( randomizer.NextDouble() + .000000001f ) * SpreadZ;
        }

        #endregion
    }
}
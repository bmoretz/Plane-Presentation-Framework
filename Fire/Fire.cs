using System;
using System.Drawing;

using RenderingFramework.Common;
using RenderingFramework.DirectDraw;
using RenderingFramework.Interfaces;

namespace Fire
{
    static unsafe class Fire
    {
        #region Definitions

        const string ApplicationName = "Fire";

        const int Width = 640, Height = 480;
        const int ColorDepth = 256;
        
        const int AvgFlameWidth = 35;
        const int MaxFlameHeight = 330;
        const int FlameChance = 15;
        const int MinFlameIntensity = 225;
        const int MaxFlameIntensity = 255;

        const int CoalHeight = 2;
        const int MinCoalIntensity = 50;
        const int MaxCoalIntensity = 125;

        #endregion

        #region Fields

        static readonly Color[] Palette = ColorUtility.CreateGradient( ColorDepth, Color.Black, Color.DarkRed, Color.Red, Color.Yellow, Color.White );
        static readonly Random Randomizer = new Random();

        static byte[] FireBuffer;
        static int FireBufferHeight;
        static int FireBufferWidth;

        static int PreviousClientSize;

        #endregion

        #region Constructor

        static Fire()
        {
            CreateBuffer( Width, Height );
        }

        #endregion

        #region Entry Point

        [STAThread]
        static int Main()
        {
            using ( RenderingDirectDraw engine = new RenderingDirectDraw( ApplicationName ) )
            {
                engine.SetFrameLimit( 60 );
                engine.SetRender( Render );
                engine.SetColorFormat( ColorFormat.True );
                engine.ClearEveryFrame = false;
                engine.BackgroundColor = Color.Black;

                engine.Start();
            }

            return 0;
        }

        #endregion

        #region Render

        static void CreateBuffer(int width, int height)
        {
            FireBufferHeight = height > MaxFlameHeight ? MaxFlameHeight : height;
            FireBufferWidth = width;

            FireBuffer = new byte[ FireBufferWidth * FireBufferHeight ];
        }

        static void Render( IRenderSurface display )
        {
            EnsureCorrectBufferSize( display );
               
            GenerateCoalBed( display );

            Flame( display );

            DrawFire( display );
        }

        static void EnsureCorrectBufferSize( IRenderSurface display )
        {
            int clientSize = display.Stride + display.Height;

            if ( PreviousClientSize != clientSize )
            {
                CreateBuffer( display.Stride, display.Height );

                PreviousClientSize = clientSize;
            }
        }

        static void GenerateCoalBed( IRenderSurface display )
        {
            int coalStart = display.YValues[ FireBufferHeight - CoalHeight - 1 ];
            int coalEnd = display.YValues[ FireBufferHeight - 1 ] + FireBufferWidth;
            
            int position = coalStart;

            while( position != coalEnd )
            {
                if( position >= FireBuffer.Length )
                    break;

                FireBuffer[ position ] = ( byte ) Randomizer.Next( MinCoalIntensity, MaxCoalIntensity );

                position++;
            }

            position = 0;

            while( position < FireBufferWidth )
            {
                if( Randomizer.Next( 0, 100 ) < FlameChance )
                {
                    int flameWidth = Randomizer.Next( 1, AvgFlameWidth );
                    int offset = coalStart + position;

                    for( int i = 0; i < flameWidth; i++ )
                    {
                        if( position > FireBufferWidth || offset >= FireBuffer.Length )
                            break;
                        
                        FireBuffer[ offset + position ] = ( byte ) Randomizer.Next( MinFlameIntensity, MaxFlameIntensity );

                        position++;
                    }
                }

                position++;
            }
        }

        static void Flame( IRenderSurface display )
        {
            byte p1, p2, p3, p4, p5, p6, p7;
            int u, d, l, r;

            for( int x = 0; x < FireBufferWidth; x++ )
            {
                l = x == 0 ? 0 : x - 1;
                r = x == FireBufferWidth - 1 ? FireBufferWidth - 1 : x + 1;

                for ( int y = 0; y < FireBufferHeight - 1; y++ )
                {
                    u = y == 0 ? 0 : y - 1;
                    d = y == FireBufferHeight - 1 ? FireBufferHeight : y + 1;

                    p1 = FireBuffer[ display.YValues[ u ] + l ];
                    p2 = FireBuffer[ display.YValues[ y ] + l ];
                    p3 = FireBuffer[ display.YValues[ d ] + l ];

                    p4 = FireBuffer[ display.YValues[ d ] + x ];

                    p5 = FireBuffer[ display.YValues[ u ] + r ];
                    p6 = FireBuffer[ display.YValues[ y ] + r ];
                    p7 = FireBuffer[ display.YValues[ d ] + r ];

                    FireBuffer[ display.YValues[ u ] + x ] = ( byte )( ( p1 + p2 + +p3 + p4 + p5 + p6 + p7 ) / 7 );
                }
            }
        }

        static void DrawFire( IRenderSurface display )
        {
            int height = display.Height >= FireBufferHeight ? 
                    FireBufferHeight : FireBufferHeight - display.Height, 
                start = display.Height - height,
                position = 0;
            
            byte* dest; int offset; Color pixel;

            for( int row = start; row < ( display.Height - CoalHeight ); row++ )
            {
                offset = display.YValues[ row + CoalHeight ];

                dest = ( byte* ) ( display.Surface ) + ( offset << 2 );

                for ( int x = 0; x < display.Stride; x++ )
                {
                    pixel = Palette[ FireBuffer[ display.YValues[ position ] + x ] ];

                    dest[ 0 ] = pixel.B;
                    dest[ 1 ] = pixel.G;
                    dest[ 2 ] = pixel.R;

                    dest += 4;
                }

                position++;
            }
        }

        #endregion
    }
}
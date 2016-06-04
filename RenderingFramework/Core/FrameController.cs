using System.Threading;

using RenderingFramework.Native;

namespace RenderingFramework.Core
{
    internal sealed class FrameController
    {
        #region Fields

        static long CpuFrequency, SleepThreashold;
        static uint RefInstance = 0;

        uint instanceId;
        long renderStart, periodStart, desiredDelta;
        int framesRendered;
        
        #endregion

        #region Constructor

        static FrameController()
        {
            NativeMethods.QueryPerformanceFrequency( ref CpuFrequency );
            SleepThreashold = CpuFrequency >> 8;
        }

        public FrameController( uint limit = 0 )
        {
            instanceId = RefInstance++;

            FrameLimit = limit;
        }

        #endregion

        #region Properties

        int framesPerSecond;
        public int FramesPerSecond
        {
            get
            {
                return framesPerSecond;
            }
        }

        uint frameLimit = 30;
        public uint FrameLimit
        {
            get { return frameLimit; }
            set { if( value > 0 ) frameLimit = value; { desiredDelta = CpuFrequency / frameLimit; } }
        }

        public bool Enabled { get; set; }

        #endregion

        #region Public Interface

        public void Start()
        {
            NativeMethods.timeBeginPeriod( instanceId );
        }

        public void Stop()
        {
            NativeMethods.timeEndPeriod( instanceId );
        }

        public void PreRender()
        {
            NativeMethods.QueryPerformanceCounter( ref renderStart );

            if ( periodStart == 0 )
                periodStart = renderStart;
        }

        public void PostRender()
        {
            long renderEnd = default( long );

            NativeMethods.QueryPerformanceCounter( ref renderEnd );

            CalculateFPS( renderEnd );

            LimitFPS( renderEnd - renderStart );
        }

        #endregion

        #region Internals

        void LimitFPS( long renderTime )
        {
            if ( !Enabled )
                return;

            if ( desiredDelta > renderTime )
            {
                long timeToWait = desiredDelta - renderTime, startTicks = 0, currentTicks = 0, elapsedTicks;

                NativeMethods.QueryPerformanceCounter( ref startTicks );

                for ( ; ; )
                {
                    NativeMethods.QueryPerformanceCounter( ref currentTicks );

                    elapsedTicks = currentTicks - startTicks;

                    if ( elapsedTicks >= timeToWait )
                        break;

                    Thread.Sleep( elapsedTicks > SleepThreashold ? 0 : 1 );
                }
            }
        }

        void CalculateFPS( long renderEnd )
        {
            if ( ( renderEnd - periodStart ) >= CpuFrequency )
            {
                periodStart = renderEnd;
                framesPerSecond = framesRendered;
                framesRendered = 1;
            }
            else
            {
                framesRendered++;
            }
        }

        #endregion
    }
}
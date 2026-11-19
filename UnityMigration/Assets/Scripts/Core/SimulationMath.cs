using System;

namespace FatesPassage.Core
{
    /// <summary>Unity-independent tick math, usable in EditMode tests.</summary>
    public static class SimulationMath
    {
        public static int CountTicks(ref double accumulator, double deltaSeconds,
            int ticksPerSecond, int maxTicksPerFrame)
        {
            if (ticksPerSecond <= 0) throw new ArgumentOutOfRangeException(nameof(ticksPerSecond));
            if (maxTicksPerFrame <= 0) throw new ArgumentOutOfRangeException(nameof(maxTicksPerFrame));
            if (double.IsNaN(deltaSeconds) || double.IsInfinity(deltaSeconds) || deltaSeconds < 0)
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            double step = 1.0 / ticksPerSecond;
            accumulator = Math.Min(accumulator + deltaSeconds, step * maxTicksPerFrame);
            int ticks = 0;
            // Tiny tolerance prevents double-rounding from dropping a tick at exact boundaries.
            while (ticks < maxTicksPerFrame && accumulator + step * 1e-9 >= step)
            {
                ticks++;
                accumulator = Math.Max(0, accumulator - step);
            }
            return ticks;
        }
    }
}

using System;
using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Bounded fixed-rate simulation event source, independent of render FPS.</summary>
    public sealed class FixedTickClock : MonoBehaviour
    {
        [SerializeField, Range(10, 120)] private int ticksPerSecond = 60;
        [SerializeField, Range(1, 10)] private int maxTicksPerFrame = 4;
        public event Action<float> Tick;
        private double accumulator;
        public int TicksPerSecond => ticksPerSecond;

        private void Update()
        {
            int count = SimulationMath.CountTicks(ref accumulator,
                Time.unscaledDeltaTime, ticksPerSecond, maxTicksPerFrame);
            float step = 1f / ticksPerSecond;
            for (int i = 0; i < count; i++) Tick?.Invoke(step);
        }

        private void OnDisable() => accumulator = 0;
    }
}

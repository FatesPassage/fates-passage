using System;
using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Frame-rate-independent simulation tick scheduler.</summary>
    public sealed class FixedTickClock : MonoBehaviour
    {
        [SerializeField, Range(10, 120)] private int ticksPerSecond = 60;
        [SerializeField, Range(1, 10)] private int maxTicksPerFrame = 4;
        public event Action<float> Tick;
        private double accumulator;
        public int TicksPerSecond => ticksPerSecond;

        private void Update()
        {
            double step = 1.0 / ticksPerSecond;
            accumulator = Math.Min(accumulator + Time.unscaledDeltaTime, step * maxTicksPerFrame);
            int count = 0;
            while (accumulator >= step && count++ < maxTicksPerFrame)
            {
                Tick?.Invoke((float)step);
                accumulator -= step;
            }
        }

        private void OnDisable() => accumulator = 0;
    }
}

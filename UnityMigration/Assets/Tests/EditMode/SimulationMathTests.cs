using NUnit.Framework;
using FatesPassage.Core;

namespace FatesPassage.Tests
{
    public class SimulationMathTests
    {
        [Test]
        public void OneSecondAt60HzProduces60Ticks()
        {
            double accumulator = 0;
            int ticks = 0;
            for (int i = 0; i < 60; i++)
                ticks += SimulationMath.CountTicks(ref accumulator, 1.0 / 60.0, 60, 4);
            Assert.AreEqual(60, ticks);
        }

        [Test]
        public void LongFrameIsCappedToAvoidSpiralOfDeath()
        {
            double accumulator = 0;
            Assert.AreEqual(4, SimulationMath.CountTicks(ref accumulator, 2.0, 60, 4));
            Assert.Less(accumulator, 1.0 / 60.0);
        }

        [Test]
        public void NegativeDeltaIsRejected()
        {
            double accumulator = 0;
            Assert.Throws<System.ArgumentOutOfRangeException>(() =>
                SimulationMath.CountTicks(ref accumulator, -0.01, 60, 4));
        }
    }
}

using NUnit.Framework;
using FatesPassage.Core;

namespace FatesPassage.Tests
{
    public sealed class CombatMathTests
    {
        [Test]
        public void ZeroArmorDoesNotReduceDamage() =>
            Assert.AreEqual(100f, CombatMath.MitigatedDamage(100, 0), 0.001f);

        [Test]
        public void ArmorReducesDamageWithoutGoingNegative() =>
            Assert.AreEqual(50f, CombatMath.MitigatedDamage(100, 100), 0.001f);

        [Test]
        public void PowerAndMultiplierApplyBeforeMitigation() =>
            Assert.AreEqual(30f, CombatMath.CalculateHit(10, 5, 0, 2), 0.001f);

        [Test]
        public void InvalidDamageIsRejected() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(() => CombatMath.MitigatedDamage(float.NaN, 0));
    }
}

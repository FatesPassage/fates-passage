using NUnit.Framework;
using FatesPassage.Core;

namespace FatesPassage.Tests
{
    public sealed class CharacterVitalsTests
    {
        [Test]
        public void DamageAndHealingStayWithinBounds()
        {
            var vitals = new CharacterVitals(100, 50, 60);
            Assert.AreEqual(100f, vitals.TakeDamage(200));
            Assert.IsFalse(vitals.IsAlive);
            Assert.AreEqual(100f, vitals.Heal(500));
            Assert.AreEqual(100f, vitals.Health);
        }

        [Test]
        public void InsufficientResourceDoesNotPartiallySpend()
        {
            var vitals = new CharacterVitals(100, 20, 30);
            Assert.IsFalse(vitals.SpendMana(21));
            Assert.AreEqual(20f, vitals.Mana);
            Assert.IsTrue(vitals.SpendStamina(15));
            Assert.AreEqual(15f, vitals.Stamina);
        }

        [Test]
        public void RegenRespectsMaximums()
        {
            var vitals = new CharacterVitals(100, 20, 30);
            vitals.SpendMana(10);
            vitals.SpendStamina(10);
            vitals.Regenerate(100, 100, 1);
            Assert.AreEqual(20f, vitals.Mana);
            Assert.AreEqual(30f, vitals.Stamina);
        }

        [Test]
        public void RejectsNegativeOrNonFiniteAmounts()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new CharacterVitals(float.NaN, 1, 1));
            var vitals = new CharacterVitals(100, 20, 30);
            Assert.Throws<System.ArgumentOutOfRangeException>(() => vitals.TakeDamage(-1));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => vitals.SpendMana(float.PositiveInfinity));
        }
    }
}

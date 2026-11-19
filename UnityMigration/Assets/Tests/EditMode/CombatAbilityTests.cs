using NUnit.Framework;
using FatesPassage.Core;

namespace FatesPassage.Tests
{
    public sealed class CombatAbilityTests
    {
        [Test]
        public void ManaCostAndCooldownPreventRepeatedCast()
        {
            var caster = new CharacterVitals(100, 30, 20);
            var ability = new AbilityRuntime(new CombatAbility("test_spell", 10, 12, 2, ResourceKind.Mana));
            Assert.IsTrue(ability.TryActivate(caster));
            Assert.AreEqual(18f, caster.Mana);
            Assert.IsFalse(ability.TryActivate(caster));
            ability.Advance(2f);
            Assert.IsTrue(ability.TryActivate(caster));
            Assert.AreEqual(6f, caster.Mana);
        }

        [Test]
        public void FailedCastDoesNotStartCooldown()
        {
            var caster = new CharacterVitals(100, 5, 20);
            var ability = new AbilityRuntime(new CombatAbility("expensive", 10, 10, 3, ResourceKind.Mana));
            Assert.IsFalse(ability.TryActivate(caster));
            Assert.AreEqual(0f, ability.RemainingCooldown);
            Assert.AreEqual(5f, caster.Mana);
        }

        [Test]
        public void DeadCasterCannotCast()
        {
            var caster = new CharacterVitals(100, 10, 10);
            caster.TakeDamage(100);
            var ability = new AbilityRuntime(new CombatAbility("basic", 1, 0, 0, ResourceKind.None));
            Assert.IsFalse(ability.TryActivate(caster));
        }
    }
}

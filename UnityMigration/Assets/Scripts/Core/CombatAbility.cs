using System;

namespace FatesPassage.Core
{
    public enum ResourceKind { None, Mana, Stamina }

    /// <summary>Provisional portable ability definition; no original game balance implied.</summary>
    public sealed class CombatAbility
    {
        public readonly string Id;
        public readonly float Damage;
        public readonly float Cost;
        public readonly float Cooldown;
        public readonly ResourceKind Resource;

        public CombatAbility(string id, float damage, float cost, float cooldown, ResourceKind resource)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Ability id required.", nameof(id));
            Validate(damage); Validate(cost); Validate(cooldown);
            Id = id; Damage = damage; Cost = cost; Cooldown = cooldown; Resource = resource;
        }

        private static void Validate(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value) || value < 0f)
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }

    /// <summary>Deterministic resource and cooldown gate. Call with elapsed simulation time.</summary>
    public sealed class AbilityRuntime
    {
        private readonly CombatAbility ability;
        private float remainingCooldown;
        public float RemainingCooldown => remainingCooldown;
        public AbilityRuntime(CombatAbility ability) =>
            this.ability = ability ?? throw new ArgumentNullException(nameof(ability));

        public void Advance(float deltaTime)
        {
            if (float.IsNaN(deltaTime) || float.IsInfinity(deltaTime) || deltaTime < 0f)
                throw new ArgumentOutOfRangeException(nameof(deltaTime));
            remainingCooldown = Math.Max(0f, remainingCooldown - deltaTime);
        }

        public bool TryActivate(CharacterVitals caster)
        {
            if (caster == null) throw new ArgumentNullException(nameof(caster));
            if (!caster.IsAlive || remainingCooldown > 0f) return false;
            bool paid = ability.Resource == ResourceKind.Mana ? caster.SpendMana(ability.Cost)
                : ability.Resource == ResourceKind.Stamina ? caster.SpendStamina(ability.Cost) : true;
            if (!paid) return false;
            remainingCooldown = ability.Cooldown;
            return true;
        }
    }
}

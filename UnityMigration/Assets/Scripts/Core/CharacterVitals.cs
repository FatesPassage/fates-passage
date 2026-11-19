using System;

namespace FatesPassage.Core
{
    /// <summary>Engine-independent character health, stamina and mana state.</summary>
    [Serializable]
    public sealed class CharacterVitals
    {
        public float MaxHealth { get; private set; }
        public float Health { get; private set; }
        public float MaxMana { get; private set; }
        public float Mana { get; private set; }
        public float MaxStamina { get; private set; }
        public float Stamina { get; private set; }
        public bool IsAlive => Health > 0f;

        public CharacterVitals(float health, float mana, float stamina)
        {
            if (!IsFiniteNonnegative(health) || !IsFiniteNonnegative(mana) || !IsFiniteNonnegative(stamina))
                throw new ArgumentOutOfRangeException(nameof(health), "Vitals must be finite and nonnegative.");
            MaxHealth = Health = health;
            MaxMana = Mana = mana;
            MaxStamina = Stamina = stamina;
        }

        public float TakeDamage(float amount)
        {
            CheckAmount(amount);
            float actual = Math.Min(Health, amount);
            Health -= actual;
            return actual;
        }

        public float Heal(float amount)
        {
            CheckAmount(amount);
            float actual = Math.Min(MaxHealth - Health, amount);
            Health += actual;
            return actual;
        }

        public bool SpendMana(float amount)
        {
            CheckAmount(amount);
            if (Mana < amount) return false;
            Mana -= amount;
            return true;
        }

        public bool SpendStamina(float amount)
        {
            CheckAmount(amount);
            if (Stamina < amount) return false;
            Stamina -= amount;
            return true;
        }

        public void Regenerate(float manaPerSecond, float staminaPerSecond, float deltaSeconds)
        {
            CheckAmount(manaPerSecond);
            CheckAmount(staminaPerSecond);
            CheckAmount(deltaSeconds);
            Mana = Math.Min(MaxMana, Mana + manaPerSecond * deltaSeconds);
            Stamina = Math.Min(MaxStamina, Stamina + staminaPerSecond * deltaSeconds);
        }

        private static bool IsFiniteNonnegative(float value) =>
            !float.IsNaN(value) && !float.IsInfinity(value) && value >= 0f;

        private static void CheckAmount(float value)
        {
            if (!IsFiniteNonnegative(value))
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}

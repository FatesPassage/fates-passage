using System;

namespace FatesPassage.Core
{
    /// <summary>Portable combat calculations. Values are provisional until browser rules are audited.</summary>
    public static class CombatMath
    {
        public static float MitigatedDamage(float rawDamage, float armor)
        {
            Validate(rawDamage);
            Validate(armor);
            return rawDamage * (100f / (100f + armor));
        }

        public static float CalculateHit(float baseDamage, float power, float armor, float multiplier = 1f)
        {
            Validate(baseDamage);
            Validate(power);
            Validate(multiplier);
            return MitigatedDamage((baseDamage + power) * multiplier, armor);
        }

        private static void Validate(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value) || value < 0f)
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}

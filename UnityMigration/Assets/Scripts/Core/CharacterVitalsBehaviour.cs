using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Unity bridge for engine-independent character vitals.</summary>
    public sealed class CharacterVitalsBehaviour : MonoBehaviour
    {
        [SerializeField, Min(0)] private float maxHealth = 100;
        [SerializeField, Min(0)] private float maxMana = 100;
        [SerializeField, Min(0)] private float maxStamina = 100;
        [SerializeField, Min(0)] private float manaRegenPerSecond = 2;
        [SerializeField, Min(0)] private float staminaRegenPerSecond = 3;

        public CharacterVitals Vitals { get; private set; }

        private void Awake()
        {
            Vitals = new CharacterVitals(maxHealth, maxMana, maxStamina);
        }

        private void Update()
        {
            if (Vitals != null && Vitals.IsAlive)
                Vitals.Regenerate(manaRegenPerSecond, staminaRegenPerSecond, Time.deltaTime);
        }
    }
}

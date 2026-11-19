using System;
using UnityEngine;

namespace FatesPassage.Core
{
    [RequireComponent(typeof(CharacterVitalsBehaviour))]
    public sealed class Combatant : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float armor;
        [SerializeField, Min(0f)] private float power = 10f;
        [SerializeField] private CombatFeedback hitFeedback;
        private CharacterVitalsBehaviour vitals;
        public event Action<float> Damaged;
        public event Action Defeated;
        public bool IsAlive => vitals != null && vitals.Vitals != null && vitals.Vitals.IsAlive;

        private void Awake()
        {
            vitals = GetComponent<CharacterVitalsBehaviour>();
            if (hitFeedback == null) hitFeedback = GetComponent<CombatFeedback>();
        }

        public float ReceiveHit(float rawDamage)
        {
            if (!IsAlive) return 0f;
            float applied = vitals.Vitals.TakeDamage(CombatMath.MitigatedDamage(rawDamage, armor));
            if (applied > 0f)
            {
                if (hitFeedback != null) hitFeedback.Flash();
                Damaged?.Invoke(applied);
                if (!IsAlive) Defeated?.Invoke();
            }
            return applied;
        }

        public float Attack(Combatant target, float baseDamage)
        {
            if (!IsAlive || target == null || !target.IsAlive) return 0f;
            return target.ReceiveHit(baseDamage + power);
        }
    }
}

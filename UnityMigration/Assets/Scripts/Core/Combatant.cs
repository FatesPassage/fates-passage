using UnityEngine;

namespace FatesPassage.Core
{
    [RequireComponent(typeof(CharacterVitalsBehaviour))]
    public sealed class Combatant : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float armor;
        [SerializeField, Min(0f)] private float power = 10f;
        private CharacterVitalsBehaviour vitals;
        public bool IsAlive => vitals != null && vitals.Vitals != null && vitals.Vitals.IsAlive;

        private void Awake() => vitals = GetComponent<CharacterVitalsBehaviour>();

        public float ReceiveHit(float rawDamage)
        {
            if (!IsAlive) return 0f;
            return vitals.Vitals.TakeDamage(CombatMath.MitigatedDamage(rawDamage, armor));
        }

        public float Attack(Combatant target, float baseDamage)
        {
            if (!IsAlive || target == null || !target.IsAlive) return 0f;
            return target.ReceiveHit(baseDamage + power);
        }
    }
}

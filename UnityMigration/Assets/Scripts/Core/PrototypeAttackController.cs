using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Temporary target-based attack bridge for the playable test scene.</summary>
    [RequireComponent(typeof(Combatant))]
    public sealed class PrototypeAttackController : MonoBehaviour
    {
        [SerializeField] private Combatant target;
        [SerializeField, Min(0f)] private float attackRange = 3f;
        [SerializeField, Min(0f)] private float damage = 12f;
        [SerializeField, Min(0f)] private float staminaCost = 5f;
        [SerializeField, Min(0f)] private float cooldownSeconds = 0.5f;
        private CharacterVitalsBehaviour vitals;
        private Combatant attacker;
        private AbilityRuntime attack;

        private void Awake()
        {
            attacker = GetComponent<Combatant>();
            vitals = GetComponent<CharacterVitalsBehaviour>();
            attack = new AbilityRuntime(new CombatAbility("prototype_strike", damage, staminaCost,
                cooldownSeconds, ResourceKind.Stamina));
        }

        private void Update()
        {
            attack.Advance(Time.deltaTime);
            if (!PrototypeInputReader.AttackPressed() || target == null || !target.IsAlive) return;
            if ((target.transform.position - transform.position).sqrMagnitude > attackRange * attackRange) return;
            if (vitals == null || vitals.Vitals == null || !attack.TryActivate(vitals.Vitals)) return;
            attacker.Attack(target, damage);
        }

        public void SetTarget(Combatant combatTarget) => target = combatTarget;
    }
}

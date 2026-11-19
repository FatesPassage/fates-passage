using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Temporary training dummy status UI for validating damage and defeat.</summary>
    public sealed class PrototypeCombatStatus : MonoBehaviour
    {
        [SerializeField] private CharacterVitalsBehaviour enemy;
        private GUIStyle style;

        public void SetEnemy(CharacterVitalsBehaviour value) => enemy = value;

        private void OnGUI()
        {
            if (enemy == null || enemy.Vitals == null) return;
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.box) { fontSize = 13, fontStyle = FontStyle.Bold };
                style.normal.textColor = Color.white;
            }
            var v = enemy.Vitals;
            string state = v.IsAlive ? $"Training Dummy  {v.Health:0}/{v.MaxHealth:0} HP" : "Training Dummy  DEFEATED";
            float width = 280;
            GUI.Box(new Rect((Screen.width - width) / 2f, 12, width, 30), state, style);
        }
    }
}

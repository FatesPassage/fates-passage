using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Compact training-target status with readable health progress and defeat feedback.</summary>
    public sealed class PrototypeCombatStatus : MonoBehaviour
    {
        [SerializeField] private CharacterVitalsBehaviour enemy;
        [SerializeField] private PrototypeAttackController attacker;
        private GUIStyle titleStyle;
        private GUIStyle valueStyle;
        private Texture2D background;
        private Texture2D track;
        private Texture2D fill;

        public void SetEnemy(CharacterVitalsBehaviour value) => enemy = value;
        public void SetAttacker(PrototypeAttackController value) => attacker = value;

        private static Texture2D Solid(Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        private void EnsureStyles()
        {
            if (titleStyle != null) return;
            background = Solid(new Color(0.035f, 0.05f, 0.075f, 0.9f));
            track = Solid(new Color(0.12f, 0.13f, 0.16f, 0.95f));
            fill = Solid(new Color(0.68f, 0.23f, 0.17f));
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.94f, 0.89f, 0.78f) }
            };
            valueStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleRight,
                normal = { textColor = new Color(0.85f, 0.88f, 0.92f) }
            };
        }

        private void OnGUI()
        {
            if (enemy == null || enemy.Vitals == null) return;
            EnsureStyles();
            var vitals = enemy.Vitals;
            var safe = Screen.safeArea;
            float width = Mathf.Min(340f, Mathf.Max(0f, safe.width - 24f));
            if (width < 100f) return;
            float x = safe.xMin + (safe.width - width) * 0.5f;
            float y = Screen.height - safe.yMax + 14f;
            titleStyle.fontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height * 0.019f), 12, 18);
            valueStyle.fontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height * 0.016f), 11, 15);
            GUI.DrawTexture(new Rect(x, y, width, 72f), background);
            GUI.Label(new Rect(x + 14f, y + 5f, width * 0.58f, 25f),
                vitals.IsAlive ? "TRAINING DUMMY" : "TARGET DEFEATED", titleStyle);
            GUI.Label(new Rect(x + width * 0.60f, y + 5f, width * 0.35f, 25f),
                vitals.IsAlive ? $"{vitals.Health:0} / {vitals.MaxHealth:0}" : "0 HP", valueStyle);
            float barWidth = width - 28f;
            GUI.DrawTexture(new Rect(x + 14f, y + 35f, barWidth, 8f), track);
            float ratio = vitals.MaxHealth > 0f ? Mathf.Clamp01(vitals.Health / vitals.MaxHealth) : 0f;
            if (ratio > 0f) GUI.DrawTexture(new Rect(x + 14f, y + 35f, barWidth * ratio, 8f), fill);
            GUI.Label(new Rect(x + 14f, y + 48f, width - 28f, 18f),
                attacker == null ? "SPACE / GAMEPAD SOUTH  •  ATTACK" : $"HITS {attacker.SuccessfulHits}  •  SPACE / GAMEPAD SOUTH", valueStyle);
        }

        private void OnDestroy()
        {
            if (background != null) Destroy(background);
            if (track != null) Destroy(track);
            if (fill != null) Destroy(fill);
        }
    }
}

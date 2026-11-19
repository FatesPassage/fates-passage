using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Target plate with readable health bar and defeated state for combat testing.</summary>
    public sealed class PrototypeCombatStatus : MonoBehaviour
    {
        [SerializeField] private CharacterVitalsBehaviour enemy;
        private GUIStyle titleStyle, valueStyle;
        private Texture2D pixel;

        public void SetEnemy(CharacterVitalsBehaviour value) => enemy = value;

        private void OnGUI()
        {
            if (enemy == null || enemy.Vitals == null) return;
            EnsureStyles();
            var v = enemy.Vitals;
            const float width = 300f, height = 58f;
            float x = (Screen.width - width) * 0.5f;
            var panel = new Rect(x, 14, width, height);
            Draw(panel, new Color(0.025f, 0.035f, 0.055f, 0.9f));
            Draw(new Rect(x, panel.y, width, 2), v.IsAlive
                ? new Color(0.68f, 0.36f, 0.19f, 0.95f)
                : new Color(0.25f, 0.68f, 0.43f, 0.95f));
            GUI.Label(new Rect(x + 13, panel.y + 6, 180, 18),
                v.IsAlive ? "TRAINING TARGET" : "TARGET DEFEATED", titleStyle);
            GUI.Label(new Rect(x + width - 105, panel.y + 6, 92, 18),
                v.IsAlive ? $"{v.Health:0} / {v.MaxHealth:0} HP" : "CLEARED", valueStyle);
            var bar = new Rect(x + 13, panel.y + 31, width - 26, 9);
            Draw(bar, new Color(0.13f, 0.16f, 0.21f, 1));
            float ratio = v.MaxHealth > 0 ? Mathf.Clamp01(v.Health / v.MaxHealth) : 0;
            Draw(new Rect(bar.x, bar.y, bar.width * ratio, bar.height),
                v.IsAlive ? new Color(0.72f, 0.22f, 0.20f) : new Color(0.25f, 0.68f, 0.43f));
        }

        private void EnsureStyles()
        {
            if (pixel == null)
            {
                pixel = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                pixel.SetPixel(0, 0, Color.white);
                pixel.Apply();
                pixel.hideFlags = HideFlags.HideAndDontSave;
            }
            if (titleStyle != null) return;
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11, fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
            titleStyle.normal.textColor = new Color(0.94f, 0.83f, 0.61f);
            valueStyle = new GUIStyle(titleStyle) { alignment = TextAnchor.MiddleRight, fontSize = 10 };
            valueStyle.normal.textColor = new Color(0.82f, 0.86f, 0.92f);
        }

        private void Draw(Rect rect, Color color)
        {
            Color old = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, pixel);
            GUI.color = old;
        }

        private void OnDestroy()
        {
            if (pixel != null) Destroy(pixel);
        }
    }
}

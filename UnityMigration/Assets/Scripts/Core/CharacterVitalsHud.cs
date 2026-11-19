using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Compact dark-fantasy resource panel, anchored to the lower-left safe area.</summary>
    public sealed class CharacterVitalsHud : MonoBehaviour
    {
        [SerializeField] private CharacterVitalsBehaviour character;
        [SerializeField] private bool visible = true;
        [SerializeField] private Vector2 margin = new Vector2(18, 18);
        [SerializeField] private float panelWidth = 292f;
        private GUIStyle titleStyle, labelStyle, valueStyle;
        private Texture2D pixel;

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
            titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 11, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleLeft };
            titleStyle.normal.textColor = new Color(0.94f, 0.83f, 0.61f);
            labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 10, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleLeft };
            labelStyle.normal.textColor = new Color(0.91f, 0.93f, 0.97f);
            valueStyle = new GUIStyle(labelStyle) { alignment = TextAnchor.MiddleRight, fontSize = 10 };
            valueStyle.normal.textColor = new Color(0.78f, 0.82f, 0.89f);
        }

        private void OnGUI()
        {
            if (!visible || character == null || character.Vitals == null) return;
            EnsureStyles();
            const float height = 126f;
            // Respect device notches/home indicators and shrink gracefully on narrow screens.
            Rect safe = Screen.safeArea;
            float width = Mathf.Max(180f, Mathf.Min(panelWidth, safe.width - margin.x * 2f));
            float x = safe.xMin + margin.x;
            float y = Screen.height - safe.yMin - height - margin.y;
            var rect = new Rect(x, y, width, height);
            DrawRect(rect, new Color(0.025f, 0.035f, 0.055f, 0.91f));
            DrawRect(new Rect(rect.x, rect.y, rect.width, 2), new Color(0.65f, 0.47f, 0.23f, 0.95f));
            GUI.Label(new Rect(rect.x + 12, rect.y + 7, 180, 18), "FATEWALKER  /  VITALS", titleStyle);
            GUI.Label(new Rect(rect.x + rect.width - 78, rect.y + 7, 66, 18), "STATUS", valueStyle);
            DrawRect(new Rect(rect.x + 12, rect.y + 29, rect.width - 24, 1), new Color(0.65f, 0.47f, 0.23f, 0.32f));

            var v = character.Vitals;
            DrawBar(rect, 0, "HEALTH", v.Health, v.MaxHealth, new Color(0.79f, 0.22f, 0.25f));
            DrawBar(rect, 1, "MANA", v.Mana, v.MaxMana, new Color(0.22f, 0.49f, 0.91f));
            DrawBar(rect, 2, "ENERGY", v.Stamina, v.MaxStamina, new Color(0.20f, 0.68f, 0.47f));
        }

        private void DrawBar(Rect panel, int row, string label, float value, float maximum, Color fill)
        {
            float y = panel.y + 39 + row * 27;
            float x = panel.x + 12;
            float width = panel.width - 24;
            GUI.Label(new Rect(x, y, 100, 15), label, labelStyle);
            GUI.Label(new Rect(x + width - 92, y, 92, 15), $"{value:0} / {maximum:0}", valueStyle);
            DrawRect(new Rect(x, y + 16, width, 6), new Color(0.13f, 0.16f, 0.21f, 1));
            float ratio = maximum > 0 ? Mathf.Clamp01(value / maximum) : 0;
            DrawRect(new Rect(x, y + 16, width * ratio, 6), fill);
            DrawRect(new Rect(x, y + 16, width * ratio, 1), new Color(1, 1, 1, 0.3f));
        }

        private void DrawRect(Rect rect, Color color)
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

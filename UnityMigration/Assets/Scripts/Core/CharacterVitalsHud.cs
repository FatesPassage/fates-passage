using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Temporary lightweight health/mana/stamina UI for validating native vitals.</summary>
    public sealed class CharacterVitalsHud : MonoBehaviour
    {
        [SerializeField] private CharacterVitalsBehaviour character;
        [SerializeField] private bool visible = true;
        [SerializeField] private Vector2 anchor = new Vector2(16, 56);
        private GUIStyle labelStyle;
        private Texture2D pixel;

        private void OnGUI()
        {
            if (!visible || character == null || character.Vitals == null) return;
            if (pixel == null)
            {
                pixel = new Texture2D(1, 1);
                pixel.SetPixel(0, 0, Color.white);
                pixel.Apply();
            }
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 12, fontStyle = FontStyle.Bold };
                labelStyle.normal.textColor = Color.white;
            }

            var v = character.Vitals;
            DrawBar(0, "HEALTH", v.Health, v.MaxHealth, new Color(0.72f, 0.19f, 0.24f));
            DrawBar(1, "MANA", v.Mana, v.MaxMana, new Color(0.22f, 0.49f, 0.91f));
            DrawBar(2, "STAMINA", v.Stamina, v.MaxStamina, new Color(0.22f, 0.68f, 0.44f));
        }

        private void DrawBar(int row, string label, float value, float maximum, Color fill)
        {
            float x = anchor.x;
            float y = anchor.y + row * 32f;
            DrawRect(new Rect(x, y, 218, 27), new Color(0.05f, 0.07f, 0.11f, 0.88f));
            DrawRect(new Rect(x + 4, y + 18, 210, 5), new Color(0.18f, 0.22f, 0.28f));
            float ratio = maximum > 0 ? Mathf.Clamp01(value / maximum) : 0;
            DrawRect(new Rect(x + 4, y + 18, 210 * ratio, 5), fill);
            GUI.Label(new Rect(x + 8, y + 1, 112, 18), label, labelStyle);
            GUI.Label(new Rect(x + 125, y + 1, 86, 18), $"{value:0}/{maximum:0}", labelStyle);
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

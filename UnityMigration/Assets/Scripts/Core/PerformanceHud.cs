using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Compact, optional runtime diagnostics. Hidden by default; toggle with F3.</summary>
    public sealed class PerformanceHud : MonoBehaviour
    {
        [SerializeField] private bool visible;
        private float elapsed;
        private int frameCount;
        private float fps;
        private GUIStyle style;
        private Texture2D backdrop;
        private int lastFontSize;

        private void Update()
        {
            if (PrototypeInputReader.DiagnosticsTogglePressed()) visible = !visible;
            elapsed += Time.unscaledDeltaTime;
            frameCount++;
            if (elapsed < 0.5f) return;
            fps = frameCount / Mathf.Max(0.0001f, elapsed);
            elapsed = 0f;
            frameCount = 0;
        }

        private void OnGUI()
        {
            if (!visible) return;
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height * 0.018f), 12, 20),
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(14, 14, 6, 6)
                };
                lastFontSize = style.fontSize;
                style.normal.textColor = new Color(0.88f, 0.95f, 1f);
                backdrop = new Texture2D(1, 1);
                backdrop.SetPixel(0, 0, new Color(0.035f, 0.055f, 0.09f, 0.9f));
                backdrop.Apply();
                style.normal.background = backdrop;
            }
            int desiredFontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height * 0.018f), 12, 20);
            if (desiredFontSize != lastFontSize)
            {
                style.fontSize = desiredFontSize;
                lastFontSize = desiredFontSize;
            }
            Rect safe = Screen.safeArea;
            float margin = Mathf.Clamp(Screen.width * 0.015f, 12f, 24f);
            float width = Mathf.Min(Mathf.Clamp(Screen.width * 0.18f, 190f, 270f), Mathf.Max(0f, safe.width - 2f * margin));
            style.normal.textColor = fps < 30f ? new Color(1f, 0.68f, 0.55f) : new Color(0.88f, 0.95f, 1f);
            GUI.Label(new Rect(safe.xMin + margin, Screen.height - safe.yMax + margin, width, 36f), $"FPS {fps:0}  •  {1000f / Mathf.Max(fps, 0.01f):0.0} ms", style);
        }

        private void OnDestroy()
        {
            if (backdrop != null) Destroy(backdrop);
        }
    }
}

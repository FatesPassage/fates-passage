using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Compact, optional runtime diagnostics. Hidden by default; toggle with F3.</summary>
    public sealed class PerformanceHud : MonoBehaviour
    {
        [SerializeField] private KeyCode toggleKey = KeyCode.F3;
        [SerializeField] private bool visible;
        private float elapsed;
        private int frameCount;
        private float fps;
        private GUIStyle style;
        private Texture2D backdrop;

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey)) visible = !visible;
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
                    fontSize = 13,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(12, 12, 4, 4)
                };
                style.normal.textColor = new Color(0.88f, 0.95f, 1f);
                backdrop = new Texture2D(1, 1);
                backdrop.SetPixel(0, 0, new Color(0.035f, 0.055f, 0.09f, 0.9f));
                backdrop.Apply();
                style.normal.background = backdrop;
            }
            GUI.Label(new Rect(12, 12, 190, 30), $"FPS {fps:0}  •  {1000f / Mathf.Max(fps, 0.01f):0.0} ms", style);
        }

        private void OnDestroy()
        {
            if (backdrop != null) Destroy(backdrop);
        }
    }
}

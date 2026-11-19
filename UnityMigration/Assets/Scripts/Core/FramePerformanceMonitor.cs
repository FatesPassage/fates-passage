using UnityEngine;

namespace FatesPassage.Core
{
    public sealed class FramePerformanceMonitor : MonoBehaviour
    {
        [SerializeField] private bool showOverlay;
        [SerializeField, Min(0.1f)] private float sampleWindowSeconds = 1f;
        private float elapsed;
        private int frames;
        private float fps;
        private float averageFrameMs;

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            frames++;
            if (elapsed < sampleWindowSeconds) return;
            fps = frames / Mathf.Max(elapsed, 0.0001f);
            averageFrameMs = 1000f / Mathf.Max(fps, 0.0001f);
            elapsed = 0f;
            frames = 0;
        }

        private void OnGUI()
        {
            if (!showOverlay) return;
            GUI.Label(new Rect(12, 12, 240, 28), $"FPS: {fps:F1} | Avg frame: {averageFrameMs:F1} ms");
        }
    }
}

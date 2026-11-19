using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Optional hit feedback, without coupling damage rules to visual effects.</summary>
    public sealed class CombatFeedback : MonoBehaviour
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private Color hitTint = new Color(1f, 0.35f, 0.25f);
        [SerializeField, Min(0.01f)] private float flashDuration = 0.12f;
        private MaterialPropertyBlock block;
        private float flashRemaining;

        public void Flash()
        {
            flashRemaining = flashDuration;
        }

        private void Update()
        {
            if (targetRenderer == null) return;
            if (block == null) block = new MaterialPropertyBlock();
            flashRemaining = Mathf.Max(0f, flashRemaining - Time.deltaTime);
            targetRenderer.GetPropertyBlock(block);
            float blend = flashDuration > 0f ? flashRemaining / flashDuration : 0f;
            block.SetColor("_BaseColor", Color.Lerp(Color.white, hitTint, blend));
            targetRenderer.SetPropertyBlock(block);
        }

        private void OnDisable()
        {
            if (targetRenderer == null) return;
            targetRenderer.SetPropertyBlock(null);
        }
    }
}

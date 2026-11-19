using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Optional URP hit feedback. Preserves original per-renderer material properties.</summary>
    public sealed class CombatFeedback : MonoBehaviour
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private Color hitTint = new Color(1f, 0.35f, 0.25f);
        [SerializeField, Min(0.01f)] private float flashDuration = 0.12f;
        private MaterialPropertyBlock block;
        private float flashRemaining;
        private Color baseColor = Color.white;
        private bool hasColor;
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            if (targetRenderer == null) return;
            block = new MaterialPropertyBlock();
            targetRenderer.GetPropertyBlock(block);
            var material = targetRenderer.sharedMaterial;
            if (material != null && material.HasProperty(BaseColorId))
            {
                baseColor = material.GetColor(BaseColorId);
                hasColor = true;
            }
        }

        public void Flash() => flashRemaining = flashDuration;

        private void Update()
        {
            if (targetRenderer == null || !hasColor || flashRemaining <= 0f) return;
            flashRemaining = Mathf.Max(0f, flashRemaining - Time.deltaTime);
            targetRenderer.GetPropertyBlock(block);
            float blend = flashDuration > 0f ? flashRemaining / flashDuration : 0f;
            block.SetColor(BaseColorId, Color.Lerp(baseColor, hitTint, blend));
            targetRenderer.SetPropertyBlock(block);
        }

        private void OnDisable()
        {
            if (targetRenderer == null || !hasColor || block == null) return;
            targetRenderer.GetPropertyBlock(block);
            block.SetColor(BaseColorId, baseColor);
            targetRenderer.SetPropertyBlock(block);
        }
    }
}

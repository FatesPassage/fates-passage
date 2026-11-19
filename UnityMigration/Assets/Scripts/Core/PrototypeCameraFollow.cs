using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Simple camera follow for prototype validation; not the final Fate's Passage camera.</summary>
    public sealed class PrototypeCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -8);
        [SerializeField, Min(0.01f)] private float smoothing = 10f;

        public void SetTarget(Transform value) => target = value;

        private void LateUpdate()
        {
            if (target == null) return;
            float blend = 1f - Mathf.Exp(-smoothing * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, target.position + offset, blend);
            transform.LookAt(target.position + Vector3.up * 0.6f);
        }
    }
}

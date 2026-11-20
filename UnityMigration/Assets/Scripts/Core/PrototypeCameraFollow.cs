using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace FatesPassage.Core
{
    /// <summary>Prototype orbit camera: desktop drag/wheel and right-side touch drag, matching browser gestures.</summary>
    public sealed class PrototypeCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField, Min(0.01f)] private float smoothing = 10f;
        [SerializeField] private float yaw;
        [SerializeField, Range(12f, 75f)] private float pitch = 32f;
        [SerializeField, Min(2f)] private float distance = 9f;
        [SerializeField, Min(0.01f)] private float dragSensitivity = 0.2f;
        private int rightTouchId = -1;
        private Vector2 previousTouch;

        public void SetTarget(Transform value) => target = value;

        private void Update()
        {
#if ENABLE_INPUT_SYSTEM
            var mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.isPressed)
            {
                Vector2 delta = mouse.delta.ReadValue();
                yaw += delta.x * dragSensitivity;
                pitch = Mathf.Clamp(pitch - delta.y * dragSensitivity, 12f, 75f);
            }
            if (mouse != null)
                distance = Mathf.Clamp(distance - mouse.scroll.ReadValue().y * 0.01f, 3f, 18f);
            var touchscreen = Touchscreen.current;
            if (touchscreen != null)
            {
                bool found = false;
                foreach (var touch in touchscreen.touches)
                {
                    if (!touch.press.isPressed) continue;
                    Vector2 pos = touch.position.ReadValue();
                    if (rightTouchId < 0 && pos.x >= Screen.width / 2f)
                    {
                        rightTouchId = touch.touchId.ReadValue();
                        previousTouch = pos;
                    }
                    if (touch.touchId.ReadValue() != rightTouchId) continue;
                    Vector2 delta = pos - previousTouch;
                    previousTouch = pos;
                    yaw += delta.x * dragSensitivity;
                    pitch = Mathf.Clamp(pitch - delta.y * dragSensitivity, 12f, 75f);
                    found = true;
                    break;
                }
                if (!found) rightTouchId = -1;
            }
#elif ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButton(0))
            {
                yaw += Input.GetAxis("Mouse X") * 2f;
                pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * 2f, 12f, 75f);
            }
            distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y, 3f, 18f);
#endif
        }

        private void LateUpdate()
        {
            if (target == null) return;
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 desired = target.position + Vector3.up * 0.6f + rotation * (Vector3.back * distance);
            float blend = 1f - Mathf.Exp(-smoothing * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desired, blend);
            transform.LookAt(target.position + Vector3.up * 0.6f);
        }
    }
}

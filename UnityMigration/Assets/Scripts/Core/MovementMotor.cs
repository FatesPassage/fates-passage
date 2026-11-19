using UnityEngine;

namespace FatesPassage.Core
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class MovementMotor : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 5f;
        [SerializeField, Min(0f)] private float gravity = 20f;
        private CharacterController controller;
        private float verticalSpeed;

        private void Awake() => controller = GetComponent<CharacterController>();

        /// <summary>Call once per simulation step with normalized world-space movement input.</summary>
        public void Simulate(Vector2 input, float deltaTime)
        {
            if (deltaTime <= 0f) return;
            Vector3 horizontal = new Vector3(input.x, 0f, input.y);
            horizontal = Vector3.ClampMagnitude(horizontal, 1f) * moveSpeed;
            if (controller.isGrounded && verticalSpeed < 0f) verticalSpeed = -1f;
            verticalSpeed -= gravity * deltaTime;
            controller.Move((horizontal + Vector3.up * verticalSpeed) * deltaTime);
        }
    }
}

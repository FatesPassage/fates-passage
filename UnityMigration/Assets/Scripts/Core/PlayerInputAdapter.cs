using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Legacy Unity input adapter; replace with Input System actions after source control audit.</summary>
    [RequireComponent(typeof(MovementMotor))]
    public sealed class PlayerInputAdapter : MonoBehaviour
    {
        [SerializeField] private FixedTickClock simulationClock;
        [SerializeField] private bool cameraRelative = true;
        [SerializeField] private Transform cameraTransform;
        private MovementMotor motor;
        private Vector2 sampledInput;

        private void Awake() => motor = GetComponent<MovementMotor>();

        private void OnEnable()
        {
            if (simulationClock != null) simulationClock.Tick += Simulate;
        }

        private void OnDisable()
        {
            if (simulationClock != null) simulationClock.Tick -= Simulate;
            sampledInput = Vector2.zero;
        }

        private void Update()
        {
            sampledInput = Vector2.ClampMagnitude(
                new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f);
            // Fallback if no clock was assigned: exactly one simulation call per frame.
            if (simulationClock == null) Simulate(Time.deltaTime);
        }

        private void Simulate(float deltaTime)
        {
            Vector2 direction = sampledInput;
            if (cameraRelative && cameraTransform != null)
            {
                Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
                Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
                Vector3 world = right * direction.x + forward * direction.y;
                direction = new Vector2(world.x, world.z);
            }
            motor.Simulate(direction, deltaTime);
        }
    }
}

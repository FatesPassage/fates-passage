using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace FatesPassage.Core
{
    /// <summary>Single input facade for the temporary prototype; prefers the modern Input System.</summary>
    public static class PrototypeInputReader
    {
#if ENABLE_INPUT_SYSTEM
        private static int movementTouchId = -1;
        private static Vector2 movementTouchOrigin;
#endif
        public static Vector2 ReadMove()
        {
#if ENABLE_INPUT_SYSTEM
            Vector2 value = Vector2.zero;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) value.x -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) value.x += 1f;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) value.y -= 1f;
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) value.y += 1f;
            }
            if (Gamepad.current != null)
            {
                Vector2 stick = Gamepad.current.leftStick.ReadValue();
                if (stick.sqrMagnitude > value.sqrMagnitude) value = stick;
                if (Gamepad.current.dpad.ReadValue().sqrMagnitude > value.sqrMagnitude)
                    value = Gamepad.current.dpad.ReadValue();
            }
            var touchscreen = Touchscreen.current;
            if (touchscreen != null)
            {
                bool found = false;
                foreach (var touch in touchscreen.touches)
                {
                    if (!touch.press.isPressed) continue;
                    Vector2 position = touch.position.ReadValue();
                    if (movementTouchId < 0 && position.x < Screen.width * 0.5f)
                    {
                        movementTouchId = touch.touchId.ReadValue();
                        movementTouchOrigin = position;
                    }
                    if (touch.touchId.ReadValue() != movementTouchId) continue;
                    Vector2 movement = Vector2.ClampMagnitude((position - movementTouchOrigin) / 72f, 1f);
                    if (movement.sqrMagnitude > value.sqrMagnitude) value = movement;
                    found = true;
                    break;
                }
                if (!found) movementTouchId = -1;
            }
            return Vector2.ClampMagnitude(value, 1f);
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Vector2.ClampMagnitude(
                new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f);
#else
            return Vector2.zero;
#endif
        }

        public static bool DiagnosticsTogglePressed()
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && Keyboard.current.f3Key.wasPressedThisFrame;
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.F3);
#else
            return false;
#endif
        }

        public static bool AttackPressed()
        {
#if ENABLE_INPUT_SYSTEM
            return (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
                   (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Space);
#else
            return false;
#endif
        }
    }
}

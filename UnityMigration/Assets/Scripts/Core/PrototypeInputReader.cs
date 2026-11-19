using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace FatesPassage.Core
{
    /// <summary>Single input facade for the temporary prototype; prefers the modern Input System.</summary>
    public static class PrototypeInputReader
    {
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
            return Vector2.ClampMagnitude(value, 1f);
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Vector2.ClampMagnitude(
                new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f);
#else
            return Vector2.zero;
#endif
        }

        public static bool DiagnosticsTogglePressed()\n        {\n#if ENABLE_INPUT_SYSTEM\n            return Keyboard.current != null && Keyboard.current.f3Key.wasPressedThisFrame;\n#elif ENABLE_LEGACY_INPUT_MANAGER\n            return Input.GetKeyDown(KeyCode.F3);\n#else\n            return false;\n#endif\n        }\n\n        public static bool AttackPressed()
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

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DetectDeviceChange : MonoBehaviour
{
    [Header("Control Schema")]
    public UnityEvent<string> onControlSchemeChanged;
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse";
    private string previousControlScheme = "";

    private void Update()
    {
        DetectControlScheme();
    }

    private void DetectControlScheme()
    {
        Mouse currentMouse = Mouse.current;
        Keyboard currentKeyboard = Keyboard.current;
        Gamepad currentGamepad = Gamepad.current;

        // Detect keyboard/mouse movement
        bool mouseMoved = currentMouse.delta.ReadValue().sqrMagnitude > 0.01f;
        bool keyPressed = currentKeyboard.anyKey.wasPressedThisFrame;

        // Detect gamepad movement
        bool gamepadMoved = currentGamepad != null && currentGamepad.leftStick.ReadValue().sqrMagnitude > 0.01f;

        if ((mouseMoved || keyPressed) && previousControlScheme != mouseScheme)
        {
            SwitchToMouseScheme();
        }
        else if (gamepadMoved && previousControlScheme != gamepadScheme)
        {
            SwitchToGamepadScheme();
        }
    }

    private void SwitchToMouseScheme()
    {
        previousControlScheme = mouseScheme;
        onControlSchemeChanged?.Invoke(mouseScheme);
    }

    private void SwitchToGamepadScheme()
    {
        previousControlScheme = gamepadScheme;
        onControlSchemeChanged?.Invoke(gamepadScheme);
    }
}

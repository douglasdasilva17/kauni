using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipButton : MonoBehaviour
{
    [Header("HOLD SETTINGS")]
    public GameObject activeButton;
    public GameObject keyboardButton;
    public GameObject gamepadButton;
    public TextMeshProUGUI keyboardButtonText;
    public TextMeshProUGUI gamepadButtonText;
    public Image keyboardButtonFill;
    public Image gamepadButtonFill;

    string currentControl;

    public bool holdOption = false;

    public void SetControl(bool hideButton, string control = "", string content = "")
    {
        activeButton.SetActive(!hideButton);

        currentControl = control;

        switch (currentControl)
        {
            case "Keyboard and Mouse":
                keyboardButtonFill.enabled = holdOption;
                keyboardButton.SetActive(true);
                gamepadButton.SetActive(false);
                break;
            case "Gamepad":
                gamepadButtonFill.enabled = holdOption;
                keyboardButton.SetActive(false);
                gamepadButton.SetActive(true);
                break;
        }

        keyboardButtonText.text = content;
        gamepadButtonText.text = content;
    }
}

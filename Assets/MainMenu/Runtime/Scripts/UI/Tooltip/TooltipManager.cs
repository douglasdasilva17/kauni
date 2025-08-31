using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager current;

    [SerializeField]
    private Tooltip tooltip;

    private void Awake()
    {
        current = this;
    }

    public static CanvasGroup GetCanvasGroup()
    {
        return current.tooltip.GetComponent<CanvasGroup>();
    }

    public static void Show(Vector3 desiredPosition, bool altPosition, bool hideButton, Sprite portrait, string header, string content = "", string controlName = "", string controlContent = "")
    {
        if (current == null) return;

        current.tooltip.SetText(header, content, portrait);
        current.tooltip.SetAltPosition(desiredPosition, altPosition);
        current.tooltip.SetControl(controlName);
        current.tooltip.tooltipButton.SetControl(hideButton, controlName, controlContent);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        if (current.tooltip == null) return;

        current.tooltip.gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("SETTINGS")]
    public string header;
    [TextArea]
    public string content;
    public Sprite portrait;

    [Header("CONTROLS")]
    public string controlContent;
    private string controlName;

    public bool hideButton = true;

    [Header("ALT POSITION")]
    public bool altPosition = false;
    [Tooltip("The tooltip has a custom offset settings which cause positioning problem to the tooltip when we use the buttonTransform position as desired position to place the tooltip")]
    public Vector2 pivotOffsetFix = new Vector2(2f, -1f);
    public Transform buttonTransform;

    [Header("TIMING")]
    [SerializeField] private float showDelay = 0.5f; // Delay before showing the tooltip
    [SerializeField] private float fadeDuration = 0.3f; // Duration of the fade-in effect

    private Tween showTween;

    public void SetControlScheme(string controlScheme)
    {
        controlName = controlScheme;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Kill any existing show tween to avoid duplicates
        showTween?.Kill();

        // Start delayed show
        showTween = DOVirtual.DelayedCall(showDelay, () =>
        {
            // Reset the controlName variable
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, buttonTransform.position + new Vector3(pivotOffsetFix.x, pivotOffsetFix.y));

            TooltipManager.Show(screenPoint, altPosition, hideButton, portrait, header, content, controlName, controlContent);

            // After showing, fade in if TooltipManager has a CanvasGroup
            CanvasGroup tooltipCanvasGroup = TooltipManager.GetCanvasGroup();
            if (tooltipCanvasGroup != null)
            {
                tooltipCanvasGroup.DOKill();
                tooltipCanvasGroup.alpha = 0f;
                tooltipCanvasGroup.DOFade(1f, fadeDuration);
            }
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Kill any pending show
        showTween?.Kill();
        TooltipManager.Hide();
    }
}

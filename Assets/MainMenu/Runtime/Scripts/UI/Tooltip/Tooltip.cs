using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Supertactic.VirtualCursor;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private Transform tooltipTransform;
    [SerializeField] TextMeshProUGUI headerField;
    [SerializeField] TextMeshProUGUI contentField;
    [SerializeField] Image portraitField;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] float characterWrapLimit = 80;

    //RectTransform rectTransform;

    private Camera mainCamera;

    public TooltipButton tooltipButton;

    bool altPosition;
    Vector2 desiredPosition;

    string controlName;

    private void Awake()
    {
        // rectTransform = GetComponent<RectTransform>();

        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 movePos;

        Vector3 targetPos = altPosition ? desiredPosition : controlName == "Gamepad" ? VirtualCursorManager.GetVirtualMousePosition() : Input.mousePosition;

        //float pivotX = (targetPos.x / Screen.width);
        //float pivotY = (targetPos.y / Screen.height);

        //rectTransform.pivot = new Vector2(pivotX, pivotY);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            targetPos,
            mainCamera, out movePos);

        tooltipTransform.localPosition = movePos; //arentCanvas.transform.TransformPoint(movePos);
    }

    public void SetText(string header, string content = "", Sprite portrait = null)
    {
        if (portrait != null)
        {
            portraitField.gameObject.SetActive(true);
            portraitField.sprite = portrait;
        }
        else
        {
            portraitField.gameObject.SetActive(false);
        }

        SetText(header, content);
    }

    public void SetText(string header, string content = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    public void SetControl(string controlName)
    {
        this.controlName = controlName;
    }

    public void SetAltPosition(Vector3 desiredPosition, bool altPosition)
    {
        this.altPosition = altPosition;
        this.desiredPosition = desiredPosition;
    }
}

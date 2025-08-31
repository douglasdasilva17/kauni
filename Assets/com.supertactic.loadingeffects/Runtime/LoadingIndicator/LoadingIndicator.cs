using TMPro;
using UnityEngine;
using DG.Tweening;

public class LoadingIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI indicatorText;

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayTime = 2f;

    private CanvasGroup canvasGroup;
    private Tween activeTween;
    private float newFadeDuration;

    private void Awake()
    {
        // Ensure the object has a CanvasGroup for fading
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Start invisible
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        gameObject.SetActive(false);

        newFadeDuration = fadeDuration;
    }

    public void Show(float duration, string content = "CARREGANDO")
    {
        newFadeDuration = duration;

        // Cancel any ongoing tween
        activeTween?.Kill();

        // Set message and make object visible
        indicatorText.text = content;
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;

        // Fade in
        activeTween = canvasGroup.DOFade(1f, duration).OnComplete(() =>
        {
            // Wait, then fade out
            activeTween = DOVirtual.DelayedCall(displayTime, () =>
            {
                activeTween = canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
                {
                    gameObject.SetActive(false); // Fully hide the object
                });
            });
        });
    }

    public void Show(string content = "CARREGANDO")
    {
        newFadeDuration = fadeDuration;

        Show(newFadeDuration, content);
    }
}

using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Supertactic.AdventureMainMenu
{
    public class UIRotatorTransform : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform UITransform;
        [SerializeField] private Image uiImage; // Adicione o componente da imagem
        [SerializeField] private bool startRotation;
        [SerializeField] private float duration = 1f;

        [Header("Color Transition")]
        [SerializeField] private Color colorStart = new Color(1f, 0.8f, 0.3f); // Dourado quente
        [SerializeField] private Color colorEnd = new Color(1f, 0.6f, 0.1f);   // Âmbar queimado
        [SerializeField] private float colorTransitionDuration = 1.5f;

        private Tween colorTween;

        private void Start()
        {
            ToggleRotation(startRotation);
        }

        public void ToggleRotation(bool value)
        {
            if (value)
            {
                StartRotation();
                StartColorTransition();
            }
            else
            {
                StopRotation();
                StopColorTransition();
                ResetToDefault();
            }
        }

        private void StartRotation()
        {
            UITransform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360)
                .SetRelative(true)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }

        private void StopRotation()
        {
            UITransform.DOKill();
        }

        private void ResetToDefault()
        {
            ResetRotation();
            if (uiImage != null) uiImage.color = colorStart;
        }

        private void ResetRotation()
        {
            UITransform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutQuad);
        }

        private void StartColorTransition()
        {
            if (uiImage == null) return;

            colorTween = uiImage.DOColor(colorEnd, colorTransitionDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void StopColorTransition()
        {
            if (colorTween != null && colorTween.IsActive())
                colorTween.Kill();
        }
    }
}

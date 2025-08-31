using System.Collections;
using UnityEngine;

namespace Michsky.UI.Dark
{
    public class ModalWindowManager : MonoBehaviour
    {
        [Header("BRUSH ANIMATION")]
        public Animator brushAnimator;
        public bool enableSplash = true;

        private Animator mWindowAnimator;
        private bool isQuitDone;
        public bool isShowing;

        void Start()
        {
            mWindowAnimator = gameObject.GetComponent<Animator>();
        }

        public void ModalWindowIn()
        {
            mWindowAnimator.Play("Modal Window In");

            if (enableSplash == true)
            {
                brushAnimator.Play("Transition Out");
            }

            isShowing = true;
        }

        public void ModalWindowOut()
        {
            mWindowAnimator.Play("Modal Window Out");

            if (enableSplash == true)
            {
                brushAnimator.Play("Transition In");
            }

            isShowing = false;
        }

        public void QuitGame()
        {
            if (!isQuitDone)
            {
                isQuitDone = true;
                StartCoroutine(QuitRoutine());
            }
        }

        IEnumerator QuitRoutine()
        {
            yield return new WaitForSeconds(2f);
            Application.Quit();
        }
    }
}
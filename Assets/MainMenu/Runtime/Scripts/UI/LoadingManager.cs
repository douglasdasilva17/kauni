using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Compixel.UI
{
    public class LoadingManager : MonoBehaviour
    {
        private static LoadingManager current;

        [Header("Data")]
        [SerializeField] private LoadingData[] loadingData;

        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image placeholder;
        [SerializeField] private CanvasGroup backgroundCanvasGroup;
        [SerializeField] private CanvasGroup informationCanvasGroup;

        [SerializeField] private float backgroundDuration = 5f;
        [SerializeField] private float fadeDuration = 1f;

        [SerializeField]
        private GameObject loadingScreen;

        [SerializeField]
        public LoadingIndicator loadingIndicator;

        private int currentIndex = 0;


        private void Awake()
        {
            current = this;
        }

        private void Start()
        {
            loadingScreen.SetActive(false);
            loadingIndicator.Show();
        }

        public static void Show(string content, float duration)
        {
            current.loadingIndicator.Show(duration, content);
        }

        public static void LoadScene(string scene)
        {
            current.StartCoroutine(current.LoadSceneRoutine(scene));
        }

        IEnumerator LoadSceneRoutine(string scene)
        {
            float prepareLoadTime = Random.Range(5f, 15f);

            ShowLoadingPanel();

            loadingIndicator.Show(prepareLoadTime);

            yield return new WaitForSeconds(prepareLoadTime);

            Debug.Log("Start loading");

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

            //Make sure GameObject persistent so it can continue to run after the scene is loaded
            DontDestroyOnLoad(gameObject);

            //Wait for the scene to load
            while (!asyncLoad.isDone)
            {
                yield return null;
                Debug.Log("Loading...");
            }

            //Scene loaded
            Debug.Log("Loaded!");

            yield return new WaitForEndOfFrame();

            Debug.Log("First frame was loaded");

            //onFirstFrameLoaded?.Invoke();

            Destroy(gameObject);
        }

        public void ShowLoadingPanel()
        {
            loadingScreen.SetActive(true);

            //Set the alphas values of Canvas Groups to 0
            backgroundCanvasGroup.alpha = 0;
            informationCanvasGroup.alpha = 0;

            StartCoroutine(ChangeBackground());
        }

        private IEnumerator ChangeBackground()
        {
            while (true)
            {
                // Fade Out
                yield return StartCoroutine(FadeCanvasGroup(0));

                // Switch to the next data set
                LoadNextData();

                // Fade In
                yield return StartCoroutine(FadeCanvasGroup(1));

                // Wait for duration
                yield return new WaitForSeconds(backgroundDuration);
            }
        }

        private void LoadNextData()
        {
            if (loadingData.Length == 0) return;

            loadingIndicator.Show("CARREGANDO");
            currentIndex = (currentIndex + 1) % loadingData.Length;
            titleText.text = loadingData[currentIndex].loadingTitle;
            descriptionText.text = loadingData[currentIndex].loadingDescription;
            placeholder.sprite = loadingData[currentIndex].loadingPlaceholder;
        }

        private IEnumerator FadeCanvasGroup(float targetAlpha)
        {
            float startAlpha = backgroundCanvasGroup.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                backgroundCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                informationCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                yield return null;
            }

            backgroundCanvasGroup.alpha = targetAlpha;
            informationCanvasGroup.alpha = targetAlpha;
        }
    }
}
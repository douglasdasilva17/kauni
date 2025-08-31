using DG.Tweening;
using Supertactic.GameEvents;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Supertactic.QuestSystem
{
    public class QuestFlowController : MonoBehaviour
    {

        [SerializeField]
        private CanvasGroup fadeBackgorund;
        public GameObject[] questLogs;

        private Coroutine questRoutine;

        // implements the currentQuest value into questEvents and remove questManager dependency
        private QuestManager questManager;

        private void Awake()
        {
            questManager = FindAnyObjectByType<QuestManager>();
        }

        void OnEnable()
        {

            GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
            GameEventsManager.instance.questEvents.onFinishQuest += QuestFinish;
            GameEventsManager.instance.miscEvents.onGameCompleted += GameComplete;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
            GameEventsManager.instance.questEvents.onFinishQuest -= QuestFinish;
            GameEventsManager.instance.miscEvents.onGameCompleted -= GameComplete;
        }

        private void GameComplete()
        {
            StartCoroutine(EndingRoutine());
        }

        IEnumerator EndingRoutine()
        {
            yield return new WaitForSeconds(5f);
            fadeBackgorund.DOFade(1, 3).OnComplete(() => SceneManager.LoadScene("Ending"));
        }

        private void StartQuest(string questId)
        {
            foreach (GameObject questLog in questLogs)
            {
                questLog.SetActive(false);
            }

            questLogs[questManager.ActiveQuest].SetActive(true);
        }

        private void QuestFinish(string questId)
        {
            if (questRoutine != null)
            {
                StopCoroutine(questRoutine);
            }

            questRoutine = StartCoroutine(InitQuestRoutine());
        }

        IEnumerator InitQuestRoutine()
        {
            yield return new WaitForSeconds(5f);
            questManager.InitQuest(questManager.ActiveQuest);
        }
    }
}
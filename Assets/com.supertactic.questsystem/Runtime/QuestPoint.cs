using Supertactic.GameEvents;
using UnityEngine;
using Supertactic.Input;

namespace Supertactic.QuestSystem
{
    [RequireComponent(typeof(SphereCollider))]
    public class QuestPoint : MonoBehaviour
    {
        [Header("Quest")]
        [SerializeField] private QuestInfoSO questInfoForPoint;
        [SerializeField] InputReader inputReader; // remove this implementation after

        [Header("Config")]
        [SerializeField] private bool uniquePoint = false;
        [SerializeField] private bool startPoint = true;
        [SerializeField] private bool finishPoint = true;

        private bool playerIsNear = false;
        private string questId;
        private QuestState currentQuestState;

        private QuestIcon questIcon;

        private void Awake()
        {
            questId = questInfoForPoint.id;
            questIcon = GetComponentInChildren<QuestIcon>();
        }

        private void OnEnable()
        {
            inputReader.EnablePlayerActions();
            GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
            GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
            GameEventsManager.instance.questEvents.onFinishQuestStep += FinishQuestStep;

            GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
            GameEventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;
            GameEventsManager.instance.questEvents.onFinishQuestStep -= FinishQuestStep;

            GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
        }

        private void Update()
        {
            if (inputReader.SubmitInput)
            {
                SubmitPressed(InputEventContext.DEFAULT);
            }
        }

        private void QuestStateChange(Quest quest)
        {
            // only update the quest state if this point has the corresponding quest
            if (quest.info.id.Equals(questId))
            {
                currentQuestState = quest.state;
                questIcon.SetState(currentQuestState, startPoint, finishPoint);
                Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);
            }
        }

        private void SubmitPressed(InputEventContext context)
        {
            if (!playerIsNear)
            {
                return;
            }

            // start or finish a quest
            if (currentQuestState.Equals(QuestState.CAN_START) && uniquePoint)
            {
                GameEventsManager.instance.questEvents.FinishQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventsManager.instance.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                GameEventsManager.instance.questEvents.FinishQuest(questId);
            }
        }

        private void FinishQuestStep(string questId)
        {
            if (currentQuestState.Equals(QuestState.CAN_FINISH) && uniquePoint)
            {
                GameEventsManager.instance.questEvents.FinishQuest(questId);
            }
        }

        private void FinishQuest(string quest)
        {
            if (questInfoForPoint.id == quest)
            {
              gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.CompareTag("Player"))
            {
                playerIsNear = true;
            }
        }

        private void OnTriggerExit(Collider otherCollider)
        {
            if (otherCollider.CompareTag("Player"))
            {
                playerIsNear = false;
            }
        }
    }
}
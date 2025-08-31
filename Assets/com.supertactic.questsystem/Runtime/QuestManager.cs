using Supertactic.GameEvents;
using System.Collections.Generic;
using UnityEngine;

namespace Supertactic.QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        private Dictionary<string, Quest> questMap;
        private List<string> questList;
        private int activeQuest;

        public int ActiveQuest => activeQuest;

        // quest start requirements
        private int currentPlayerLevel;

        public void InitializeQuestMap()
        {
            questMap = CreateQuestMap();
            questList = new List<string>();

            foreach (Quest quest in questMap.Values)
            {
                questList.Add(quest.info.id);
            }

            PrintQuestMap();
        }

        private void PrintQuestMap()
        {
            foreach (var kvp in questMap)
            {
                Debug.Log($"Quest Key: {kvp.Key}, Quest: {kvp.Value.info}");
            }
        }

        private void OnEnable()
        {
            GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
            GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
            GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;

            GameEventsManager.instance.playerEvents.onPlayerLevelChange += PlayerLevelChange;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
            GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
            GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;

            GameEventsManager.instance.playerEvents.onPlayerLevelChange -= PlayerLevelChange;
        }

        private void Start()
        {
            // broadcast the initial state of all quests on startup
            foreach (Quest quest in questMap.Values)
            {
                GameEventsManager.instance.questEvents.QuestStateChange(quest);
            }
        }

        private void Update()
        {
            // loop through all quests
            foreach (Quest quest in questMap.Values)
            {
                // if we're now meeting the requirements, switch over to the CAN_START state
                if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
                {
                    ChangeQuestState(quest.info.id, QuestState.CAN_START);
                }
            }
        }

        private void ChangeQuestState(string id, QuestState state)
        {
            Quest quest = GetQuestById(id);
            quest.state = state;
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }

        // Is used to start a quest without the player actions
        public void InitQuest(int questIndex)
        {
            if (HaveQuestToDo())
            {
                string questId = questList[questIndex];
                StartQuest(questId);
            }
        }

        //public void NextQuest()
        //{
        //    if (CanDoQuest())
        //    {
        //        InitQuest(activeQuest);
        //    }
        //}

        public bool HaveQuestToDo()
        {
            return activeQuest < questList.Count;
        }

        private void StartQuest(string id)
        {
            Quest quest = GetQuestById(id);
            quest.InstantiateCurrentQuestStep(this.transform);
            ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
        }

        private void AdvanceQuest(string id)
        {
            Quest quest = GetQuestById(id);

            // move on to the next step
            quest.MoveToNextStep();

            // if there are more steps, instantiate the next one
            if (quest.CurrentStepExists())
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            // if there are no more steps, then we've finished all of them for this quest
            else
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);

                // this quest will end automatically
                if (quest.info.dontRequireUserConfirmation)
                {
                    GameEventsManager.instance.questEvents.FinishQuestStep(quest.info.id);
                }
            }
        }

        private void FinishQuest(string id)
        {
            Quest quest = GetQuestById(id);
            ClaimRewards(quest);
            ChangeQuestState(quest.info.id, QuestState.FINISHED);
            activeQuest++;

            if (activeQuest >= questList.Count)
            {
                Debug.Log("all the quests were completed");
                GameEventsManager.instance.miscEvents.GameCompleted();
            }
        }

        private void ClaimRewards(Quest quest)
        {
            GameEventsManager.instance.weaponEvents.BowGained(quest.info.bowReward);
            GameEventsManager.instance.goldEvents.GoldGained(quest.info.goldReward);
            GameEventsManager.instance.playerEvents.ExperienceGained(quest.info.experienceReward);
        }

        private void PlayerLevelChange(int level)
        {
            currentPlayerLevel = level;
        }

        private bool CheckRequirementsMet(Quest quest)
        {
            // start true or prove to be false
            bool meetsRequirements = true;

            // check player level requirements
            if (currentPlayerLevel < quest.info.levelRequirement)
            {
                meetsRequirements = false;
            }

            // check quest prerequisites for completion
            foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
            {
                if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
                {
                    meetsRequirements = false;
                    // add this break statement here so that we don't continue on to the next quest, since we've proven meetsRequirements to be false at this point.
                    break;
                }
            }

            return meetsRequirements;
        }

        private Dictionary<string, Quest> CreateQuestMap()
        {
            // Loads all QuestInfoSO Scriptable Objects under the Assets/Resources/Quests folder
            QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
            // create the quest map
            Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

            foreach (QuestInfoSO questInfo in allQuests)
            {
                if (idToQuestMap.ContainsKey(questInfo.id))
                {
                    Debug.LogWarning("Duplicate ID found when creating the quest map " + questInfo.id);
                }
                idToQuestMap.Add(questInfo.id, new Quest(questInfo));
            }
            return idToQuestMap;
        }

        public Quest GetQuestById(string id)
        {
            Quest quest = questMap[id];
            if (quest == null)
            {
                Debug.LogError("ID not found in the Quest Map: " + id);
            }
            return quest;
        }
    }
}

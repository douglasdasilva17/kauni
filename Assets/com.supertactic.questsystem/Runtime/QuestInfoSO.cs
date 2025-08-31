using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Supertactic.QuestSystem
{
    [CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObject/QuestInfoSO", order = 1)]
    public class QuestInfoSO : ScriptableObject
    {
        [field: SerializeField] public string id { get; private set; }

        [Header("General")]
        public string displayName;

        [Header("Requirements")]
        public int levelRequirement;
        public QuestInfoSO[] questPrerequisites;
        // when we are leading with auto quest (If TRUE, the quest will end automatically when CAN_FINISH be the state)
        public bool dontRequireUserConfirmation;

        [Header("Steps")]
        public GameObject[] questStepPrefabs;

        [Header("Rewards")]
        public int goldReward;
        public int experienceReward;
        public int bowReward;

        // ensure the id is always the name of the Scriptable Object
        private void OnValidate()
        {
#if UNITY_EDITOR
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
using Supertactic.GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Supertactic.QuestSystem
{
    public class KillMonsterQuestStep : QuestStep
    {
        private int enemiesDefeated = 0;
        private int enemiesToComplete = 2;

        void OnEnable()
        {
            GameEventsManager.instance.enemyEvents.onEnemyDefeated += EnemyDefeated;
        }

        void OnDisable()
        {
            GameEventsManager.instance.enemyEvents.onEnemyDefeated -= EnemyDefeated;
        }

        private void EnemyDefeated()
        {
            if (enemiesDefeated < enemiesToComplete)
            {
                enemiesDefeated++;
            }

            if (enemiesDefeated >= enemiesToComplete)
            {
                FinishQuestStep();
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supertactic.GameEvents;

namespace Supertactic.QuestSystem
{
    public class CollectBowQuestStep : QuestStep
    {
        private int bowsCollected = 0;
        private int bowsToComplete = 1;

        void OnEnable()
        {
            GameEventsManager.instance.weaponEvents.onBowCollected += BowCollected;
        }
        
        void OnDisable()
        {
            GameEventsManager.instance.weaponEvents.onBowCollected -= BowCollected;
        }

        private void BowCollected()
        {
            if (bowsCollected < bowsToComplete)
            {
                bowsCollected++;
            }

            if (bowsCollected >= bowsToComplete)
            {
                FinishQuestStep();
            }
        }
    }
}
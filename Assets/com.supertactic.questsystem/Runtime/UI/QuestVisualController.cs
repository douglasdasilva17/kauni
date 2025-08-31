using Supertactic.GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Supertactic.QuestSystem
{
    public class QuestVisualController : MonoBehaviour
    {
        [SerializeField] GameObject collectUI;

        private void OnEnable()
        {
            GameEventsManager.instance.weaponEvents.onBowGained += BowGained;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.weaponEvents.onBowGained -= BowGained;
        }

        private void BowGained(int bow)
        {
            StartCoroutine(CollectRoutine());
        }

        private IEnumerator CollectRoutine()
        {
            collectUI.GetComponent<Animator>().SetBool("Display", true);
            yield return new WaitForSeconds(3);
            collectUI.GetComponent<Animator>().SetBool("Display", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Supertactic.GameEvents
{
    public class GameEventsManager : MonoBehaviour
    {
        public static GameEventsManager instance { get; private set; }

        public InputEvents inputEvents;
        public PlayerEvents playerEvents;
        public WeaponEvents weaponEvents;
        public GoldEvents goldEvents;
        public MiscEvents miscEvents;
        public QuestEvents questEvents;
        public EnemyEvents enemyEvents;

        void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Found more than one Game Events Manager in the scene.");
            }
            instance = this;

            //initialize all events
            inputEvents = new InputEvents();
            playerEvents = new PlayerEvents();
            weaponEvents = new WeaponEvents();
            goldEvents = new GoldEvents();
            miscEvents = new MiscEvents();
            questEvents = new QuestEvents();
            enemyEvents = new EnemyEvents();
        }
    }
}

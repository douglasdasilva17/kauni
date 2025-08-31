using System.Collections;
using System.Collections.Generic;
using Supertactic.QuestSystem;
using UnityEngine;

public class CoreManager : MonoBehaviour
{
    [SerializeField] Transform[] enemySpawnPoints;
    [SerializeField] int enemyCount;
    [SerializeField] Transform startPoint;
    private QuestManager questManager;

    private void Start()
    {
        // spawning level
        GameObject levelPrefab = Resources.Load<GameObject>("Levels/Forest Level");
        Instantiate(levelPrefab, this.transform.parent);

        // spawning player
        GameObject playerPrefab = Resources.Load<GameObject>("Characters/Kauni");
        Instantiate(playerPrefab, startPoint.position, startPoint.rotation);

        for (int i = 0; i < enemyCount; i++)
        {
            // spawning enemies
            GameObject enemyPrefab = Resources.Load<GameObject>("Characters/SwampMonster");
            Instantiate(enemyPrefab, enemySpawnPoints[i].position, enemySpawnPoints[i].rotation);
        }

        questManager = FindAnyObjectByType<QuestManager>();

        if (questManager != null)
        {
            questManager.InitializeQuestMap();
        }
    }
}

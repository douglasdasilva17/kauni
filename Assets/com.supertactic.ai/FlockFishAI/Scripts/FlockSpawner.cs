using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using UnityEngine;

namespace Supertactic.Mukani
{
    [System.Serializable]
    public class AIObject
    {
        public string AIGroupName { get { return _aiGroupName; } }
        public GameObject objectPrefab { get { return _objectPrefab; } }
        public int maxAI { get { return _maxAI; } }
        public int spawnRate { get { return _spawnRate; } }
        public int spawnAmount { get { return _spawnAmount; } }
        public bool randomizeStats { get { return _randomizeStats; } }
        public bool enableSpawner { get { return _enableSpawner; } }
        public float spawnScaleFactor { get { return _spawnScaleFactor; } }

        [Header("AI Group Stats")]
        [SerializeField] private string _aiGroupName;
        [SerializeField] private GameObject _objectPrefab;
        [Range(0f, 30f)]
        [SerializeField] private int _maxAI;
        [Range(0f, 20f)]
        [SerializeField] private int _spawnRate;
        [Range(0f, 10f)]
        [SerializeField] private int _spawnAmount;
        [SerializeField] private bool _randomizeStats;
        [SerializeField] private bool _enableSpawner;
        [SerializeField] private float _spawnScaleFactor = 1.5f;

        public AIObject(string name, GameObject prefab, int maxAI, int spawnRate, int spawnAmount, bool randomizeStats)
        {
            this._aiGroupName = name;
            this._objectPrefab = prefab;
            this._maxAI = maxAI;
            this._spawnRate = spawnRate;
            this._spawnAmount = spawnAmount;
            this._randomizeStats = randomizeStats;
        }

        public void SetValues(int maxAI, int spawnRate, int spawnAmount)
        {
            this._maxAI = maxAI;
            this._spawnRate = spawnRate;
            this._spawnAmount = spawnAmount;
        }
    }

    public class FlockSpawner : MonoBehaviour
    {
        [Header("AI Groups Settings")]
        [SerializeField] AIObject[] AIObjects = new AIObject[5];

        public float spawnTimer { get { return _spawnTimer; } }
        public Vector3 spawnArea { get { return _spawnArea; } }

        [Header("Global Stats")]
        [Range(0f, 600f)]
        [SerializeField] float _spawnTimer;
        [SerializeField] Color _spawnColor = new Color(1000f, 0.000f, 0.000f, 0.300f);
        [SerializeField] Vector3 _spawnArea = new Vector3(2f, 1f, 2f);


        private List<Transform> waypoints = new List<Transform>();

        private void Start()
        {
            GetWaypoints();
            RandomizeGroups();
            CreateAIGroups();
            InvokeRepeating("SpawnNPC", 0.5f, spawnTimer);
        }

        private void SpawnNPC()
        {
            for (int i = 0; i < AIObjects.Length; i++)
            {
                if (AIObjects[i].enableSpawner && AIObjects[i].objectPrefab != null)
                {
                    // Make sure that AI group doesnt have max NPCs
                    GameObject tempGroup = GameObject.Find(AIObjects[i].AIGroupName);
                    tempGroup.transform.localPosition = Vector3.zero;

                    if (tempGroup.GetComponentInChildren<Transform>().childCount < AIObjects[i].maxAI)
                    {
                        for (int y = 0; y < UnityEngine.Random.Range(0, AIObjects[i].spawnAmount); y++)
                        {
                            Quaternion randomRotation = Quaternion.Euler(UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(0, 360), 0);
                            GameObject tempSpawn;
                            tempSpawn = Instantiate(AIObjects[i].objectPrefab, RandomPosition(), randomRotation, tempGroup.transform);
                            tempSpawn.AddComponent<FlockUnit>();
                            tempSpawn.GetComponent<FlockUnit>().SetNPCScale(AIObjects[i].spawnScaleFactor);
                            //tempGroup.GetComponent<FlockUnit>().SetupNPC();
                        }
                    }
                }
            }
        }

        // Method for random position within the spawn area
        public Vector3 RandomPosition()
        {
            // Get a random position within our spawn area
            Vector3 randomPosition =
                new Vector3(
                UnityEngine.Random.Range(-spawnArea.x, spawnArea.x),
                UnityEngine.Random.Range(-spawnArea.y, spawnArea.y),
                UnityEngine.Random.Range(-spawnArea.z, spawnArea.z)
                );
            // Debug.Log("Spawn position " + randomPosition);

            randomPosition = transform.TransformDirection(randomPosition * .5f);
            return transform.position + randomPosition;
        }

        // Method for getting a random Waypoint
        public Vector3 RandomWaypoint()
        {
            int randomWp = UnityEngine.Random.Range(0, (waypoints.Count - 1));
            Vector3 randomWaypoint = waypoints[randomWp].transform.position;
            return randomWaypoint;
        }

        // Method for putting random values in the AI Group setting
        private void RandomizeGroups()
        {
            // Randomize
            for (int i = 0; i < AIObjects.Length; i++)
            {
                if (AIObjects[i].randomizeStats)
                {
                    AIObjects[i].SetValues(UnityEngine.Random.Range(1, 30), UnityEngine.Random.Range(1, 20), UnityEngine.Random.Range(1, 10));
                }
            }
        }

        // Method for creating the empty world object groups
        private void CreateAIGroups()
        {
            for (int i = 0; i < AIObjects.Length; i++)
            {
                GameObject AIGroupSpawn;
                AIGroupSpawn = new GameObject(AIObjects[i].AIGroupName);
                AIGroupSpawn.transform.parent = this.transform;
            }
        }

        private void GetWaypoints()
        {
            var wp = transform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < wp.Length; i++)
            {
                if (wp[i].tag == "Waypoint")
                {
                    waypoints.Add(wp[i]);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _spawnColor;
            Gizmos.DrawCube(transform.position, spawnArea);
        }
    }
}

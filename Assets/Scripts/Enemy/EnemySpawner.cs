using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<int> enemyCounts = new List<int>(); [Tooltip("How many enemies to spawn per wave")]
    [SerializeField] private int wave = 0;
    public int EnemiesAlive = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SpawnWave();
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            wave++;
        }

        CheckWaveDone();
    }

    public void SpawnWave()
    {
        if (wave >= enemyCounts.Count)
        {
            wave = enemyCounts.Count;
        }
        for (int i = 0; i < enemyCounts[wave]; i++)
        {
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            float offsetX = Random.Range(1, 3);
            float offsetY = Random.Range(1, 3);
            Vector3 spawnOffset = new Vector3(offsetX, offsetY, 0f);

            Instantiate(enemyPrefab, spawnPoint.position + spawnOffset, spawnPoint.rotation);
            EnemiesAlive++;
        }
    }

    public void EnemyKilled()
    {
        EnemiesAlive--;
    }

    public void CheckWaveDone()
    {
        if (EnemiesAlive == 0)
        {
            SpawnWave();
        }
    }
}
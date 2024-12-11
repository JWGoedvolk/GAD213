using JW.GPG.Procedural;
using SAE.FileSystem;
using SAE.Movement.Enemy;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject fireEnemy;
    [SerializeField] private GameObject iceEnemy;
    [SerializeField] private GameObject acidEnemy;
    [SerializeField] private GameObject plasmaEnemy;
    [Header("Wave Enemy Counts")] // TODO: Add custom inspector for adding waves consistantly and easily at the push of a button
    [SerializeField] public List<int> fireEnemies = new();
    [SerializeField] public List<int> iceEnemies = new();
    [SerializeField] public List<int> acidEnemies = new();
    [SerializeField] public List<int> plasmaEnemies = new();
    [Header("Other")]
    [SerializeField] private GameObject spawnPointObject;
    [SerializeField] private PointExtractor pointExtractor;
    [SerializeField] private List<Transform> spawnPoints = new();
    [SerializeField] private float offsetRange = 1f;
    [SerializeField] public int wave = 0;
    public int EnemiesAlive = 0;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Debug.Log("Enemy spawner waiting for wave info to extract");
        while (!WaveExtractor.WaveInfoExtracted)
        {
            yield return null;
        }
        Debug.Log("Wave info extracted, starting enemy spawning");

        Debug.Log("[STARTUP] Waiting fo points to load in from the texture");
        while (!PointExtractor.PointsLoadedFromFile) { yield return null; }
        LoadSpawnPoints();
        SpawnWave();
    }

    private void LoadSpawnPoints()
    {
        spawnPoints.Clear();
        foreach (var point in pointExtractor.EnemyPoints)
        {
            GameObject spawnObject = Instantiate(spawnPointObject);
            spawnObject.transform.position = new Vector3(point.x, point.y, 0f);
            spawnPoints.Add(spawnObject.transform);
        }
    }

    public void SpawnWave()
    {
        Debug.LogWarningFormat("spawning enemy wave");
        if (wave >= fireEnemies.Count)
        {
            wave--;
        }
        
        for(int fireCount = 0; fireCount < fireEnemies[wave]; fireCount++)
        {
            Vector3 offset = new Vector3(Random.Range(-offsetRange, offsetRange), Random.Range(-offsetRange, offsetRange), 0f);
            int spawnPoint = Random.Range(0, spawnPoints.Count);
            Instantiate(fireEnemy, spawnPoints[spawnPoint].position + offset, Quaternion.identity);
            EnemiesAlive++;
        }
        for (int iceCount = 0; iceCount < iceEnemies[wave]; iceCount++)
        {
            Vector3 offset = new Vector3(Random.Range(-offsetRange, offsetRange), Random.Range(-offsetRange, offsetRange), 0f);
            int spawnPoint = Random.Range(0, spawnPoints.Count);
            Instantiate(iceEnemy, spawnPoints[spawnPoint].position + offset, Quaternion.identity);
            EnemiesAlive++;
        }
        for (int acidCount = 0; acidCount < acidEnemies[wave]; acidCount++)
        {
            Vector3 offset = new Vector3(Random.Range(-offsetRange, offsetRange), Random.Range(-offsetRange, offsetRange), 0f);
            int spawnPoint = Random.Range(0, spawnPoints.Count);
            Instantiate(acidEnemy, spawnPoints[spawnPoint].position + offset, Quaternion.identity);
            EnemiesAlive++;
        }
        for (int plasmaCount = 0; plasmaCount < plasmaEnemies[wave]; plasmaCount++)
        {
            Vector3 offset = new Vector3(Random.Range(-offsetRange, offsetRange), Random.Range(-offsetRange, offsetRange), 0f);
            int spawnPoint = Random.Range(0, spawnPoints.Count);
            Instantiate(plasmaEnemy, spawnPoints[spawnPoint].position + offset, Quaternion.identity);
            EnemiesAlive++;
        }

        wave++;
    }

    public void EnemyKilled()
    {
        EnemiesAlive--;
        CheckWaveDone();
    }

    public void CheckWaveDone()
    {
        if (EnemiesAlive == 0) SpawnWave();
    }
}
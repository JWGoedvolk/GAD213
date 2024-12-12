using JW.GPG.CloudSave;
using SAE.FileSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JW.GPG.Procedural
{
    public class PointExtractor : MonoBehaviour
    {
        [SerializeField] private MetaDataScriptableObject textureFile;
        [SerializeField] private Texture2D textureMap;
        [SerializeField] private GameObject playerPrefab;
        public bool WaveExtracted = false;

        [Header("Points")]
        [SerializeField] GameObject emptyObject;
        [SerializeField] Vector2 scale = Vector2.one;
        public List<Transform> EnemyPoints  = new();
        [SerializeField] private Color enemyColor = Color.red;
        public List<Transform> PlayerPoints = new();
        [SerializeField] private Color playerColor = Color.blue;
        public List<Transform> HazardPoints = new();
        [SerializeField] private Color hazardColor = Color.magenta;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        IEnumerator Start()
        {
            //Debug.Log("[INFO][STARTUP][SYNC] Point Extractor waiting for files to sync");
            while (!GameManager.WaveInfoExtracted)
            {
                yield return null;
            }
            ScreenManager.Instance.SetLoadingText($"[INFO][STARTUP][SYNC] Point Extractor using {textureFile.filename} from sync file");

            textureMap = AssetLoader.LoadTextureFromFile(textureFile.LocalFilePath);
            ExtractPointsFromTexture();

            GameManager.SetupComplete = true;
            ScreenManager.Instance.SetLoadingText("[STARTUP] Finished loading in points from " + textureFile.filename);
        }

        private void ExtractPointsFromTexture()
        {
            for (int y = 0; y < textureMap.height; y++)
            {
                for (int x = 0; x < textureMap.width; x++)
                {
                    Color pixel = textureMap.GetPixel(x, y);
                    Vector2 point = Vector2.one;
                    point.x = x;
                    point.y = y;
                    point -= new Vector2(textureMap.width / 2, textureMap.height / 2);
                    point *= scale;

                    if (pixel == enemyColor)
                    {
                        GameObject spawn = Instantiate(emptyObject, transform);
                        spawn.transform.position = point;
                        spawn.name = $"EnemyPoint({x},{y})";
                        EnemyPoints.Add(spawn.transform);
                    }
                    else if (pixel == playerColor)
                    {
                        GameObject spawn = Instantiate(emptyObject, transform);
                        spawn.transform.position = point;
                        spawn.name = $"PlayerPoint({x},{y})";
                        PlayerPoints.Add(spawn.transform);
                    }
                    else if (pixel == hazardColor)
                    {
                        GameObject spawn = Instantiate(emptyObject, transform);
                        spawn.transform.position = point;
                        spawn.name = $"HazardPoint({x},{y})";
                        HazardPoints.Add(spawn.transform);
                    }
                }
            }

            GameObject player;
            if (PlayerPoints.Count > 0)
            {
                int playerPointIndex = Random.Range(0, PlayerPoints.Count);
                Transform playerPoint = PlayerPoints[playerPointIndex];
                player = Instantiate(playerPrefab, playerPoint.position, Quaternion.identity);
            }
            else
            {
                player = Instantiate(playerPrefab);
            }

            EnemySpawner enemySpawner = GetComponent<EnemySpawner>();
            if (enemySpawner != null) enemySpawner.spawnPoints = EnemyPoints;

            HazardSpawner hazardSpawner = GetComponent<HazardSpawner>();
            if (hazardSpawner != null) hazardSpawner.hazardPoints = HazardPoints;
            {
                hazardSpawner.Player = playerPrefab;
            }
        }
    } 
}

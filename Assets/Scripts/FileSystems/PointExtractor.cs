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
        public static bool PointsLoadedFromFile = false;

        [Header("Points")]
        [SerializeField] Vector2 scale = Vector2.one;
        public List<Vector2> EnemyPoints  = new();
        [SerializeField] private Color enemyColor = Color.red;
        public List<Vector2> PlayerPoints = new();
        [SerializeField] private Color playerColor = Color.blue;
        public List<Vector2> HazardPoints = new();
        [SerializeField] private Color hazardColor = Color.magenta;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        IEnumerator Start()
        {
            PointsLoadedFromFile = false;
            Debug.Log("[INFO][STARTUP][SYNC] Point Extractor waiting for files to sync");
            while (!GameManager.WaveInfoExtracted)
            {
                yield return null;
            }
            ScreenManager.Instance.SetLoadingText($"[INFO][STARTUP][SYNC] Point Extractor using {textureFile.filename} from sync file");

            textureMap = AssetLoader.LoadTextureFromFile(textureFile.LocalFilePath);
            ExtractPointsFromTexture();


            PointsLoadedFromFile = true;
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
                    point -= new Vector2(textureMap.width/2, textureMap.height/2);
                    point *= scale;

                    if (pixel == enemyColor)
                    {
                        EnemyPoints.Add(point);
                    }
                    else if (pixel == playerColor)
                    {
                        PlayerPoints.Add(point);
                    }
                    else if (pixel == hazardColor)
                    {
                        HazardPoints.Add(point);
                    }
                }
            }
        }
    } 
}

using JW.GPG.CloudSave;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SAE.FileSystem
{
    /// <summary>
    /// This extracts and sets up the number of enemies to spawn of each type each wave
    /// 
    /// File format: .txt
    /// fire,ice,acid,plasma;
    /// fire,ice,acid,plasma;
    /// ...
    /// </summary>
    public class WaveExtractor : MonoBehaviour
    {
        [SerializeField] MetaDataScriptableObject waveFile;
        [SerializeField] EnemySpawner enemySpawner;
        [SerializeField] string FileContent;
        [SerializeField] public static bool WaveInfoExtracted = false;
        // Start is called before the first frame update
        IEnumerator Start()
        {
            WaveInfoExtracted = false;
            //Debug.Log("Wave extractor waiting for file sync to finish");
            while(!GameManager.FileSyncCompleted)
            {
                //Debug.Log("Wave extractor waiting for file sync");
                yield return null;
            }
            ScreenManager.Instance.SetLoadingText("[STARTUP][FILE][ENEMY] File sync complete. Start wave infor extraction");
            yield return new WaitForSeconds(1);
            ReadFileContent();
            string[] waves = FileContent.Split(";");
            foreach (var wave in waves)
            {
                if (!string.IsNullOrEmpty(wave))
                {
                    AddNewWave(wave.Trim());
                }
            }
            ScreenManager.Instance.SetLoadingText("[STARTUP][FILE][ENEMY] Wave info extracted");
            yield return new WaitForSeconds(1);
            GameManager.WaveInfoExtracted = true;
            yield return null;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddNewWave(string waveInfo)
        {
            var enemyTypes = waveInfo.Split(',');
            int fireCount   = int.Parse(enemyTypes[0]);
            int iceCount    = int.Parse(enemyTypes[1]);
            int acidCount   = int.Parse(enemyTypes[2]);
            int plasmaCount = int.Parse(enemyTypes[3]);

            enemySpawner.fireEnemies.Add(fireCount);
            enemySpawner.iceEnemies.Add(iceCount);
            enemySpawner.acidEnemies.Add(acidCount);
            enemySpawner.plasmaEnemies.Add(plasmaCount);
        }

        private void ReadFileContent()
        {
            FileContent = File.ReadAllText(waveFile.LocalFilePath);
            FileContent = FileContent.Trim();
        }
    } 
}

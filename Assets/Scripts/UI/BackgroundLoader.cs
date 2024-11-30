using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SAE.FileSystem;
using SAE.Scene.Transition;

namespace SAE.FileSystem
{
    /// <summary>
    /// Loads the background textures.
    /// </summary>
    public class BackgroundLoader : MonoBehaviour
    {
        [Header("Folder Path")]
        //[SerializeField] private string uiPath;
        private string folderDataPath = Application.streamingAssetsPath;
        [SerializeField] private string combinedPath;
        [Header("Files")]
        [SerializeField] private string bgBase;
        [SerializeField] private string bgStars;
        private string combinedStars, combinedBase;
        //[SerializeField] private string bgPlanets;

        [Header("Scene References")]
        [SerializeField] private Image bgBaseUI;
        [SerializeField] private Image bgStarsUI;

        private void Awake()
        {
            //combinedPath = Path.Combine(folderDataPath, uiPath);

            combinedStars = Path.Combine(folderDataPath, bgStars);
            combinedBase  = Path.Combine(folderDataPath, bgBase);
            Debug.Log($"Backround stars path: {combinedStars}");
            Debug.Log($"Backround base  path: {combinedBase}");

            StartCoroutine(LoadBackground());
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        IEnumerator LoadBackground()
        {
            if (AssetLoader.CheckFile(combinedBase))
            {
                Debug.Log("Using streamed background base");
                yield return bgBaseUI.sprite = AssetLoader.LoadSprite(Path.Combine(folderDataPath, bgBase));
            }
            if (AssetLoader.CheckFile(combinedStars))
            {
                Debug.Log("Using streamed background stars");
                yield return bgStarsUI.sprite = AssetLoader.LoadSprite(combinedStars);
            }
        }
    } 
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SAE.FileSystem;

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
        [SerializeField] private string bundleName;
        [SerializeField] private string bgBase;
        [SerializeField] private string bgStars;
        [SerializeField] private string bgMaterial;
        private string combindedBundle, combinedStars, combinedBase, combinedMaterial;
        private AssetBundle backgroundBundle;

        [Header("Scene References")]
        [SerializeField] private Image bgBaseUI;
        [SerializeField] private Image bgStarsUI;

        IEnumerator Start()
        {
            // Wait for the files to be synced for incase we send it a new one through the Google Drive
            while(!GameManager.FileSyncCompleted && !GameManager.UserAuthenticated)
            {
                yield return null;
            }

            combinedStars = Path.Combine(folderDataPath, bgStars);
            combinedBase  = Path.Combine(folderDataPath, bgBase);
            combindedBundle = Path.Combine (folderDataPath, bundleName);
            combinedMaterial = Path.Combine (folderDataPath, bgMaterial);

            StartCoroutine(LoadBackground());
        }

        // Update is called once per frame
        void Update()
        {
        }

        IEnumerator LoadBackground()
        {
            // Check for the asset bundle, but if there are individual files, then prioritise those. So asset bundles are taken as lowest priority.
            if (AssetLoader.CheckFile(combindedBundle))
            {
                Debug.Log("Using asset bundle assets for background");
                // Get the asset bundle
                backgroundBundle = AssetBundle.LoadFromFile(combindedBundle);

                // Base layer
                if (backgroundBundle.Contains(bgBase))
                {
                    var bgbase = backgroundBundle.LoadAsset<Texture2D>(bgBase);
                    bgBaseUI.sprite = AssetLoader.LoadSpriteFromTexture(bgbase);
                }

                // Stars layer
                if (backgroundBundle.Contains(bgStars))
                {
                    var bgstars = backgroundBundle.LoadAsset<Texture2D>(bgStars);
                    bgStarsUI.sprite = AssetLoader.LoadSpriteFromTexture(bgstars);
                }

                if (backgroundBundle.Contains(bgMaterial))
                {
                    Material mat = backgroundBundle.LoadAsset<Material>(bgMaterial);
                    bgStarsUI.material = mat;
                }
            }

            if (AssetLoader.CheckFile(combinedBase))
            {
                Debug.Log("Using streamed background base");
                yield return bgBaseUI.sprite = AssetLoader.LoadSpriteFromFile(Path.Combine(folderDataPath, bgBase));
            }
            if (AssetLoader.CheckFile(combinedStars))
            {
                Debug.Log("Using streamed background stars");
                yield return bgStarsUI.sprite = AssetLoader.LoadSpriteFromFile(combinedStars);
            }
        }
    } 
}

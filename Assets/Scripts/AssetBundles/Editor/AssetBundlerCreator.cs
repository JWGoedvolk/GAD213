using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
public class AssetBundlerCreator
{
    [MenuItem("Assets/Build Asset Bundles")]
    static void BuildAssetBundles()
    {
        string assetBundleDirectory = Application.streamingAssetsPath;
        
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}

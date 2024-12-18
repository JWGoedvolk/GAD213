using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetBundleSettings", menuName = "AssetBundles/New Asset Bundle Settings")]
public class AssetBundleSettings : ScriptableObject
{
    [Tooltip("The directory path to use. 0: Assets/AssetBundles \n" +
        "1: StreamingAssets/AssetBundles\n" +
        "2: Provided directory")]
    public int PathDirectorySetting = 0;
    private string streamingAssetPath = Application.streamingAssetsPath;
    [SerializeField] private string directoryName;
    public string PathDirectory => PathDirectorySetting == 0 ? "Assets/AssetBundles" : PathDirectorySetting == 1 ? Path.Combine(streamingAssetPath, "AssetBundles") : Path.Combine(Application.dataPath, directoryName);
    public List<string> AssetNames = new();
}

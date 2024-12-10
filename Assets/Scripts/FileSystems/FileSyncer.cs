using SAE.FileSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using JW.GPG.CloudSave;

public class FileSyncer : MonoBehaviour
{
    [SerializeField] private List<MetaDataScriptableObject> filesToDownload = new List<MetaDataScriptableObject>();
    [SerializeField] private string driveLink = string.Empty;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckAndDownloadFiles());
    }

    private IEnumerator CheckAndDownloadFiles()
    {
        Debug.Log("[STARTUP][SYNC] Starting file syncing");
        foreach (MetaDataScriptableObject metaData in filesToDownload)
        {
            // Grab the local meta data
            metaData.SetupLocalMetaData();
            Debug.Log($"[SYNC] Syncing {metaData.filename}");

            // Grab the online meta data
            yield return StartCoroutine(DownloadFile(metaData.DirectMetadataDownloadLink, metaData.LocalMetadataFilePath));


            if (metaData.FileNeedsUpdating())
            {
                if (metaData.localIsNewer)
                {
                    // Load local file into byte[] to prepare for updating
                    AssetLoader.LoadFileContent(metaData.LocalFilePath);

                    // Update file on the Drive
                    yield return StartCoroutine(AssetLoader.UpdateFileOnCload(metaData.filename, metaData.associatedFileLink));

                    // Load the local meta data file into the byte[] to prepare for updating
                    AssetLoader.LoadFileContent(metaData.LocalMetadataFilePath);

                    // Update the meta data file on the cloud
                    yield return StartCoroutine(AssetLoader.UpdateFileOnCload(metaData.filename, metaData.metadataFileLink));
                }
                else if (!metaData.localIsNewer)
                {
                    // Delete the local
                    metaData.DeleteLocalFile();

                    // Download new file from the cloud
                    yield return StartCoroutine(AssetLoader.DownloadFileFromCloud(metaData.associatedFileLink, metaData.LocalFilePath));

                    // Update the local meta data
                    yield return StartCoroutine(AssetLoader.DownloadFileFromCloud(metaData.metadataFileLink, metaData.LocalMetadataFilePath));
                }
            }
            else
            {
                Debug.Log($"{metaData.filename} is up to date");
            }
        }
        Debug.Log("[STARTUP][SYNC] Completed file syncing");
    }

    private IEnumerator DownloadFile(string fileLink, string savePath)
    {
        if (string.IsNullOrEmpty(fileLink)) 
        {
            Debug.LogError("Invalid file link provided");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(fileLink);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Falied to download from: {request.error}");
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            File.WriteAllBytes(savePath, request.downloadHandler.data);
            Debug.Log($"File downloaded to: {savePath}");
        }
    }
}

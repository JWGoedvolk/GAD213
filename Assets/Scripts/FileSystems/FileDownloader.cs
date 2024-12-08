using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloader : MonoBehaviour
{
    [SerializeField] private List<MetaDataScriptableObject> filesToDownload = new List<MetaDataScriptableObject>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckAndDownloadFiles());
    }

    private IEnumerator CheckAndDownloadFiles()
    {
        foreach (MetaDataScriptableObject metaData in filesToDownload)
        {
            // Grab the local meta data
            metaData.SetupLocalMetaData();
            // Grab the online meta data
            yield return StartCoroutine(DownloadFile(metaData.DirectMetadataDownloadLink, metaData.LocalMetadataFilePath));


            if (metaData.FileNeedsUpdating())
            {
                // Delete the local
                metaData.DeleteLocalFile();
                // Download the new one
                if (!string.IsNullOrEmpty(metaData.LocalMetadataFilePath) & !string.IsNullOrEmpty(metaData.DirectMetadataDownloadLink)) // We have all the paths to the meta data locations
                {
                    yield return StartCoroutine(DownloadFile(metaData.RemoteFileDownloadLink, metaData.LocalFilePath));
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    Debug.LogError("Failed to obtain a valid download link,  or valid local meta data path");
                }
            }
            else
            {
                Debug.Log($"{metaData.filename} is up to date");
            }



        }
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

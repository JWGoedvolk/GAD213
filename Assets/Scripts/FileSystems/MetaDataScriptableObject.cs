using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JW.GPG.CloudSave
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "file_metadata", menuName = "Metadata/file metadata")]
    public class MetaDataScriptableObject : ScriptableObject
    {
        [Header("Meta Data")]
        public string filename = "file_metadata";
        public string extension = ".json";
        public string metadataFileLink;
        [Header("File Data")]
        public string associatedFileLink;
        public string associatedFileExtension;
        public long tick;
        public bool localIsNewer = false;

        // Runtime properties
        public string LocalMetadataFilePath => Path.Combine(Application.streamingAssetsPath, filename + extension);
        public string LocalFilePath => Path.Combine(Application.streamingAssetsPath, filename + associatedFileExtension);
        public string DirectMetadataDownloadLink => GoogleDriveHelper.ConvertToDirectDownloadLink(metadataFileLink);
        public string RemoteFileDownloadLink { get { return GoogleDriveHelper.ConvertToDirectDownloadLink(associatedFileLink); } set { } }

        public void SetupLocalMetaData()
        {

            if (File.Exists(LocalMetadataFilePath))
            {
                Debug.Log($"{filename} local meta data file exists");
                string localMetaDataContent = File.ReadAllText(LocalMetadataFilePath);
                MetadataFile localMetaData = JsonUtility.FromJson<MetadataFile>(localMetaDataContent);

                if (localMetaData != null)
                {
                    tick = localMetaData.ticks;
                }
            }
        }

        public bool FileNeedsUpdating()
        {
            Debug.Log($"Checking local meta data file at path: {LocalMetadataFilePath}");

            // Step 1: Check if the local meta data file exists
            if (!File.Exists(LocalMetadataFilePath))
            {
                Debug.LogError("Meta data file does not exist, update is required!");
                return true;
            }

            // Step 2: Read the local meta data content
            string metadataContent = File.ReadAllText(LocalMetadataFilePath);
            if (string.IsNullOrEmpty(metadataContent))
            {
                Debug.LogError("Local meta data file is empty, update is required!");
                return true;
            }

            // Step 3: parse the remote meta data content to json
            MetadataFile remoteMetadata = JsonUtility.FromJson<MetadataFile>(metadataContent);
            if (remoteMetadata == null)
            {
                Debug.LogError("Failed to parse remote meta data file to json, update is required");
                return true;
            }
            Debug.Log($"[DEBUG][FILE] Local: {tick} vs Remote: {remoteMetadata.ticks}");
            // Step 4: Compare local and remote meta data versions
            if (tick > remoteMetadata.ticks)
            {
                Debug.Log($"Local file is newer | Local: {tick} vs Remote: {remoteMetadata.ticks}");
                remoteMetadata.ticks = tick; // Update remote tick to match
                localIsNewer = true;
                return true;
            }
            else if (tick < remoteMetadata.ticks)
            {
                Debug.Log($"Remote file is newer | Remote: {remoteMetadata.ticks} vs Local: {tick}");
                tick = remoteMetadata.ticks;
                localIsNewer = false;
                return true;
            }

            Debug.Log($"{filename} is up to date at version: {tick}");
            return false;
        }

        public void DeleteLocalFile()
        {
            if (File.Exists(LocalFilePath))
            {
                File.Delete(LocalFilePath);
                Debug.Log($"Deleting outdated file at {LocalFilePath}");
            }
        }
    } 
}

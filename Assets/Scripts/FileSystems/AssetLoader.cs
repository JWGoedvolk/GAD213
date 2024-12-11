using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGoogleDrive;

namespace SAE.FileSystem
{
    /// <summary>
    /// This class 
    /// </summary>
    public class AssetLoader
    {
        public static byte[] FileContent;

        public static void LoadFileContent(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"[ERROR][FILE] No file found at destination: {path}");
                return;
            }
            FileContent = File.ReadAllBytes(path);
        }

        public static IEnumerator DownloadFileFromCloud(string link, string savePath = "")
        {
            Debug.Log($"[INFO][CLOUD][DOWN] Download file from {link}");
            string id = GoogleDriveHelper.GetIDFromLink(link);
            var request = GoogleDriveFiles.Download(id);
            yield return request.Send();
            if (request != null )
            {
                if (request.IsError)
                {
                    Debug.Log($"[RESULT][DOWNLOAD][{id}] Error: {request.IsError}");
                }
                FileContent = request.ResponseData.Content;
                Debug.Log($"[RESULT][DOWNLOAD][{id}] Create time ticks: {request.ResponseData.CreatedTime.Value.Ticks}");

                // Write the file if a save path is provided
                if (!string.IsNullOrEmpty(savePath))
                {
                    File.WriteAllBytes(savePath, FileContent);
                }

                Debug.Log($"[RESULT][DOWNLOAD][{id}] Finished downoading file");
            }
        }

        public static IEnumerator UpdateFileOnCload(string fileName, string link)
        {
            string id = GoogleDriveHelper.GetIDFromLink(link);
            var updateFile = new UnityGoogleDrive.Data.File() { Content = FileContent };
            var request = GoogleDriveFiles.Update(id, updateFile);
            yield return request.Send();
            if (request != null)
            {
                if (request.IsError)
                {
                    Debug.Log($"[RESULT][UPDATE][{id}] Error: {request.IsError}");
                }
                Debug.Log($"[RESULT][UPDATE][{id}] Finished updating file");
            }
        }

        public static bool CheckFile(string path)
        {
            return File.Exists(path);
        }
        /// <summary>
        /// Loads the given Texture2D from the given filename path
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>Texture2D if the file exists, null if it doesn't</returns>
        public static Texture LoadTexture(string path)
        {
            if (File.Exists(path))
            {
                byte[] imageBytes = File.ReadAllBytes(path);

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);
                return texture;
            }
            else
            {
                Debug.LogError($"Texture at path {path} not found");
                return null;
            }
        }

        /// <summary>
        /// Loads the given Sprite from the given filename path
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>Sprite if the file exists, null if it doesn't</returns>
        public static Sprite LoadSprite(string path)
        {
            Debug.Log($"Loading sprite from {path}");
            if (File.Exists(path))
            {
                byte[] imageBytes = File.ReadAllBytes(path);

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                return sprite;
            }
            else
            {
                Debug.LogError($"Sprite at path {path} not found");
                return null;
            }
        }
    } 
}

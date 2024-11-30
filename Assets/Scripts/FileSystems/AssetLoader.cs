using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SAE.FileSystem
{
    /// <summary>
    /// This class 
    /// </summary>
    public class AssetLoader
    {
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

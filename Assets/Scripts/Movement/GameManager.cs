using JW.GPG.Unlockables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameManager : MonoBehaviour 
{
    public enum GameMode
    {
        Waves,
        Endless,
        Hardcore
    }

    [Header("Game States")]
    public static bool IsPaused = true;
    public static bool UserAuthenticated = false;
    public static bool FileSyncCompleted = false;
    public static bool WaveInfoExtracted = false;
    public static bool PointInfoExtracted = false;
    public static bool UpgradesLoaded = false;
    public static bool SetupComplete = false;

    public static GameMode gameMode = GameMode.Waves;

    IEnumerator Start()
    {
        // Start up sequence
        // check for internet connection
        //   Connection == true
        //     Sync files with Google Drive
        //     Async load wave from text file
        //     Async load points from textures
        //     Async load player profile
        //   Connection == false
        //     Async load wave from local text file
        //     Async load points from local textures
        //     Async load local player profile

        // We have to wait for user authentication before we can continue
        while (!UserAuthenticated)
        {
            yield return null;
        }

        // After user authentications we can sync files
        while (!FileSyncCompleted)
        {
            yield return null;
        }
        // Async run: wave from text, point from texture, loading player profile

        while (!SetupComplete)
        {
            yield return null;
        }

        ScreenManager.Instance.HideLoadingScreen();
        IsPaused = false;
    }

    public void PauseGame()
    {
        IsPaused = true;
    }
    public void UnPauseGame()
    {
        IsPaused = false;
    }
}

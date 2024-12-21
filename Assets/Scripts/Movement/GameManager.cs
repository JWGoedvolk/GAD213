using JW.GPG.Achievements;
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
    public static bool PlayfabIsDoneSaving = false;
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
        IsPaused = true;

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
        //IsPaused = false;
    }

    public void PauseGame()
    {
        IsPaused = true;
    }
    public void UnPauseGame()
    {
        IsPaused = false;
    }

    public void SelectGameMode(int id)
    {
        if (id == 1)
        {
            gameMode = GameMode.Waves;
            ScreenManager.Instance.HideMainMenu();
            IsPaused = false;
        }
        else if (id == 2)
        {
            if (UnlockablesManager.IsItemUnlocked(UnlockablesManager.UnlockableItem.Endless))
            {
                gameMode = GameMode.Endless;
                IsPaused = false;
                ScreenManager.Instance.HideMainMenu();
            }
            else
            {
                ScreenManager.Instance.ShowPopUpWindow("Game Mode Error", $"This game mode has not been unlocked yet.\nYou need the {UnlockablesManager.itemName[UnlockablesManager.UnlockableItem.Endless]} item unlocked first");
            }
        }
        else if (id == 3)
        {
            if (UnlockablesManager.IsItemUnlocked(UnlockablesManager.UnlockableItem.Hardcore))
            {
                gameMode = GameMode.Hardcore;
            }
            else
            {
                ScreenManager.Instance.ShowPopUpWindow("Game Mode Error", $"This game mode is not yet unlockable!\nCheck back in future for possible updates");
            }
        }
    }
}

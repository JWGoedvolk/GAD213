using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static bool IsPaused = true;
    public static bool FileSyncCompleted = false;
    public static bool WaveInfoExtracted = false;
    public static bool SetupComplete = false;

    IEnumerator Start()
    {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static bool IsPaused = false;
    public static bool SetupComplete = false;

    public void PauseGame()
    {
        IsPaused = true;
    }
    public void UnPauseGame()
    {
        IsPaused = false;
    }
}

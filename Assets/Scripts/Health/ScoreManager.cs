using JW.GPG.Achievements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score = 0;
    
    public void AddScore(int amount)
    {
        float toAdd = amount;
        if (GameManager.gameMode == GameManager.GameMode.Endless)
        {
            toAdd = amount * 1.5f;
        }
        else if (GameManager.gameMode == GameManager.GameMode.Hardcore)
        {
            toAdd = amount * 4f;
        }

        Score += (int)toAdd;

        if (Score >= 5000)
        {
            AchievementsManager.UnlockAchievement(AchievementsManager.AchievementType.ImUnstopable);
        }
    }
}

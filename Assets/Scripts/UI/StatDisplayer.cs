using UnityEngine;
using TMPro;
using SAE.Health;
using SAE.Upgrades;
using SAE.Weapons;
using System.Collections.Generic;
using JW.GPG.Achievements;
using System;
using JW.GPG.Unlockables;
using System.Collections;

public class StatDisplayer : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text scroreText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text weaponElementText;
    [SerializeField] private TMP_Text armorElementText;
    [SerializeField] private TMP_Text invincibleText;

    [Header("Stat References")]
    [SerializeField] private ExperienceHandler playerExperience;
    [SerializeField] private HealthManager playerHealth;
    [SerializeField] private ScoreManager playerScore;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private ElementalManager playerDefenseElement;
    [SerializeField] private WeaponSystem playerWeaponElement;

    [Header("Achievements")]
    [SerializeField] List<GameObject> achievementCompletedPanels;
    [SerializeField] List<TMP_Text> achievementPanelTitle;
    [SerializeField] List<TMP_Text> achievementPaneCondition;

    [Header("Unlocks")]
    [SerializeField] List<GameObject> unlockedCompletedPanels;
    [SerializeField] List<TMP_Text> unlockPanelTitle;
    [SerializeField] List<TMP_Text> unlockPanelCondition;

    public void ShowAchievements()
    {
        //AchievementsManager.UpdateAchievementDictionary();
        int index = 0;
        foreach (AchievementsManager.AchievementType achievement in Enum.GetValues(typeof(AchievementsManager.AchievementType)))
        {
            achievementCompletedPanels[index].SetActive(AchievementsManager.IsAchievementUnlocked(achievement));
            achievementPanelTitle[index].text = AchievementsManager.achievementNames[achievement];
            achievementPaneCondition[index].text = AchievementsManager.achievementConditions[achievement];
            index++;
        }
    }

    public void ShowItems()
    {
        int index = 0;
        foreach (UnlockablesManager.UnlockableItem item in Enum.GetValues(typeof(UnlockablesManager.UnlockableItem)))
        {
            unlockedCompletedPanels[index].SetActive(UnlockablesManager.IsItemUnlocked(item));
            unlockPanelTitle[index].text = UnlockablesManager.itemName[item];
            unlockPanelCondition[index].text = UnlockablesManager.itemCondition[item];
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Make the UI text only update when a value is changed

        if (GameManager.SetupComplete)
        {
            if (playerExperience != null)
            {
                xpText.text = $"XP: {playerExperience.currentXP}/{playerExperience.xpToNextLevel}";
            }
            if (playerHealth != null)
            {
                healthText.text = $"Health: {playerHealth.Health}";
                invincibleText.text = playerHealth.isInvul ? "Invincible" : "";
            }
            if (playerScore != null)
            {
                scroreText.text = $"Score: {playerScore.Score}";
            } 
            if (enemySpawner != null)
            {
                waveText.text = $"Wave: {enemySpawner.wave - 1}";
            }
            if (playerDefenseElement != null)
            {
                string elementString = ElementalManager.ReadableElement(playerDefenseElement.element);
                armorElementText.text = $"Armor Element: {elementString}";
            }
            if (playerWeaponElement != null)
            {
                string elementString = ElementalManager.ReadableElement(playerWeaponElement.bulletElement);
                weaponElementText.text = $"Weapon Element: {elementString}";
            }
        }
    }
}

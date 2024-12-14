using SAE.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.Upgrades
{
    /// <summary>
    /// This script handles the storing of current XP and leveling up when we have enough
    /// </summary>
    public class ExperienceHandler : MonoBehaviour
    {
        public float xpToNextLevel = 10f;
        [SerializeField, Tooltip("This is by how much the xp needed to reach the next level will be calculated (xp needed = current needed * scale factor)")] private float scaleFactor = 1.5f;
        public float currentXP = 0f;
        [SerializeField] private UpgradeHandler upgradeHandler;
        [SerializeField] private GameEventScriptable pauseGame;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                CollectXP(1);
            }
        }

        /// <summary>
        /// Adds the given amount of XP to the current total and checks for leveling up, doing so as needed
        /// </summary>
        /// <param name="amount">Amount of XP collected</param>
        public void CollectXP(float amount)
        {
            currentXP += amount;
            if (currentXP >= xpToNextLevel) // TODO: Allow for multiple level ups to be taken
            {
                currentXP -= xpToNextLevel; // This is so we can have xp flow over to the next level if we collect to much
                xpToNextLevel *= scaleFactor;
                pauseGame.Raise();
                GameManager.IsPaused = true;

                int rand = Random.Range(0, 2);
                upgradeHandler.UpgradeType = rand;
                upgradeHandler.ShowUpgrades();
            }
        }
    } 
}

using SAE.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.Upgrades
{
    public class ExperienceHandler : MonoBehaviour
    {
        [SerializeField] public float xpToNextLevel = 10f;
        [SerializeField, Tooltip("This is by how much the xp needed to reach the next level will be calculated (xp needed = current needed * scale factor)")] private float scaleFactor = 1.5f;
        public float currentXP = 0f;
        [SerializeField] private GameObject normalUpgrades, weaponUpgrades;
        [SerializeField] private UpgradeHandler upgradeHandler;
        [SerializeField] private GameEventScriptable pauseGame;
        [SerializeField] private float distance = 5f;

        // Start is called before the first frame update
        void Start()
        {
            upgradeHandler = GetComponent<UpgradeHandler>();
        }

        // Collect XP => amount to add
        //     If we are above the threshold, set the upgrade handler to be able to interact.
        public void CollectXP(float amount)
        {
            currentXP += amount;
            if (currentXP >= xpToNextLevel)
            {
                currentXP -= xpToNextLevel; // This is so we can have xp flow over to the next level if we collect to much
                xpToNextLevel *= scaleFactor;
                upgradeHandler.isLeveledUp = true;
                pauseGame.Raise();

                int rand = Random.Range(0, 2);
                if (rand % 2 == 0)
                {
                    upgradeHandler.ShowUpgrades();
                    //var pos = Random.insideUnitCircle.normalized * distance;
                    //normalUpgrades.transform.position = pos;
                }
                else
                {
                    weaponUpgrades.SetActive(true);
                    //var pos = Random.insideUnitCircle.normalized * distance;
                    //weaponUpgrades.transform.position = pos;
                    upgradeHandler.gameObject.GetComponent<WeaponUpgrade>().SetUpgrades(); 
                }

                //dissolverShow.Raise();
            }
        }
    } 
}

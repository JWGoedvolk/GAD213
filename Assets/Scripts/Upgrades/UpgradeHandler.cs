using JW.GPG.Achievements;
using JW.GPG.Unlockables;
using SAE.EventSystem;
using SAE.Health;
using SAE.Variavles;
using SAE.Weapons;
using SAE.Weapons.Bullets;
using SAE.Weapons.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SAE.Upgrades
{
    /// <summary>
    /// Handles setting the UpgradeIDs on the upgrade buttons. Also does the same for weapon and bullet switching
    /// </summary>
    public class UpgradeHandler : MonoBehaviour
    {
        [Header("UI")]
        public GameObject NormalUpgradePanel;
        public GameObject WeaponUpgradePanel;
        public int UpgradeType = 0;
        [SerializeField] private const int weaponChangeID = 17;
        [SerializeField] private const int bulletChangeID = 18;
        public List<UpgradeButton> NormalButtons;
        public List<UpgradeButton> WeaponButtons; 
        public List<UpgradeButton> BulletButtons;
        [SerializeField] private List<WeaponScriptables> unlockedWeapons = new();
        [SerializeField] private List<BulletScriptable> unlockedBullets = new();
        [SerializeField] private List<WeaponScriptables> allWeapons = new();
        [SerializeField] private List<BulletScriptable> allBullets = new();
        public List<string> UpgradeNames;
        public List<int> UpgradeIDsChosen;
        public List<int> WeaponIDsChosen = new();
        public List<int> BulletIDsChosen = new();
        public int IDChosen = 0;
        public GameEventScriptable UnPause;

        [Header("Stat References")]
        [SerializeField] private Rigidbody2D playerRB;
        [SerializeField] private FloatVarScriptable moveSpeed;
        [SerializeField] private FloatVarScriptable maxSpeed;
        [SerializeField] private FloatVarScriptable turnSpeed;
        [SerializeField] private WeaponSystem weaponSystem;
        [SerializeField] private HealthManager playerHealth;

        [Header("Achievements")]
        public int TimesSkipped = 0;
        [SerializeField] private int lastChosenID = 0;
        [SerializeField] private int timesChosenConsecutivly = 0;
        public int PlayerLevel = 0;

        IEnumerator Start()
        {
            unlockedWeapons.Clear();
            unlockedBullets.Clear();

            while (!GameManager.FileSyncCompleted && !GameManager.UserAuthenticated)
            {
                yield return null;
            }

            //UnlockablesManager.LoadUnlockables();

            // Load all weapons and check for unlocks
            foreach (var weapon in allWeapons)
            {
                if (weapon.Name == "Bolter")
                {
                    weapon.IsUnlocked = UnlockablesManager.IsItemUnlocked(UnlockablesManager.UnlockableItem.Bolter);
                }
                
                if (weapon.IsUnlocked) unlockedWeapons.Add(weapon);
            }

            // Load all bullets and check for unlocks
            foreach (var bullet in allBullets)
            {
                if (bullet.Name == "Torpedo")
                {
                    bullet.IsUnlocked = UnlockablesManager.IsItemUnlocked(UnlockablesManager.UnlockableItem.Torpedo);
                }

                if (bullet.IsUnlocked) unlockedBullets.Add(bullet);
            }
        }

        /// <summary>
        /// Changes the player's stats based on the set UpgradeID int
        /// ID 17, 18 are used for weapon, bullet used respectively
        /// </summary>
        public void Upgrade(int UpgradeID, int extraID = 0)
        {
            if (lastChosenID == UpgradeID)
            {
                timesChosenConsecutivly++;
                if (timesChosenConsecutivly >= 5)
                {
                    AchievementsManager.UnlockAchievement(AchievementsManager.AchievementType.OrSoHelpMe);
                }
            }
            else
            {
                timesChosenConsecutivly = 0;
            }
            lastChosenID = UpgradeID;

            // TODO: Make -1 skip and -2 reroll for ananlytic purposes
            switch (UpgradeID)
            {
                case 0: 
                    Debug.Log("Case 0");
                    break;
                case 1:
                    playerRB.mass += 0.1f;
                    if (playerRB.mass >= 5f)
                    {
                        AchievementsManager.UnlockAchievement(AchievementsManager.AchievementType.OhLawdHeComin);
                        UnlockablesManager.UnlockItem(UnlockablesManager.UnlockableItem.Dreadnaught);
                    }
                    break;
                case 2: playerRB.mass -= 0.1f; break;
                case 3: playerRB.linearDamping -= 0.1f; break;
                case 4: playerRB.linearDamping += 0.1f; break;
                case 5: moveSpeed.Value += 0.5f; break;
                case 6: moveSpeed.Value -= 0.5f; break;
                case 7: maxSpeed.Value += 0.5f; break;
                case 8: maxSpeed.Value -= 0.5f; break;
                case 9: turnSpeed.Value += 0.1f; break;
                case 10:turnSpeed.Value -= 0.1f; break;
                case 11:weaponSystem.fireRateModifier -= 0.1f; break;
                case 12:weaponSystem.fireRateModifier += 0.1f; break;
                case 13:weaponSystem.bulletSpeedModifier += 0.1f; break;
                case 14:weaponSystem.bulletSpeedModifier -= 0.1f; break;
                case 15:weaponSystem.bulletSizeModifier += 0.1f; break;
                case 16:weaponSystem.bulletSizeModifier -= 0.1f; break;
                case weaponChangeID:
                    weaponSystem.weaponStats = unlockedWeapons[extraID];
                    weaponSystem.isFireable = true; 
                    break;
                case bulletChangeID:
                    weaponSystem.bulletStat = unlockedBullets[extraID]; 
                    break;
                case 19: playerHealth.Health = 5; break;
                case 20: weaponSystem.BulletDamageMod += 0.5f; break;
                case 21:
                    weaponSystem.BulletDamageMod -= 0.5f;
                    if (weaponSystem.BulletDamageMod < 0f)
                    {
                        AchievementsManager.UnlockAchievement(AchievementsManager.AchievementType.Medic);
                    }
                    break;
                default: break;
            }
            ShowUpgrades();
            UnPause.Raise();
            PlayerLevel++;
        }

        /// <summary>
        /// Sets the UpgradeIDs in the list of buttons. These IDs do not repeat in the chosen IDs
        /// </summary>
        public void SetUpgrades()
        {
            // Reset the previously chosen upgrades
            UpgradeIDsChosen.Clear();
            WeaponIDsChosen.Clear();
            BulletIDsChosen.Clear();

            if (UpgradeType == 0) // Normal upgrades
            {
                for (int i = 0; i < NormalButtons.Count; i++)
                {
                    do
                    {
                        IDChosen = Random.Range(1, UpgradeNames.Count);
                    } while (UpgradeIDsChosen.Contains(IDChosen) || IDChosen == 17 || IDChosen == 18);
                    UpgradeIDsChosen.Add(IDChosen);
                    NormalButtons[i].UpgradeID = IDChosen;
                    NormalButtons[i].text.text = UpgradeNames[IDChosen];
                }
            }
            else if (UpgradeType == 1) // Weapon and Bullet upgrades
            {
                // Weapons
                for (int i = 0; i < WeaponButtons.Count && i < unlockedWeapons.Count; i++)
                {
                    do
                    {
                        IDChosen = Random.Range(0, unlockedWeapons.Count);
                    }
                    while (WeaponIDsChosen.Contains(IDChosen));
                    WeaponIDsChosen.Add(IDChosen);
                    WeaponButtons[i].UpgradeID = 17;
                    WeaponButtons[i].WeaponID = IDChosen;
                    WeaponButtons[i].text.text = unlockedWeapons[IDChosen].Name;
                }

                // Bullets
                for (int j = 0; j < BulletButtons.Count && j < unlockedBullets.Count; j++)
                {
                    do
                    {
                        IDChosen = Random.Range(0, unlockedBullets.Count);
                    }
                    while (BulletIDsChosen.Contains(IDChosen));
                    BulletIDsChosen.Add(IDChosen);
                    BulletButtons[j].UpgradeID = 17;
                    BulletButtons[j].BulletID = IDChosen;
                    BulletButtons[j].text.text = unlockedBullets[IDChosen].Name;
                }
            }
        }

        /// <summary>
        /// This controlls whether the upgrade panel is hidden or visible
        /// </summary>
        /// <param name="state">bool => the state to set it to</param>
        public void ShowUpgrades()
        {
            

            SetUpgrades();
            if (UpgradeType == 0)
            {
                if (NormalUpgradePanel.activeSelf) NormalUpgradePanel.SetActive(false);
                else NormalUpgradePanel.SetActive(true);
            }
            else if (UpgradeType == 1)
            {
                if (WeaponUpgradePanel.activeSelf) WeaponUpgradePanel.SetActive(false);
                else WeaponUpgradePanel.SetActive(true);
            }
        }

        public void Skip()
        {
            TimesSkipped++;

            // Skip 10 times without leveling up
            if (TimesSkipped >= 3 && PlayerLevel == 0)
            {
                AchievementsManager.UnlockAchievement(AchievementsManager.AchievementType.StrongEnoughAsIs);
            }
            NormalUpgradePanel.SetActive(false);
            WeaponUpgradePanel.SetActive(false);
            UnPause.Raise();
        }

        public void Reroll()
        {
            SetUpgrades();
        }
    } 
}

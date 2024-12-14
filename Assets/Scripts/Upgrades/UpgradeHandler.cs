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
        public List<UpgradeButton> NormalButtons;
        public List<UpgradeButton> WeaponButtons; 
        public List<UpgradeButton> BulletButtons;
        [SerializeField] private List<WeaponScriptables> weapons = new();
        [SerializeField] private List<BulletScriptable> bullets = new();
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


        /// <summary>
        /// Changes the player's stats based on the set UpgradeID int
        /// ID 17, 18 are used for weapon, bullet used respectively
        /// </summary>
        public void Upgrade(int UpgradeID, int extraID = 0)
        {
            switch (UpgradeID)
            {
                case 0:
                    Debug.Log("Case 0");
                    break;
                case 1:
                    playerRB.mass += 0.1f;
                    break;
                case 2:
                    playerRB.mass -= 0.1f;
                    break;
                case 3:
                    playerRB.linearDamping -= 0.1f;
                    break;
                case 4:
                    playerRB.linearDamping += 0.1f;
                    break;
                case 5:
                    moveSpeed.Value += 0.5f;
                    break;
                case 6:
                    moveSpeed.Value -= 0.5f;
                    break;
                case 7:
                    maxSpeed.Value += 0.5f;
                    break;
                case 8:
                    maxSpeed.Value -= 0.5f;
                    break;
                case 9:
                    turnSpeed.Value += 0.1f;
                    break;
                case 10:
                    turnSpeed.Value -= 0.1f;
                    break;
                case 11:
                    weaponSystem.fireRateModifier -= 0.1f;
                    break;
                case 12:
                    weaponSystem.fireRateModifier += 0.1f;
                    break;
                case 13:
                    weaponSystem.bulletSpeedModifier += 0.1f;
                    break;
                case 14:
                    weaponSystem.bulletSpeedModifier -= 0.1f;
                    break;
                case 15:
                    weaponSystem.bulletSizeModifier += 0.1f;
                    break;
                case 16:
                    weaponSystem.bulletSizeModifier -= 0.1f;
                    break;
                case 17:
                    weaponSystem.weaponStats = weapons[extraID];
                    weaponSystem.isFireable = true;
                    break;
                case 18:
                    weaponSystem.bulletStat = bullets[extraID];
                    break;
                case 19:
                    playerHealth.Health = 5;
                    break;
                default:
                    break;
            }
            ShowUpgrades();
            UnPause.Raise();
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
                for (int i = 0; i < WeaponButtons.Count && i < weapons.Count; i++)
                {
                    do
                    {
                        IDChosen = Random.Range(0, weapons.Count);
                    }
                    while (WeaponIDsChosen.Contains(IDChosen));
                    WeaponIDsChosen.Add(IDChosen);
                    WeaponButtons[i].UpgradeID = 17;
                    WeaponButtons[i].WeaponID = IDChosen;
                    WeaponButtons[i].text.text = weapons[IDChosen].Name;
                }

                // Bullets
                for (int j = 0; j < BulletButtons.Count && j < bullets.Count; j++)
                {
                    do
                    {
                        IDChosen = Random.Range(0, bullets.Count);
                    }
                    while (BulletIDsChosen.Contains(IDChosen));
                    BulletIDsChosen.Add(IDChosen);
                    BulletButtons[j].UpgradeID = 17;
                    BulletButtons[j].BulletID = IDChosen;
                    BulletButtons[j].text.text = bullets[IDChosen].Name;
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
            NormalUpgradePanel.SetActive(false);
            WeaponUpgradePanel.SetActive(false);
        }

        public void Reroll()
        {
            SetUpgrades();
        }
    } 
}

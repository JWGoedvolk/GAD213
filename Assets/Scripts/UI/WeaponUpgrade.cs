using SAE.Weapons;
using SAE.Weapons.Bullets;
using SAE.Weapons.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SAE.Upgrades
{
    /// <summary>
    /// Handles setting the player's weapon and bullet upgrades
    /// </summary>
    public class WeaponUpgrade : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private List<UpgradeButton> weaponButtons;
        [SerializeField] private List<UpgradeButton> bulletButtons;
        [SerializeField] private List<WeaponScriptables> weaponsChosen;
        [SerializeField] private List<BulletScriptable> bulletsChosen;

        [Header("Upgrades")]
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private WeaponSystem weaponSystem;
        [SerializeField] private List<WeaponScriptables> weapons;
        [SerializeField] private List<BulletScriptable> bullets;
        [SerializeField] public  GameObject weaponPanel;

        /// <summary>
        /// Sets the UpgradeIDs in the list of buttons. These IDs do not repeat in the chosen IDs
        /// </summary>
        public void SetUpgrades()
        {
            // Setting the weapon upgrades
            weaponsChosen.Clear();
            foreach (var item in weaponButtons)
            {
                int index = 0;
                do
                {
                    index = Random.Range(0, weapons.Count);
                } while (weaponsChosen.Contains(weapons[index]));
                weaponsChosen.Add(weapons[index]);
                item.Weapon = weapons[index];
                item.UpgradeID = 17;
                item.text.text = weapons[index].Name;
            }

            // Setting the bullet upgrades
            bulletsChosen.Clear();
            foreach (var item in bulletButtons)
            {
                int index = 0;
                do
                {
                    index = Random.Range(0, bullets.Count);
                } while (bulletsChosen.Contains(bullets[index]));
                bulletsChosen.Add(bullets[index]);
                item.Bullet = bullets[index];
                item.UpgradeID = 18;
                item.text.text = bullets[index].Name;
            }

        }

        public void SetPanel(bool state)
        {
            if (state)
            {
                SetUpgrades();
            }
            weaponPanel.SetActive(state);
        }

        public void TogglePanel()
        {
            weaponPanel.SetActive(!weaponPanel.activeSelf);
        }
    } 
}

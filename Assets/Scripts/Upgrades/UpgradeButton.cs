using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SAE.EventSystem;
using SAE.Weapons;
using SAE.Variavles;
using SAE.Weapons.Weapons;
using SAE.Weapons.Bullets;
using SAE.Movement.Player;
using SAE.Health;

namespace SAE.Upgrades
{
    /// <summary>
    /// This script is placed on a UI Button and allows for the player's stats to be upgraded with the given ID
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UpgradeButton : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private UpgradeHandler upgradeHandler;
        public TMP_Text text;
        public int UpgradeID = 0;
        public int WeaponID = 0;
        public int BulletID = 0;

        public void Upgrade()
        {
            if (UpgradeID == 17)
            {
                upgradeHandler.Upgrade(UpgradeID, WeaponID);
            }
            else if (UpgradeID == 18)
            {
                upgradeHandler.Upgrade(UpgradeID, BulletID);
            }
            else
            {
                upgradeHandler.Upgrade(UpgradeID);
            }
        }

        public void Skip()
        {
            upgradeHandler.Skip();
        }

        public void Reroll()
        {
            upgradeHandler.Reroll();
        }
    } 
}

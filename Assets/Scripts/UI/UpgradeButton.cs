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

namespace SAE.Upgrades
{
    /// <summary>
    /// This script is placed on a UI Button and allows for the player's stats to be upgraded with the given ID
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UpgradeButton : MonoBehaviour
    {
        [Header("Stat References")]
        public Rigidbody2D RB;
        public WeaponSystem weaponSystem;
        public FloatVarScriptable Speed;
        public FloatVarScriptable MaxSpeed;
        public FloatVarScriptable TurnSpeed;
        public WeaponScriptables Weapon;
        public BulletScriptable Bullet;
        public Movement.Player.Movement movement;

        [Header("Buttons")]
        [SerializeField] private UpgradeHandler upgradeHandler;
        [SerializeField] private WeaponUpgrade weaponUpgrade;
        public Button button;
        public TMP_Text text;
        public int UpgradeID = 0;
        public List<string> upgradeNames = new List<string>();
        public GameEventScriptable Unpause;
        public GameEventScriptable UnTrigger;
        public GameEventScriptable DissolverHide;

        private void OnEnable()
        {
            if (button == null) button = gameObject.GetComponent<Button>();

            // Automatically set the upgrade when enabled
            text.text = upgradeNames[UpgradeID];
            if (UpgradeID == 17) text.text = Weapon.name;
            else if (UpgradeID == 18) text.text = Bullet.name;
        }

        /// <summary>
        /// Changes the player's stats based on the set UpgradeID int
        /// ID 17, 18 are used for weapon, bullet used respectively
        /// </summary>
        public void Upgrade()
        {
            switch (UpgradeID)
            {
                case 0:
                    Debug.Log("Case 0");
                    
                    break;
                case 1:  
                    RB.mass += 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 2:  
                    RB.mass -= 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 3:  
                    RB.drag -= 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 4:  
                    RB.drag += 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 5:  
                    Speed.Value += 0.5f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 6:  
                    Speed.Value -= 0.5f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 7:  
                    MaxSpeed.Value += 0.5f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 8:  
                    MaxSpeed.Value -= 0.5f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 9:  
                    TurnSpeed.Value += 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 10: 
                    TurnSpeed.Value -= 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 11: 
                    weaponSystem.fireRateModifier -= 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 12: 
                    weaponSystem.fireRateModifier += 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 13: 
                    weaponSystem.bulletSpeedModifier += 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 14: 
                    weaponSystem.bulletSpeedModifier -= 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 15: 
                    weaponSystem.bulletSizeModifier += 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 16: 
                    weaponSystem.bulletSizeModifier -= 0.1f;
                    upgradeHandler.ShowUpgrades();
                    break;
                case 17: 
                    weaponSystem.weaponStats = Weapon;
                    weaponSystem.isFireable = true;
                    weaponUpgrade.SetPanel(false);
                    break;
                case 18: 
                    weaponSystem.bulletStat = Bullet;
                    weaponUpgrade.SetPanel(false);
                    break;
                default: 
                    break;
            }
            Unpause.Raise();
            UnTrigger.Raise();
            DissolverHide.Raise();
        }
    } 
}

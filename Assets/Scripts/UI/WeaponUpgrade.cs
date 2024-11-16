using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgrade : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private List<UpgradeButton> weaponButtons;
    [SerializeField] private List<UpgradeButton> bulletButtons;
    [SerializeField] private List<WeaponScriptables> weaponsChosen;
    [SerializeField] private List<BulletScriptable> bulletsChosen;

    [Header("Upgrades")]
    [SerializeField] private WeaponSystem _weaponSystem;
    [SerializeField] private List<WeaponScriptables> weapons;
    [SerializeField] private List<BulletScriptable> bullets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpgrades()
    {
        weaponsChosen.Clear(); // Clearing 
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
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D RB;
    public WeaponSystem weaponSystem;
    public FloatVarScriptable Speed;
    public FloatVarScriptable MaxSpeed;
    public FloatVarScriptable TurnSpeed;
    public WeaponScriptables Weapon;
    public BulletScriptable Bullet;

    [Header("Buttons")]
    public Button button;
    public TMP_Text text;
    public int UpgradeID = 0;
    public  List<string> upgradeNames = new List<string>();
    public GameEventScriptable Unpause;
    public GameEventScriptable UnTrigger;

    private void OnEnable()
    {
        //UpgradeID = Random.Range(1, maxUpgradeID);
        //button.onClick.AddListener(Upgrade);
        text.text = upgradeNames[UpgradeID];
        if      (UpgradeID == 17) text.text = Weapon.name;
        else if (UpgradeID == 18) text.text = Bullet.name;
    }

    private void OnDisable()
    {
        //button.onClick.RemoveListener(Upgrade);
    }

    public void Upgrade()
    {
        switch (UpgradeID)
        {
            case  0: break;
            case  1: RB.mass += 0.1f; break;
            case  2: RB.mass -= 0.1f; break;
            case  3: RB.drag -= 0.1f; break;
            case  4: RB.drag += 0.1f; break;
            case  5: Speed.Value += 0.5f; break;
            case  6: Speed.Value -= 0.5f; break;
            case  7: MaxSpeed.Value += 0.5f; break;
            case  8: MaxSpeed.Value -= 0.5f; break;
            case  9: TurnSpeed.Value += 0.1f; break;
            case 10: TurnSpeed.Value -= 0.1f; break;
            case 11: weaponSystem.fireRateModifier -= 0.1f; break;
            case 12: weaponSystem.fireRateModifier += 0.1f; break;
            case 13: weaponSystem.bulletSpeedModifier += 0.1f; break;
            case 14: weaponSystem.bulletSpeedModifier -= 0.1f; break;
            case 15: weaponSystem.bulletSizeModifier += 0.1f; break;
            case 16: weaponSystem.bulletSizeModifier -= 0.1f; break;
            case 17: weaponSystem.weaponStats = Weapon; break;
            case 18: weaponSystem.bulletStat = Bullet; break;
            default: break;
        }
        Unpause.Raise();
        UnTrigger.Raise();
    }
}

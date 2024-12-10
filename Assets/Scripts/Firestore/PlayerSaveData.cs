using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerSaveData
{
    public int Score;
    public string weaponName;
    public float fireRateModifier;
    public string bulletName;
    public float bulletSpeedModifier;
    public float bulletSizeModifier;
}

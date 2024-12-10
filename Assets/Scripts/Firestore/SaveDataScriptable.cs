using SAE.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JW.GPG.Firestore
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "Firestore/Save Data")]
    public class SaveDataScriptable : ScriptableObject
    {
        public string Name = "";
        public string Password = "";
        public string Email = "";
        public int Score = 0;
        public WeaponSystem weaponSystem;
        public string GetJSONData()
        {
            PlayerSaveData saveData = new();
            saveData.Score = Score;
            saveData.weaponName = weaponSystem.weaponStats.Name;
            saveData.fireRateModifier = weaponSystem.fireRateModifier;
            saveData.bulletName = weaponSystem.bulletStat.Name;
            saveData.bulletSizeModifier = weaponSystem.bulletSizeModifier;
            saveData.bulletSpeedModifier = weaponSystem.bulletSpeedModifier;

            string json = JsonUtility.ToJson(saveData);
            return json;
        }
    } 

    public struct PlayerSaveData
    {
        public int Score;
        public string weaponName;
        public float fireRateModifier;
        public string bulletName;
        public float bulletSpeedModifier;
        public float bulletSizeModifier;
    }
}

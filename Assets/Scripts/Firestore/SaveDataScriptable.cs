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
        public string Data = "";
    }
}

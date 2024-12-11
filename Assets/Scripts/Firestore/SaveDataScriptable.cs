using SAE.Weapons;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        void Awake()
        {
            string str= File.ReadAllText("hello.txt");
            JsonUtility.FromJsonOverwrite(str, this);
        }

    }
}

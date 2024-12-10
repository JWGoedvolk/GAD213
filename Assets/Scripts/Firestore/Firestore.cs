using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using SAE.Weapons;

namespace JW.GPG.Firestore
{
    /// <summary>
    /// This script will handle reading and writing from the Firestore database
    /// </summary>
    public class Firestore : MonoBehaviour
    {
        public FirebaseFirestore db;
        public string CollectionName;
        public WeaponSystem weaponSystem;
        public string Username;
        public string Password;
        public string Email;
        public int Score = 0;
        public PlayerSaveData playerSaveData = new();
        public Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "name", string.Empty },
            { "password", string.Empty },
            { "data", string.Empty }
        };

        private void Awake()
        {
            db = FirebaseFirestore.DefaultInstance;
            Debug.Log($"Database get: {db == null}");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                SaveToCloud();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadFromCloud();
            }
        }

        public void SaveToCloud()
        {
            Debug.Log("[INFO][DB] Saving data to Firestore");
            DocumentReference docRef = db.Collection(CollectionName).Document(Email);
            
            data["name"] = Username;
            data["password"] = Password;

            playerSaveData.weaponName = weaponSystem.weaponStats.Name;
            playerSaveData.bulletName = weaponSystem.bulletStat.Name;
            playerSaveData.bulletSizeModifier = weaponSystem.bulletSizeModifier;
            playerSaveData.bulletSpeedModifier = weaponSystem.bulletSpeedModifier;
            playerSaveData.fireRateModifier = weaponSystem.fireRateModifier;
            playerSaveData.Score = Score;
            string jsonData = JsonUtility.ToJson(playerSaveData);
            Debug.Log("JSON data: " +  jsonData);

            data["data"] = jsonData;

            docRef.SetAsync(data).ContinueWithOnMainThread(task => {
                Debug.Log($"[INFO][DB] Saved data to Firestore database at {CollectionName}/{Email}");
            });

            Debug.Log("[INFO][DB] Saved data to Firestore");
        }

        public void LoadFromCloud()
        {
            Debug.LogWarning("Loading from Firebase Firestore not yet implemented");
        }
    } 
}

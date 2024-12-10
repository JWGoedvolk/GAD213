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
        public SaveDataScriptable NewUser;
        public Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "name", string.Empty },
            { "password", string.Empty },
            { "data", string.Empty }
        };
        public bool PlayerRetrieved = false;
        public bool RetrieveSuccesful = false;

        private void Awake()
        {
            db = FirebaseFirestore.DefaultInstance;
        }

        private void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.V))
            {
                SaveToCloud();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadFromCloud();
            }
            */
        }

        public void SaveToCloud()
        {
            Debug.Log("[INFO][DB][UPLOAD]|START| Saving data to Firestore");
            NewUser = ScriptableObject.CreateInstance<SaveDataScriptable>();
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

            data["data"] = jsonData;

            docRef.SetAsync(data).ContinueWithOnMainThread(task => {
                NewUser.Name = Username;
                NewUser.Password = Password;
                NewUser.Email = Email;
                NewUser.Data = jsonData;
                Debug.Log($"[INFO][DB][UPLOAD]|COMPLETED| Saved data to Firestore database at {CollectionName}/{Email}");
            });
        }

        public List<SaveDataScriptable> GetUserData()
        {
            Debug.Log("[INFO][DB][DOWNLOAD]|START| Starting user list updating");
            List<SaveDataScriptable> users = new List<SaveDataScriptable>();
            Query query = db.Collection(CollectionName);
            query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot queryResult = task.Result;
                if (queryResult.Count > 0)
                {
                    foreach (var item in queryResult)
                    {
                        var user = item.ToDictionary();
                        SaveDataScriptable saveData = new SaveDataScriptable();
                        saveData.Data = user["data"].ToString();
                        saveData.Email = item.Id;
                        saveData.Password = user["password"].ToString();
                        saveData.Name = user["name"].ToString();
                        users.Add(saveData);
                    }
                    Debug.Log($"[INFO][DB][DOWNLOAD]|COMPLETED| User list updating with {queryResult.Count} users retireved");
                    return users;
                }
                else
                {
                    Debug.Log($"[ERROR][DB][DOWNLOAD]|EXITED| User list could not be retrieved");
                    return null;
                }
            });
            return users;
        }

        public void LoadFromCloud()
        {
            Debug.Log($"[INFO][DB] Loading data from Firestore at {CollectionName}/{Email}");
            PlayerRetrieved = false;
            DocumentReference docRef = db.Collection(CollectionName).Document(Email);
            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log($"Loading data from {snapshot.Id}");
                    PlayerRetrieved = true;
                    RetrieveSuccesful = true;
                    Dictionary<string, object> data = snapshot.ToDictionary();
                    foreach (var item in data)
                    {
                        Debug.Log($"{item.Key}: {item.Value}");
                        data[item.Key] = item.Value;
                    }

                    Debug.Log($"[INFO][DB] Finished loading data from {CollectionName}/{Email}");
                }
                else
                {
                    Debug.LogError(string.Format("Document {0} does not exist!", snapshot.Id));
                    PlayerRetrieved = true;
                    RetrieveSuccesful = false;
                }
            });
        }
    } 
}

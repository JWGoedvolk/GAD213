using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace JW.GPG.Unlockables
{
    /// <summary>
    /// This class handles the saving, loading, and unlocking of game content
    /// </summary>
    public static class UnlockablesManager
    {
        private static string filepath = Path.Combine(Application.persistentDataPath, "unlocables.json");

        public enum UnlockableItem
        {
            Bolter, // Reach wave 5 
            Torpedo, // Bullet size >= 5
            HullBreaker, // 50 damage taken
            Dreadnaught, // mass >= 5
            Endless, // Last wave cleared
            Hardcore // 5 minutes in endless
        }
        public static Dictionary<UnlockableItem, string> itemName = new Dictionary<UnlockableItem, string>()
        {
            { UnlockableItem.Bolter,      "Bolter Cannon" },
            { UnlockableItem.Torpedo,     "Torpedo Munition" },
            { UnlockableItem.HullBreaker, "Hullbreaker Hull" },
            { UnlockableItem.Dreadnaught, "Dreadnaught Hull" },
            { UnlockableItem.Endless,     "Endless Mode" },
            { UnlockableItem.Hardcore,    "Hardcore Mode" }
        };
        public static Dictionary<UnlockableItem, string> itemCondition = new Dictionary<UnlockableItem, string>()
        {
            { UnlockableItem.Bolter,      "Reach Wave 5" },
            { UnlockableItem.Torpedo,     "Bullet Size >= 5" },
            { UnlockableItem.HullBreaker, "NYI" },
            { UnlockableItem.Dreadnaught, "NYI" },
            { UnlockableItem.Endless,     "Get Beat The Game achievement" },
            { UnlockableItem.Hardcore,    "Last for 5 minutes in Endless Mode. NYI!" }
        };
        public static Dictionary<UnlockableItem, bool> unlockedItems = new Dictionary<UnlockableItem, bool>();
        public static List<bool> unlockBools = new List<bool>();
        public static string jsonString;

        static UnlockablesManager()
        {
            // TODO: load unlockables from the file if it exists. then load it from Playfab to prioritise the remote version
            
            // Set up the dictionary and list of bools
            InitializeUnlocks();

            // We then load from the file if it exists, or create the file and save to it
            //LoadUnlockables();
        }

        /// <summary>
        /// This populates the dictionary and bool list with all items still locked
        /// </summary>
        private static void InitializeUnlocks()
        {
            Debug.LogWarning("init unlocks");
            unlockBools.Clear();
            foreach (UnlockableItem item in Enum.GetValues(typeof(UnlockableItem)))
            {
                unlockedItems[item] = false;
                unlockBools.Add(false);
            }
            Debug.LogWarning("unlock init done: " + unlockBools.Count);
        }

        /// <summary>
        /// This will check if the save file exists and loaf from it if it does, otherwise it creates the save file
        /// </summary>
        public static void LoadUnlockables()
        {
            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath);

                Unlocks unlocks = JsonUtility.FromJson<Unlocks>(json);
                unlockBools = unlocks.unlockedItems;
                UpdateDictionary();
            }
            else
            {
                SaveUnlockables();
            }
        }

        /// <summary>
        /// This saves a list of bools in a json file
        /// </summary>
        public static void SaveUnlockables()
        {
            string json = FormatJsonString();
            File.WriteAllText(filepath, json);
        }

        /// <summary>
        /// This creates the json string to write to the file
        /// </summary>
        /// <returns>json string of bool list</returns>
        public static string FormatJsonString()
        {
            UpdateBoolList();
            Unlocks unlocks = new Unlocks();
            unlocks.unlockedItems = unlockBools;
            string json = JsonUtility.ToJson(unlocks);
            jsonString = json;
            return jsonString;
        }

        /// <summary>
        /// Saves all the bools from the dictionary into the bool list to make sure its the same
        /// </summary>
        public static void UpdateBoolList()
        {
            int index = 0;
            foreach (var item in unlockedItems)
            {
                unlockBools[index] = item.Value;
                index++;
            }
        }

        /// <summary>
        /// This uses the bool list to update the bools in the dictionary to make sure its up to date
        /// </summary>
        public static void UpdateDictionary()
        {
            int index = 0;
            foreach (UnlockableItem item in Enum.GetValues(typeof(UnlockableItem)))
            {
                unlockedItems[item] = unlockBools[index];
                index++;
            }
        }

        /// <summary>
        /// This checks if the item in the dictionary has been unlocked yet
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>unlock status</returns>
        public static bool IsItemUnlocked(UnlockableItem item)
        {
            return unlockedItems[item];
        }

        /// <summary>
        /// This will set the item to be unlocked in the dictionary if it isn't already. Also shows the pop up window for it
        /// </summary>
        /// <param name="item">Item to unlock</param>
        public static void UnlockItem(UnlockableItem item)
        {
            if (!IsItemUnlocked(item))
            {
                unlockedItems[item] = true;
                ScreenManager.Instance.ShowPopUpWindow($"{itemName[item]} unlocked", itemCondition[item]);
                SaveUnlockables();
            }
        }
    }

    [System.Serializable]
    public class Unlocks
    {
        public List<bool> unlockedItems;
    }
}

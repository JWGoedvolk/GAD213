using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JW.GPG.Unlockables
{
    public static class UnlockablesManager
    {
        private static string filepath = Path.Combine(Application.persistentDataPath, "unlocables.json");


        public enum UnlockableItem
        {
            Bolter, // Reach wave 5 [x]
            Torpedo, // Bullet size >= 5 [x]
            HullBreaker, // 50 damage taken
            Dreadnaught, // mass >= 5
            Endless, // Last wave cleared
            Hardcore // 5 minutes in endless
        }

        private static Dictionary<UnlockableItem, bool> unlockedItems = new Dictionary<UnlockableItem, bool>();

        static UnlockablesManager()
        {
            // TODO: load unlockables from Firestore if possible

            // If we already have an unlockables file, then load it in
            if (File.Exists(filepath))
            {
                UnlockablesManager.LoadUnlockables();
            }

            // Only initialize the dicitionary if it isn't aready
            if (unlockedItems.Count == 0)
            {
                foreach (UnlockableItem item in Enum.GetValues(typeof(UnlockableItem)))
                {
                    unlockedItems[item] = false;
                }
            }
        }

        public static bool IsItemUnlocked(UnlockableItem item)
        {
            return unlockedItems[item];
        }

        public static void UnlockItem(UnlockableItem item)
        {
            if (!IsItemUnlocked(item))
            {
                unlockedItems[item] = true;
                SaveUnlockables();
            }
        }

        public static void SaveUnlockables()
        {
            string json = JsonUtility.ToJson(unlockedItems);
            File.WriteAllText(filepath, json);
        }

        public static void LoadUnlockables()
        {
            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath);
                Dictionary<UnlockableItem, bool> savedUnlockables = JsonUtility.FromJson<Dictionary<UnlockableItem, bool>>(json);
                if (savedUnlockables != null)
                {
                    unlockedItems = savedUnlockables;
                }
            }
            else
            {
                SaveUnlockables();
            }
        }
    } 
}

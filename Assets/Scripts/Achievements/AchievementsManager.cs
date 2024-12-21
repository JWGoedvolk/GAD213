using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace JW.GPG.Achievements
{
	public static class AchievementsManager
	{
		public enum AchievementType
		{
			StrongEnoughAsIs, // Skip 10 without upgrading [x]
			OrSoHelpMe, // Choose the same upgrade 5 times consecutively [x]
			BeatTheGame, // Clear the final wave in Wave mode [x]
			OhLawdHeComin, // Player mass >= 5 [x]
			BigChungusBuild, // Bullet size >= 5 [x]
			ImUnstopable, // Get a score >= 5000 [x]
			Medic // Get negative damage modifier. Cuz then your theoretically healing the enemy [x]
		}
		[SerializeField] public static Dictionary<AchievementType, bool> achievementProgress = new();
		[SerializeField] public static List<bool> achievementBools = new();
        public static Dictionary<AchievementType, string> achievementNames = new()
        {
            { AchievementType.StrongEnoughAsIs, "Strong Enough As Is" },
            { AchievementType.OrSoHelpMe,       "Or So Help Me!" },
            { AchievementType.BeatTheGame,      "Beat The Game" },
            { AchievementType.OhLawdHeComin,    "Oh Lawd He Comin!" },
            { AchievementType.BigChungusBuild,  "Big Chungus" },
			{ AchievementType.ImUnstopable,		"I'm Unstopable!" },
            { AchievementType.Medic,            "Medic!" }
        };
        public static Dictionary<AchievementType, string> achievementConditions = new()
		{
			{ AchievementType.StrongEnoughAsIs, "Skip 10 upgrades while at level 1" },
			{ AchievementType.OrSoHelpMe,       "Choose the same upgrade 5 times in a row" },
			{ AchievementType.BeatTheGame,      "Reach the final wave in Wave mode" },
			{ AchievementType.OhLawdHeComin,	"Raise the player's mass to 5 or more" },
			{ AchievementType.BigChungusBuild,  "Make a bullet 5 times biggger than normal" },
			{ AchievementType.ImUnstopable,		"Get a score higher than 5k (5000)" },
			{ AchievementType.Medic,			"Get a negetive damage modifier, which would heal the enemies instead" }
		};
        public static string jsonString;
		private static string filepath;

		static AchievementsManager()
		{
            // TODO: load achievements from the file if it exists, then load it from Playfab to prioritise the remote version
            InitializeAchievements();

			// Automatically load the achievements from the file if it exists
            filepath = Path.Combine(Application.persistentDataPath, "achievements.json");
			if (File.Exists(filepath))
			{
				Debug.LogWarning("LOading achivements from file");
				LoadAchievements();
			}
		}

		/// <summary>
		/// Sets up the dictionary and list of bools with all the achievements
		/// </summary>
		private static void InitializeAchievements()
		{
			achievementBools.Clear();
			foreach (AchievementType achievement in Enum.GetValues(typeof(AchievementType)))
			{
				achievementProgress[achievement] = false;
				achievementBools.Add(false);
			}
		}

		/// <summary>
		/// Loads the achievements gottent from a file if it exists, otherwise creates and saves current progress to the file
		/// </summary>
        public static void LoadAchievements()
        {
            if (File.Exists(filepath))
            {
                string jsonData = File.ReadAllText(filepath);

                Achievements achievementFile = JsonUtility.FromJson<Achievements>(jsonData);
                achievementBools = achievementFile.achievements;
                UpdateAchievementDictionary();
            }
            else
            {
                InitializeAchievements();
            }
        }

		/// <summary>
		/// This updates the list of bools with the bools from the dictionary
		/// </summary>
        public static void UpdateBoolList()
		{
			int index = 0;
            foreach (var achievement in achievementProgress)
            {
				achievementBools[index] = achievement.Value;
				index++;
            }
        }

		/// <summary>
		/// This updates the dictionary from the list of bools
		/// </summary>
		public static void UpdateAchievementDictionary()
		{
            int index = 0;
            foreach (AchievementType achievement in Enum.GetValues(typeof(AchievementType)))
			{
				achievementProgress[achievement] = achievementBools[index];
				index++;
			}
        }

		/// <summary>
		/// This unlocks, saves, and displayes the pop up window for the given achievement
		/// </summary>
		/// <param name="achievement">The achievement to unlock</param>
		public static void UnlockAchievement(AchievementType achievement)
		{
			if (!achievementProgress[achievement])
			{
				achievementProgress[achievement] = true;
				ScreenManager.Instance.ShowPopUpWindow($"{achievementNames[achievement]} acquired!", achievementConditions[achievement]);
				UpdateBoolList();
				SaveAchievements();
			}
		}

		/// <summary>
		/// This gets the json string from the list of bools
		/// </summary>
		/// <returns></returns>
		public static string FormatJsonString()
		{
			UpdateBoolList();
			var file = new Achievements();
			file.achievements = achievementBools;
			jsonString = JsonUtility.ToJson(file);
			return jsonString;
        }

		/// <summary>
		/// This saves the achievement progress into a json file
		/// </summary>
		public static void SaveAchievements()
		{
			string json = FormatJsonString();
			File.WriteAllText(filepath, json);
		}

		/// <summary>
		/// This checks if the given achievement is unlocked
		/// </summary>
		/// <param name="achievement">the achievement to check</param>
		/// <returns>unlock status</returns>
		public static bool IsAchievementUnlocked(AchievementType achievement)
		{
			return achievementProgress[achievement];
		}
    }

	[System.Serializable]
	public class Achievements
	{
		public List<bool> achievements;
	}
}

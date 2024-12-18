using System;
using System.Collections.Generic;
using System.IO;
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

		private static Dictionary<AchievementType, bool> achievementProgress = new();

		private static string filepath;

		static AchievementsManager()
		{
            // TODO: load achievements from Firestore if possible

            filepath = Path.Combine(Application.persistentDataPath, "achievements.json");

			// Load achievements
			LoadAchievements();
		}

		private static void InitializeAchievements()
		{
			foreach (AchievementType achievement in Enum.GetValues(typeof(AchievementType)))
			{
				achievementProgress[achievement] = false;
			}
			SaveAchievements();
		}

		public static void UnlockAchievement(AchievementType achievement)
		{
			if (!achievementProgress[achievement])
			{
				achievementProgress[achievement] = true;
				SaveAchievements();
			}
		}

		private static void LoadAchievements()
		{
			if (File.Exists(filepath))
			{
				string jsonData = File.ReadAllText(filepath);
				achievementProgress = JsonUtility.FromJson<Dictionary<AchievementType, bool>>(jsonData);
			}
            else
            {
				InitializeAchievements();
            }
        }

		private static void SaveAchievements()
		{
			string json = JsonUtility.ToJson(achievementProgress);
			File.WriteAllText(filepath, json);
		}

		public static bool IsAchievementUnlocked(AchievementType achievement)
		{
			return achievementProgress[achievement];
		}
	} 
}

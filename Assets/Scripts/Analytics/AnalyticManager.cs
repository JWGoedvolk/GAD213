using SAE.Upgrades;
using System.Collections;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;

namespace JW.GPG.Analytics
{
    /// <summary>
    /// This class handles sending events to the Unity Analytics service
    /// </summary>
    public class AnalyticManager : MonoBehaviour
    {
        public static AnalyticManager Instance;

        public UpgradeHandler playerUpgrades;
        public ScoreManager playerScore;
        public ExperienceHandler playerExperience;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        IEnumerator Start()
        {
            // Signleton set up
            if (Instance == null)
            {
                Instance = this;
            }
            if (Instance != this)
            {
                Destroy(this);
            }

            yield return UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
        }
    }

    /// <summary>
    /// A class of static functions for logging data at set evenets, such as on player death
    /// </summary>
    public class AnalyticGameEvent
    {
        public static void OnPlayrDeath(AnalyticManager analyticData)
        {
            CustomEvent OnDeath = new CustomEvent("OnPlayerDeath")
            {
                {"PlayerLevel", analyticData.playerExperience.PlayerLevel },
                {"Score", analyticData.playerScore.Score },
                {"TimesSkipped",  analyticData.playerUpgrades.TimesSkipped}
            };

            AnalyticsService.Instance.RecordEvent(OnDeath);
        }
    }
}

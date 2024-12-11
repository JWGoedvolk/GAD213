using UnityEngine;
using TMPro;
using SAE.Health;
using SAE.Upgrades;

public class StatDisplayer : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text scroreText;
    [SerializeField] private TMP_Text waveText;

    [Header("Stat References")]
    [SerializeField] private ExperienceHandler playerExperience;
    [SerializeField] private HealthManager playerHealth;
    [SerializeField] private ScoreManager playerScore;
    [SerializeField] private EnemySpawner enemySpawner;

    // Update is called once per frame
    void Update()
    {
        // TODO: Make the UI text only update when a value is changed

        if (GameManager.SetupComplete)
        {
            if (playerExperience != null)
            {
                xpText.text = $"XP: {playerExperience.currentXP}/{playerExperience.xpToNextLevel}";
            }
            if (playerHealth != null)
            {
                healthText.text = $"Health: {playerHealth.Health}";
            }
            if (playerScore != null)
            {
                scroreText.text = $"Score: {playerScore.Score}";
            } 
            if (enemySpawner != null)
            {
                waveText.text = $"Wave: {enemySpawner.wave - 1}";
            }
        }
    }
}

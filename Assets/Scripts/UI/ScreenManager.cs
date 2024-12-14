using System.Collections;
using TMPro;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [SerializeField] GameObject LoadingScreenPanel;
    [SerializeField] GameObject DeathScreenPanel;
    [SerializeField] TMP_Text DeathText;
    [SerializeField] ScoreManager playerScore;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] string deathText = "Score: {0}\nWave: {1}";

    [SerializeField] TMP_Text loadingText;
    [SerializeField] TMP_Text dotsText;
    [SerializeField] float loadTextWaitTime = 0.5f;
    [SerializeField] string dots = "";
    [SerializeField] string loadString = "";
    public float ArtificialWaitTime = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }

        //SetLoadingText(loadingText.text);

        StartCoroutine(LoadingTextAnimator());
    }

    public void HideLoadingScreen()
    {
        LoadingScreenPanel.SetActive(false);
    }

    public void SetLoadingText(string text)
    {
        loadString = text;
        loadingText.text = loadString;
    }

    public IEnumerator LoadingTextAnimator()
    {
        //SetLoadingText(loadingText.text);
        while (LoadingScreenPanel.activeSelf)
        {
            dotsText.text = dots;
            dots += ".";

            if (dots.Length > 3) 
            {
                dots = "";
            }

            yield return new WaitForSecondsRealtime(loadTextWaitTime);

        }
        yield return null;
    }

    public void ShowDeathScreen()
    {
        DeathText.text = string.Format(deathText, playerScore.Score, enemySpawner.wave);

        DeathScreenPanel.SetActive(true);
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [Header("Main Menu")]
    [SerializeField] GameObject MainMenuPanel;

    [Header("Pop Up Window")]
    [SerializeField] GameObject popUp;
    [SerializeField] TMP_Text popUpTitleText;
    [SerializeField] TMP_Text popUpBodyText;
    [SerializeField] float popUpTime = 3f;

    [Header("Death Screen")]
    [SerializeField] GameObject DeathScreenPanel;
    [SerializeField] TMP_Text DeathText;
    [SerializeField] ScoreManager playerScore;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] string deathText = "Score: {0}\nWave: {1}";

    [Header("Loading Screen")]
    [SerializeField] GameObject LoadingScreenPanel;
    [SerializeField] TMP_Text loadingText;
    [SerializeField] TMP_Text dotsText;
    [SerializeField] float loadTextWaitTime = 0.5f;
    [SerializeField] string dots = "";
    [SerializeField] string loadString = "";
    public float ArtificialWaitTime = 1f;

    private IEnumerator ShowPopUp(string title, string body)
    {
        popUp.SetActive(true);
        popUpTitleText.text = title;
        popUpBodyText.text = body;
        yield return new WaitForSeconds(popUpTime);
        popUp.SetActive(false);
    }

    public void ShowPopUpWindow(string title, string body)
    {
        StartCoroutine(ShowPopUp(title, body));
    }

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

        //SetLoadingText(loadingText.body);

        StartCoroutine(LoadingTextAnimator());
    }

    public void HideMainMenu()
    {
        MainMenuPanel.SetActive(false);
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
        //SetLoadingText(loadingText.body);
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

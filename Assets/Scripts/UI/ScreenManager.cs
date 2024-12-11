using System.Collections;
using TMPro;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [SerializeField] GameObject LoadingScreenPanel;
    [SerializeField] GameObject DeathScreenPanel;

    [SerializeField] TMP_Text loadingText;
    [SerializeField] float loadTextWaitTime = 0.5f;
    [SerializeField] int numDots = 0;
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
        StopAllCoroutines();
        //StopCoroutine(LoadingTextAnimator());
        StartCoroutine(LoadingTextAnimator());
    }

    public IEnumerator LoadingTextAnimator()
    {
        //SetLoadingText(loadingText.text);
        while (LoadingScreenPanel.activeSelf)
        {
            //Debug.Log(loadingText.text.Length);
            //Debug.Log(dots.Length);

            loadingText.text = loadingText.text + dots;
            dots += ".";

            if (dots.Length > 3) 
            {
                loadingText.text = loadingText.text.Trim().Substring(0, loadingText.text.Length - dots.Length - 2);
                //Debug.Log($"Reset length: {loadingText.text.Length}");

                dots = string.Empty;
            }

            yield return new WaitForSecondsRealtime(loadTextWaitTime);

        }
        yield return null;
    }

    public void ShowDeathScreen()
    {
        DeathScreenPanel.SetActive(true);
    }
}

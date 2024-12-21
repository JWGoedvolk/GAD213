using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using JW.GPG.Achievements;
using System;
using JW.GPG.Unlockables;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class PlayfabManager : MonoBehaviour
{
    public ScoreManager scoreManager;

    public List<bool> BoolsAchievements = new List<bool>();
    public List<bool> BoolsUnlocks = new();
    public List<string> boolStringsAchievements = new();
    public List<string> boolStringsUnlocks = new();

    [SerializeField] private TMP_Text outputText;
    [SerializeField] private StatDisplayer statDisplayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            outputText.text += "Internet not reachable";
            AchievementsManager.LoadAchievements();
            UnlockablesManager.LoadUnlockables();
        }
        else
        {
            LogIn();
        }

        BoolsAchievements = AchievementsManager.achievementBools;
        BoolsUnlocks = UnlockablesManager.unlockBools;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AchievementsManager.UnlockAchievement(AchievementsManager.AchievementType.BeatTheGame);
        }
    }

    void LogIn()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        outputText.text = ("Succesful Log In\n");
        GameManager.UserAuthenticated = true;
        GetData();
    }

    void OnDataRecieved(GetUserDataResult result)
    {
        outputText.text += ("User data sucessfully retrieved\n");
        string unlockJson = result.Data["unlocks"].Value.Trim();
        string achievementJson = result.Data["achievements"].Value.Trim();
        Debug.Log($"unlocks: {unlockJson}");
        Debug.Log($"unlocks: {achievementJson}");
        
        Achievements achievements = JsonUtility.FromJson<Achievements>(achievementJson);
        AchievementsManager.achievementBools = achievements.achievements;
        Unlocks unlocks = JsonUtility.FromJson<Unlocks>(unlockJson);
        UnlockablesManager.unlockBools = unlocks.unlockedItems;

        outputText.text += "Data saved succesfully";
        GameManager.PlayfabIsDoneSaving = true;
        statDisplayer.ShowAchievements();
        statDisplayer.ShowItems();
        //UnlockablesManager.LoadUnlockables();
    }

    void OnError(PlayFabError error)
    {
        outputText.text  = ("Error Loggin In\n");
        outputText.text += (error.GenerateErrorReport());
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        outputText.text += ("Succesfully sent user data update request\n");
        outputText.text += ("Finished playfab saving\n");

    }

    public void GetData()
    {
        outputText.text += ("Starting data retireval\n");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
        outputText.text += ("Done sending data retrieval request\n");
    }

    public void SaveData()
    {
        outputText.text = ("Starting playfab saving\n");

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["unlocks"] = UnlockablesManager.FormatJsonString();
        data["achievements"] = AchievementsManager.FormatJsonString();
        data["score"] = scoreManager.Score.ToString();
        
        BoolsUnlocks = UnlockablesManager.unlockBools;
        BoolsAchievements = AchievementsManager.achievementBools;

        var request = new UpdateUserDataRequest
        {
            Data = data
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }
}

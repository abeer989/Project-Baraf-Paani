using TMPro;
using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;

    [SerializeField] GameObject leaderBoardPanel;
    [SerializeField] Transform entryParent;
    [SerializeField] LeaderBoardEntry entryPrefab;

    [Header("Username")]
    [SerializeField] GameObject namePanel;
    [SerializeField] TMP_InputField nameIPF;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Login();
    }

    void Login()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            //CustomId = SystemInfo.deviceUniqueIdentifier,
            CustomId = "abc" + UnityEngine.Random.Range(0, 1000).ToString(),
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    public void UpdateLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "ScoreLeaderboard",
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    public void GetLeaderboardData()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "ScoreLeaderboard",
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult obj)
    {
        leaderBoardPanel.SetActive(true);

        foreach (PlayerLeaderboardEntry item in obj.Leaderboard)
        {
            //item.Profile.DisplayName
            LeaderBoardEntry entry = Instantiate(original: entryPrefab, parent: entryParent);
            entry.Setup((item.Position + 1).ToString(), item.Profile.DisplayName, item.StatValue.ToString());
        }
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult obj) => Debug.Log("LB update success");

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login success");

        string name = null;

        if (result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;

        if (string.IsNullOrEmpty(name))
            namePanel.SetActive(true);
    }

    private void OnError(PlayFabError obj)
    {
        Debug.Log(obj.GenerateErrorReport());
    }

    public void OnSubmitNameButtonPressed()
    {
        var rqt = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameIPF.text,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(rqt, OnNameUpdated, OnError);
    }

    private void OnNameUpdated(UpdateUserTitleDisplayNameResult obj)
    {
        namePanel.SetActive(false);
        leaderBoardPanel.SetActive(true);
    }
}

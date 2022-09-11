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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Login();
    }

    void Login()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
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
            LeaderBoardEntry entry = Instantiate(entryPrefab, entryParent);
            entry.Setup((item.Position + 1).ToString(), item.PlayFabId.ToString(), item.StatValue.ToString());
        }
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult obj) => Debug.Log("LB update success");

    private void OnSuccess(LoginResult obj) => Debug.Log("Login success");

    private void OnError(PlayFabError obj)
    {
        Debug.Log(obj.GenerateErrorReport());
    }
}

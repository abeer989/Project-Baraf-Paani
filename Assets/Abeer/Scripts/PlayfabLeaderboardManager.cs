using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayfabLeaderboardManager : MonoBehaviour
{
    public static PlayfabLeaderboardManager instance;

    [Space]
    [SerializeField] GameObject leaderBoardPanel;
    [SerializeField] Transform entryParent;
    [SerializeField] LeaderBoardEntry entryPrefab;
    [SerializeField] Button leaderboardButton;

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

    #region Callbacks
    private void OnLeaderboardGet(GetLeaderboardResult obj)
    {
        leaderBoardPanel.SetActive(true);

        for (int i = 0; i < entryParent.childCount; i++)
            Destroy(entryParent.GetChild(i).gameObject);

        foreach (PlayerLeaderboardEntry item in obj.Leaderboard)
        {
            LeaderBoardEntry entry = Instantiate(original: entryPrefab, parent: entryParent);

            if (item.Profile.DisplayName == null)
                entry.Setup((item.Position + 1).ToString(), item.PlayFabId, item.StatValue.ToString()); 

            else
                entry.Setup((item.Position + 1).ToString(), item.DisplayName, item.StatValue.ToString());
        }
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult obj) => Debug.Log("LB update success");

    private void OnError(PlayFabError obj) => Debug.Log(obj.GenerateErrorReport());
    #endregion
}

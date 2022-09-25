using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using System;

public class PlayFabManager : MonoBehaviour
{

    //public Action<LoginResult> onLoginSuccessEvent;
    public Action<PlayFabError> onLoginFailEvent;

    public Action<UpdateUserTitleDisplayNameResult> onNameUpdateSuccessEvent;
    public Action<PlayFabError> onNameUpdateFailEvent;

    public Action<UpdatePlayerStatisticsResult> onLBSuccessEvent;
    public Action<PlayFabError> onLBFailEvent;

    public Action<GetLeaderboardResult> onLBGetSuccessEvent;
    public Action<GetLeaderboardAroundPlayerResult> onLBGetAroundPlayerSuccessEvent;
    public Action<PlayFabError> onLBGetFailEvent;

    const string runnerGW_LBKey = "runnerGamesWon";
    const string runnerGL_LBKey = "runnerGameLost";
    const string ruunerHS_LBKey = "runnerPlayScore";
    
    const string seekerGW_LBKey = "seekerGamesWon";
    const string seekerGL_LBKey = "seekerGamesLost";
    const string seekerHS_LBKey = "seekerPlayScore";

    public PlayerProfileModel playerProfile;
    public UserAccountInfo playerAccountInfo;

    #region Login Related

    string playFabIDKey = "playfabId";

    public void LoginCustomID()
    {
        Debug.Log("Login ...");
       

        if (/*PlayerPrefs.HasKey(playFabIDKey)*/ PlayerPrefs.HasKey(playFabIDKey))
        {
            Debug.Log("Login ...");
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
            {
                CustomId = PlayerPrefs.GetString(playFabIDKey),

                CreateAccount = true,

                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                    GetUserAccountInfo = true
                }
            };

            PlayFabClientAPI.LoginWithCustomID(request,
           OnLoginSuccess,
           OnError);
        }
        else
        {
            Debug.Log("Login ...");
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
            {

                CustomId = $"USER{UnityEngine.Random.Range(0,999)}{UnityEngine.Random.Range(0, 999)}",

                CreateAccount = true,

                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                    GetUserAccountInfo = true
                }
            };

            PlayFabClientAPI.LoginWithCustomID(request,
           OnLoginSuccess,
           OnError);
        }

        Debug.Log("Login ...");

       

    }

    void OnLoginSuccess(LoginResult result)
    {


        Debug.Log("OnLoginSuccess");
        if(result.InfoResultPayload.AccountInfo !=null)
        {
            Debug.Log("OnLoginSuccess");
            playerAccountInfo = result.InfoResultPayload.AccountInfo;
        }

        if(result.InfoResultPayload.PlayerProfile !=null)
        {
            Debug.Log("OnLoginSuccess");
            playerProfile = result.InfoResultPayload.PlayerProfile;

            //Debug.Log($"{playerProfile}");

            
                //PlayerPrefs.SetString(playFabIDKey, playerProfile.PlayerId);
               // PlayerPrefs.Save();
            

                

        }

        Debug.Log($"{result.InfoResultPayload.AccountInfo.CustomIdInfo.CustomId}");

        PlayerPrefs.SetString(playFabIDKey, result.InfoResultPayload.AccountInfo.CustomIdInfo.CustomId);
        PlayerPrefs.Save();


    }

    public void UpdateName(string name)
    {
        var rqt = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(rqt,
            onNameUpdateSuccessEvent,
            OnError);

    }

    #endregion

    #region Leader Board Related

    public void UpdateLB_Runner(int gameScore,bool isWon,int playScore)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = isWon?runnerGW_LBKey:runnerGL_LBKey,
                    Value = gameScore

                },

                new StatisticUpdate
                {
                    StatisticName = ruunerHS_LBKey,
                    Value = playScore
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
            OnLeaderboardUpdate,
            OnError);
    }

    public void UpdateLB_Seeker(int gameScore, bool isWon, int playScore)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = isWon?seekerGW_LBKey:seekerGL_LBKey,
                    Value = gameScore

                },

                new StatisticUpdate
                {
                    StatisticName = seekerHS_LBKey,
                    Value = playScore
                }

            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
            OnLeaderboardUpdate,
            OnError);
    }

    public void GetLB_Runner()
    {
        var req = new GetLeaderboardRequest
        {
            StatisticName = runnerGW_LBKey,
            StartPosition = 0,
            MaxResultsCount = 20,

        };

        PlayFabClientAPI.GetLeaderboard(req,
            onLBGetSuccessEvent,
           OnError);
    }

    public void GetLB_Seeker()
    {
        var req = new GetLeaderboardRequest
        {
            StatisticName = seekerGW_LBKey,
            StartPosition = 0,
            MaxResultsCount = 20,

        };

        PlayFabClientAPI.GetLeaderboard(req,
            onLBGetSuccessEvent,
            OnError);
    }


    public void GetLBAroundPlayer_Seeker()
    {
        var req = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = seekerGW_LBKey,
            MaxResultsCount = 20,

        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(req,
            onLBGetAroundPlayerSuccessEvent,
            OnError);
    }
    public void GetLBAroundPlayer_Runner()
    {
        var req = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = runnerGW_LBKey,
            MaxResultsCount = 20,

        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(req,
            onLBGetAroundPlayerSuccessEvent,
            OnError);
    }
    #endregion

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult obj) => Debug.Log("LB update success");

    private void OnError(PlayFabError obj) => Debug.Log(obj.GenerateErrorReport());
}

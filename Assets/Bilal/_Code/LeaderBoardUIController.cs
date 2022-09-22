using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using PlayFab.ClientModels;
using PlayFab;

public class LeaderBoardUIController : MonoBehaviour
{
    [SerializeField] CanvasGroup leaderBoardCanvasGroup;

    [SerializeField] Button backBtn;

    [SerializeField] Button seekerLBBtn;
    [SerializeField] Button runnerLBBtn;

    [SerializeField] Button refreshLBBtn;
    [SerializeField] Button getLBAroundPlayerBtn;

    [SerializeField] LBEntryUIController leaderBoardUIItem;

    [SerializeField] Transform contentParent;

    [SerializeField] PlayFabManager playfabManagerInstance;

    public Action onBackClick;

    [Header("TweenControls")]
    [SerializeField] float fadeDuration;


    List<LBEntryUIController> lbUIItemList = new List<LBEntryUIController>();

    enum lbSelected { runner,seeker}
    lbSelected lbselectedEnum = lbSelected.runner;

    private void Start()
    {
        playfabManagerInstance.onLBGetSuccessEvent = PopulateLeaderBoardUI;
        playfabManagerInstance.onLBGetAroundPlayerSuccessEvent = PopulateLeaderBoardUIAroundPlayer;

        seekerLBBtn.onClick.AddListener(delegate 
        {
            lbselectedEnum = lbSelected.seeker;
            playfabManagerInstance.GetLB_Seeker();
        });

        runnerLBBtn.onClick.AddListener(delegate 
        {
            lbselectedEnum = lbSelected.runner;
            playfabManagerInstance.GetLB_Runner();
        });

        refreshLBBtn.onClick.AddListener(delegate 
        {
            if(lbselectedEnum == lbSelected.runner)
                playfabManagerInstance.GetLB_Runner();
            else
                playfabManagerInstance.GetLB_Seeker();

        });

        getLBAroundPlayerBtn.onClick.AddListener(delegate 
        {
            if (lbselectedEnum == lbSelected.runner)
                playfabManagerInstance.GetLBAroundPlayer_Runner();
            else
                playfabManagerInstance.GetLBAroundPlayer_Seeker();
        });




        backBtn.onClick.AddListener(delegate { onBackClick(); });
    }



    public void FadeOutPanel()
    {
        leaderBoardCanvasGroup.DOFade(0, fadeDuration);
        leaderBoardCanvasGroup.interactable = false;
        leaderBoardCanvasGroup.blocksRaycasts = false;
    }

    public void FadeInPanel()
    {
        leaderBoardCanvasGroup.DOFade(1, fadeDuration);
        leaderBoardCanvasGroup.interactable = true;
        leaderBoardCanvasGroup.blocksRaycasts = true;
    }

    private void PopulateLeaderBoardUI(GetLeaderboardResult obj)
    {
      //  foreach(var item in )

        foreach(var lb in lbUIItemList)
        {
            Destroy(lb.gameObject);
        }

        lbUIItemList.Clear();

        foreach (PlayerLeaderboardEntry item in obj.Leaderboard)
        {
            LBEntryUIController entry = Instantiate(leaderBoardUIItem, contentParent);
            if(item.Profile.DisplayName == null)
            {
                entry.SetUpEntry(item.Position, item.PlayFabId, item.StatValue);
            }
            else
            {
                entry.SetUpEntry(item.Position, item.DisplayName, item.StatValue);
            }

            lbUIItemList.Add(entry);
        }
    }

    private void PopulateLeaderBoardUIAroundPlayer(GetLeaderboardAroundPlayerResult obj)
    {
        //  foreach(var item in )

        foreach (var lb in lbUIItemList)
        {
            Destroy(lb.gameObject);
        }

        lbUIItemList.Clear();

        foreach (PlayerLeaderboardEntry item in obj.Leaderboard)
        {
            LBEntryUIController entry = Instantiate(leaderBoardUIItem, contentParent);
            if (item.Profile.DisplayName == null)
            {
                entry.SetUpEntry(item.Position, item.PlayFabId, item.StatValue);
            }
            else
            {
                entry.SetUpEntry(item.Position, item.DisplayName, item.StatValue);
            }

            lbUIItemList.Add(entry);
        }
    }

    private void OnLBError(PlayFabError obj) => Debug.Log(obj.GenerateErrorReport());

}

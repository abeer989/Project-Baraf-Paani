using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;


public class GameManager_Bilal : MonoBehaviourPunCallbacks
{
    public static GameManager_Bilal instance;

    private void Awake()
    {
        instance = this;
    }


    public SpawnPlayersHandler spawnPlayersInstance;
    public GamePhotonManager photonManagerInstance;

    public GameUIManager uiManagerInstance;
    public TimerHandler gameTimerInstance;

    bool isGameActive = false;
   


    bool areAllFrozen = false;
    bool isTimeOver = false;

    private void Start()
    {
        spawnPlayersInstance.SpawnPlayers();

        uiManagerInstance.SetStartBtnInteractibility(PhotonNetwork.IsMasterClient); // set interactibility if is master client

        uiManagerInstance.onStartClicked = StartGame;
        gameTimerInstance.onTimerFinishedEvent = WhenTimeOverFunction;
        uiManagerInstance.onPlayAgainClicked = WhenPlayAgain;

    }

    public void StartGame()
    {
        uiManagerInstance.SetStartBtnInteractibility(false);
        //StartCoroutine(StartGameCoroutine());
        base.photonView.RPC(nameof(photonManagerInstance.RPC_StartGame),RpcTarget.All, PhotonNetwork.Time);
        
    }




    public IEnumerator StartGameCoroutine(double photonTime)
    {
        Debug.Log("Starting Game...");

        

        if(PhotonNetwork.IsMasterClient)
        {
            DetermineSeeker();
        }

        isGameActive = true;
        // run time and check for time
        // catching mechanic
        gameTimerInstance.InitializeTimer(photonTime);
        yield return new WaitUntil( ()=> CheckIFEveryRunnerIsFrozen() || isTimeOver);

        gameTimerInstance.StopTimer();
        
        isGameActive = false;

        if(areAllFrozen)
        {
            uiManagerInstance.SetActiveGameOverPanel(true);
            uiManagerInstance.SetGameOverText("Seeker Won The Game");
        }
        else
        {
            uiManagerInstance.SetActiveGameOverPanel(true);
            uiManagerInstance.SetGameOverText("Time Over! The runner team has won");
        }
        // open ui screen;

        yield return null;

    }

    public void WhenTimeOverFunction()
    {
        isTimeOver = true;
        
    }
    
    public void WhenPlayAgain()
    {
        uiManagerInstance.SetActiveGameOverPanel(false);

        areAllFrozen = false;
        isTimeOver = false;
        isGameActive = false;

        foreach(var player in spawnPlayersInstance.playersInRoom_List)
        {
            player.ResetPlayerParams();
        }

        if(PhotonNetwork.IsMasterClient)
        {
            uiManagerInstance.SetStartBtnInteractibility(true);
        }
    }

    #region MASTERCLIENT FUNCTIONS
    public void DetermineSeeker()
    {


        var playersList = spawnPlayersInstance.playersInRoom_List;

        Debug.Log($"Players List Gotten -> {playersList.Count}");

        var seekerIndex = Random.Range(0, playersList.Count);

        Debug.Log($"Seeker Index -> {seekerIndex}");

        var playerManager = playersList[seekerIndex];

        Debug.Log($"playerManager Found -> {playerManager}");

        Debug.Log($"Seeker Dteremine {playerManager.photonView.ViewID}");

        base.photonView.RPC(nameof(photonManagerInstance.RPC_SetSeeker), RpcTarget.All, playerManager.photonView.ViewID);

    }

    public bool CheckIFEveryRunnerIsFrozen()
    {
        var playerList = spawnPlayersInstance.playersInRoom_List;

        

        for(int i=0; i< playerList.Count;i++)
        {
            if(!playerList[i].isSeeker && !playerList[i].isFrozen)
            {
                return false;
            }
        }

        areAllFrozen = true;

        return true;
    }

    #endregion


    #region Game Start Functions

    public void RPC_Freeze(int viewID)
    {
        base.photonView.RPC(nameof(photonManagerInstance.RPC_SetFrozen),RpcTarget.All,viewID);
    }

    public void RPC_UnFreeze(int viewID)
    {
        base.photonView.RPC(nameof(photonManagerInstance.RPC_SetUnfrozen), RpcTarget.All, viewID);
    }
    public void FreezeTargetPlayer(int viewID)
    {
        var playerManager = spawnPlayersInstance.GetPlayerWithViewId(viewID);

        playerManager.FreezePlayer();
    }

    public void UnFreezeTargetPlayer(int viewID)
    {
        var playerManager = spawnPlayersInstance.GetPlayerWithViewId(viewID);

        playerManager.UnFreezePlayer();
    }


    public void SetSeeker(int viewId)
    {
        Debug.Log("Setting Seekers and Runners");

        var playersList = spawnPlayersInstance.playersInRoom_List;

        foreach (var player in playersList)
        {
            Debug.Log($" player view ID-> {player.photonView.ViewID}");

            if (viewId == player.photonView.ViewID)
            {
                Debug.Log("Setting Seeker");
                player.isSeeker = true;
                player.indicatorInstance.SetIndicator(true);
            }
            else
            {
                Debug.Log("Setting Non Seeker");

                player.isSeeker = false;
                player.indicatorInstance.SetIndicator(false);
            }
        }

    }

    public bool GetGameActiveStatus()
    {
        return isGameActive;
    }

    #endregion







}

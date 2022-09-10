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

    GameState gameState = GameState.Idle;


    bool areAllFrozen = false;
    bool isTimeOver = false;

    enum GameState 
    {
        Idle,
        SetUP,
        SeekerDecided,
        GameStart,
        GameEnd,
    }

    private void Start()
    {
        spawnPlayersInstance.SpawnPlayers();

        uiManagerInstance.SetStartBtnInteractibility(PhotonNetwork.IsMasterClient); // set interactibility if is master client

        uiManagerInstance.onStartClicked = StartGame;
        gameTimerInstance.onTimerFinishedEvent = WhenTimeOverFunction;
        uiManagerInstance.onPlayAgainClicked = PlayAgainEvent;
        uiManagerInstance.onleaveBtnClicked = LeaveRoom;
    }

    private void Update()
    {
        uiManagerInstance.SetPingTxt( PhotonNetwork.GetPing().ToString());
    }

    public void StartGame()
    {
        
        uiManagerInstance.SetStartBtnInteractibility(false);
        //StartCoroutine(StartGameCoroutine());
        base.photonView.RPC(nameof(RPC_StartGame),RpcTarget.All, PhotonNetwork.Time);

        // Close Room when game has started
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;


    }




    public IEnumerator StartGameCoroutine(double photonTime)
    {
        Debug.Log("Starting Game...");
        gameState = GameState.SetUP;


        if (PhotonNetwork.IsMasterClient)
        {
            var vId = DetermineSeeker();

            base.photonView.RPC(nameof(RPC_SetSeekerAndStartGameTimer), RpcTarget.All, vId);

            
            


        }


        yield return new WaitUntil(() => gameState == GameState.SeekerDecided);
        
        spawnPlayersInstance.SpawnAllPlayersInGameReadyLocations();

        yield return StartCoroutine(RoundStartCountDownTimer(photonTime));



        gameState = GameState.GameStart;
        isGameActive = true;



        // wait for game start timer

        // run time and check for time
        // catching mechanic
        gameTimerInstance.InitializeTimer(photonTime);
        yield return new WaitUntil( ()=> CheckIFEveryRunnerIsFrozen() || isTimeOver);

        gameTimerInstance.StopTimer();

        gameState = GameState.GameEnd;
        isGameActive = false;

        if (areAllFrozen)
        {
            uiManagerInstance.SetActiveGameOverPanel(true);
            uiManagerInstance.SetGameOverText("Seeker Won The Game");
        }
        else
        {
            uiManagerInstance.SetActiveGameOverPanel(true);
            uiManagerInstance.SetGameOverText("Time Over! The runner team has won");
        }


        yield return null;

    }

    public void WhenTimeOverFunction()
    {
        isTimeOver = true;
        
    }


    #region OnClickFunctions
    public void PlayAgainEvent()
    {
        uiManagerInstance.SetActiveGameOverPanel(false);

        areAllFrozen = false;
        isTimeOver = false;
        isGameActive = false;

        foreach (var player in spawnPlayersInstance.playersInRoom_List)
        {
            player.ResetPlayerParams();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            uiManagerInstance.SetStartBtnInteractibility(true);
        }
    }
    #endregion



    #region MASTERCLIENT FUNCTIONS
    public int DetermineSeeker()
    {


        var playersList = spawnPlayersInstance.playersInRoom_List;

        Debug.Log($"Players List Gotten -> {playersList.Count}");

        var seekerIndex = Random.Range(0, playersList.Count);

        Debug.Log($"Seeker Index -> {seekerIndex}");

        var playerManager = playersList[seekerIndex];

        Debug.Log($"playerManager Found -> {playerManager}");

        Debug.Log($"Seeker Dteremine {playerManager.photonView.ViewID}");

        if (!playerManager)
            return -1;

        return playerManager.photonView.ViewID;

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

    void UnlockEveryPlayer()
    {
        foreach (var player in spawnPlayersInstance.playersInRoom_List)
        {
            player.UnlockPlayer();

            
        }
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

        gameState = GameState.SeekerDecided;

    }



    #endregion

    #region Utility Functions
    public bool GetGameActiveStatus()
    {
        return isGameActive;
    }

    IEnumerator RoundStartCountDownTimer(double photonTime)
    {
        var oneSecDelay = new WaitForSeconds(1);

        uiManagerInstance.SetCoundDownTimerTxt("Ready!!!!");

        yield return oneSecDelay;

        double timerIncValue = 0.0f;
        double startTime = photonTime;
        double timer = 5.0f;

        while(timerIncValue < timer)
        {
            timerIncValue = PhotonNetwork.Time - startTime;
            var t = timer - timerIncValue;
            uiManagerInstance.SetCoundDownTimerTxt(((int)t).ToString());
            yield return null;
        }

        uiManagerInstance.SetCoundDownTimerTxt("START!!!!");
        UnlockEveryPlayer();
        yield return oneSecDelay;
        uiManagerInstance.SetCoundDownTimerTxt("");



        yield return null;

        

    }

    #endregion



    #region Photon Functions

    public void LeaveRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }

    [PunRPC]
    public void RPC_StartGame(double photonTime)
    {
        StartCoroutine( StartGameCoroutine(photonTime));
    }

    [PunRPC]
    public void RPC_SetSeekerAndStartGameTimer(int viewId)
    {
        SetSeeker(viewId);

        //spawnPlayersInstance.SpawnAllPlayersInGameReadyLocations();

    }

    #endregion



}

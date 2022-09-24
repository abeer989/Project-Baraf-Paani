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

    public PowerUpUIEffectController powerUpUIeffectControllerInstance;

    bool isGameActive = false;

    GameState gameState = GameState.Idle;


    bool areAllFrozen = false;
    bool isTimeOver = false;

    //List<Player> playersInRoomList = new List<Player>();

    [SerializeField] GameObject freezeGunPowerUpPrefab;
    [SerializeField] GameObject invincibilityPowerUpPrefab;


    [SerializeField] PlayFabManager playfabManager;


 
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
        

       // uiManagerInstance.SetStartBtnInteractibility(PhotonNetwork.IsMasterClient); // set interactibility if is master client

        uiManagerInstance.onStartClicked = StartGame;
        gameTimerInstance.onTimerFinishedEvent = WhenTimeOverFunction;
        uiManagerInstance.onBackToRoomClicked = LeaveRoom;
        uiManagerInstance.onleaveBtnClicked = LeaveRoom;
        uiManagerInstance.onReadyBtnClicked = OnReadyFunction;

        StartCoroutine(StartGameCoroutine(PhotonNetwork.Time));
    }

    


    private void Update()
    {
        uiManagerInstance.SetPingTxt( PhotonNetwork.GetPing().ToString());
    }

    public void StartGame()
    {
        

    }

    public void OnReadyFunction()
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["isReady"] = true;

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    
    public void SetAllPlayersIndicatorStatus()
    {

        if(PhotonNetwork.IsMasterClient)
        {
            // Close Room when game has started
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        


        var playerList = spawnPlayersInstance.playersInRoom_List;

        Debug.Log($"player List = {playerList.Count}");

        foreach(var player in playerList)
        {
            Debug.Log($"pp =  {player.photonPlayer.NickName} ");

            if(player.isSeeker)
            {
                player.indicatorInstance.SetIndicator(true);
            }
            else
            {
                player.indicatorInstance.SetIndicator(false);
            }
        }

    }

    public bool CheckIfAllReady()
    {
        foreach(var player in spawnPlayersInstance.playersInRoom_List)
        {
            if(!player.isReady)
            {
                return false;
            }
        }

        return true;
    }

    public IEnumerator StartGameCoroutine(double photonTime)
    {
        Debug.Log("Starting Game...");
        gameState = GameState.SetUP;

        //Waiting for all to spawn ;

        yield return new WaitForSeconds(1);

        spawnPlayersInstance.SpawnPlayers();


        yield return new WaitForSeconds(4);

        uiManagerInstance.SetActiveLoadingPanel(false);

        yield return new WaitUntil(CheckIfAllReady);

        SetAllPlayersIndicatorStatus();

        
       
        
       // spawnPlayersInstance.SpawnAllPlayersInGameReadyLocations();

        yield return StartCoroutine(RoundStartCountDownTimer(photonTime));



        gameState = GameState.GameStart;
        isGameActive = true;

       
           StartCoroutine( SpawnPowerUps());
       


        // wait for game start timer

        // run time and check for time
        // catching mechanic
        gameTimerInstance.InitializeTimer(photonTime);
        yield return new WaitUntil(() => CheckIFEveryRunnerIsFrozen() || isTimeOver);

        gameTimerInstance.StopTimer();

        gameState = GameState.GameEnd;
        isGameActive = false;

        uiManagerInstance.SetActiveIceOverLay(false);

        if (areAllFrozen)
        {
            uiManagerInstance.SetActiveGameOverPanel(true);
            uiManagerInstance.SetGameOverText("Catcher Team Won The Game");

            /// Updating LEader Boards
            if (spawnPlayersInstance.localPlayerManager.isSeeker)
                playfabManager.UpdateLB_Seeker(1, true, 100);
            else
                playfabManager.UpdateLB_Runner(1, false, 100);
            
        }
        else
        {
            uiManagerInstance.SetActiveGameOverPanel(true);
            uiManagerInstance.SetGameOverText("Time Over! The runner team has won");

            /// Updating LEader Boards
            if (spawnPlayersInstance.localPlayerManager.isSeeker)
                playfabManager.UpdateLB_Seeker(1, false, 100);
            else
                playfabManager.UpdateLB_Runner(1, true, 100);
        }

        


        yield return null;

    }

    WaitForSeconds powerUpDelay = new WaitForSeconds(15);

    IEnumerator SpawnPowerUps()
    {
        Vector2 minPos = new Vector2(-9, -9);
        Vector2 maxPos = new Vector2(9, 9);


        int i = 1;
        while(gameState == GameState.GameStart)
        {
            

            yield return powerUpDelay;

            if (!PhotonNetwork.IsMasterClient)
                continue;

            var spawnPos = new Vector2(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y));
            PhotonNetwork.InstantiateRoomObject(invincibilityPowerUpPrefab.name, spawnPos, Quaternion.identity);
            
            if(i%2==0)
            {
                var s = new Vector2(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y));
                PhotonNetwork.InstantiateRoomObject(freezeGunPowerUpPrefab.name, s, Quaternion.identity);
            }
            
            i++;
        }
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


        //PhotonNetwork.LoadLevel(0);

        PhotonNetwork.LoadLevel(1);

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


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.IsMasterClient)
        {

            LeaveRoom();


        }
    }

    #endregion



}

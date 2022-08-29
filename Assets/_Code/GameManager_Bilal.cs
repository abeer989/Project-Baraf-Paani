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



    private void Start()
    {
        spawnPlayersInstance.SpawnPlayers();

        uiManagerInstance.SetStartBtnInteractibility(PhotonNetwork.IsMasterClient); // set interactibility if is master client

        uiManagerInstance.onStartClicked = StartGame;

    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }


    IEnumerator StartGameCoroutine()
    {
        Debug.Log("Starting Game...");

        if(PhotonNetwork.IsMasterClient)
        {
            DetermineSeeker();
        }

        yield return null;

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



    #endregion


    #region Game Start Functions

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

    #endregion







}

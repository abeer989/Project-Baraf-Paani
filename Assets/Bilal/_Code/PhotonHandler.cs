using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;




public class PhotonHandler : MonoBehaviourPunCallbacks
{
    public LobbyUIManager lobbyUImanagerInstance;
    public TitleUIManager titleUImanagerInstance;
    public LeaderBoardUIController leaderBoardUIInstance;

    public PlayFabManager playfabManager;

    public int levelToLoadIndex;

   


    private void Start()
    {
        titleUImanagerInstance.onStartClick += OnStartClickEvent;
        titleUImanagerInstance.onLeaderBoardClick += OnLeaderBoardClickEvent;


        lobbyUImanagerInstance.onJoinClick += JoinRoom;
        lobbyUImanagerInstance.onCreateClick += CreateRoom;
        lobbyUImanagerInstance.onBackClick += LobbyBackEvent;


        leaderBoardUIInstance.onBackClick += LeaderboardBackEvent;
        

        Debug.Log("ffad");
        if(Application.internetReachability != NetworkReachability.NotReachable  )
        {
            Debug.Log("ffad");
            if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
            {
                Debug.Log("ffad");
                lobbyUImanagerInstance.SetActiveLoadingPanel(false);
            }
            else
            {
                Debug.Log("ffad");
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.ConnectUsingSettings();
            }

           

        }
        else
        {
            // give ui error of no internet connectivity and open reconnect button
            Debug.Log(" Not Connected To internet");
            lobbyUImanagerInstance.SetActiveLoadingPanel(false);
        }    
    }

    public void OnStartClickEvent()
    {
        titleUImanagerInstance.FadeOutPanel();
        lobbyUImanagerInstance.FadeInPanel();
    }

    public void OnLeaderBoardClickEvent()
    {
        titleUImanagerInstance.FadeOutPanel();
        leaderBoardUIInstance.FadeInPanel();
    }

    public void LeaderboardBackEvent()
    {
        titleUImanagerInstance.FadeInPanel();
        leaderBoardUIInstance.FadeOutPanel();
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(lobbyUImanagerInstance.GetRoomName()))
        {
            lobbyUImanagerInstance.SetStaticText("Please Input Room Name To Create Or Join.");
            return;
        }

        if (!lobbyUImanagerInstance.ValidateName())
            return;

        if (string.IsNullOrEmpty(playfabManager.playerProfile.DisplayName))
            playfabManager.UpdateName(lobbyUImanagerInstance.GetPlayerName());

        PhotonNetwork.NickName = lobbyUImanagerInstance.GetPlayerName();
        PhotonNetwork.JoinRoom(lobbyUImanagerInstance.GetRoomName());

        Debug.Log("Joining Room");


    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(lobbyUImanagerInstance.GetRoomName()))
        {
            lobbyUImanagerInstance.SetStaticText("Please Input Room Name To Create Or Join.");
            return;
        }

        if (!lobbyUImanagerInstance.ValidateName())
            return;

        if (string.IsNullOrEmpty(playfabManager.playerProfile.DisplayName))
            playfabManager.UpdateName(lobbyUImanagerInstance.GetPlayerName());

        PhotonNetwork.NickName = lobbyUImanagerInstance.GetPlayerName();

        PhotonNetwork.CreateRoom(lobbyUImanagerInstance.GetRoomName(),
            new Photon.Realtime.RoomOptions() { MaxPlayers = 5, BroadcastPropsChangeToAll = true });
    }

    public void LobbyBackEvent()
    {
        lobbyUImanagerInstance.ResetLobbyForm();

        titleUImanagerInstance.FadeInPanel();
        lobbyUImanagerInstance.FadeOutPanel();
    }

    #region PHOTON


    public override void OnConnectedToMaster()
    {


        playfabManager.LoginCustomID();

        PhotonNetwork.JoinLobby();



    }

    public override void OnJoinedLobby()
    {
        lobbyUImanagerInstance.SetActiveLoadingPanel(false);
        titleUImanagerInstance.InitializeTitleTween();

        MusicManager.instance.PlayTitleMusic();

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Loading Level");
        PhotonNetwork.LoadLevel(levelToLoadIndex);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {

        Debug.Log($"On Join Error: -> {message} | {returnCode}");
        lobbyUImanagerInstance.SetStaticText(message);

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"On Create Error: -> {message} | {returnCode}");
        lobbyUImanagerInstance.SetStaticText(message);
    }
    #endregion




}

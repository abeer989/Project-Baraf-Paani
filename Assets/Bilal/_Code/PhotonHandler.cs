using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;




public class PhotonHandler : MonoBehaviourPunCallbacks
{
    public LobbyUIManager lobbyUImanagerInstance;

    public int levelToLoadIndex;

   


    private void Start()
    {
        lobbyUImanagerInstance.onJoinClick += JoinRoom;
        lobbyUImanagerInstance.onCreateClick += CreateRoom;


        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {
            Debug.Log(" Not Connected To internet");
        }    
    }

    public override void OnConnectedToMaster()
    {
        

        PhotonNetwork.JoinLobby();

        
    }

    public override void OnJoinedLobby()
    {
        lobbyUImanagerInstance.SetActiveLoadingPanel(false);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Loading Level");
        PhotonNetwork.LoadLevel(levelToLoadIndex);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {

        Debug.Log($"On Join Error: -> {message} | {returnCode}");
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"On Create Error: -> {message} | {returnCode}");
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(lobbyUImanagerInstance.GetRoomName()))
            return;

        if (!lobbyUImanagerInstance.ValidateName())
            return;

        PhotonNetwork.NickName = lobbyUImanagerInstance.GetPlayerName();
        PhotonNetwork.JoinRoom(lobbyUImanagerInstance.GetRoomName());

        Debug.Log("Joining Room");


    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(lobbyUImanagerInstance.GetRoomName()))
            return;

        if (!lobbyUImanagerInstance.ValidateName())
            return;

        PhotonNetwork.NickName = lobbyUImanagerInstance.GetPlayerName();

        PhotonNetwork.CreateRoom(lobbyUImanagerInstance.GetRoomName(),
            new Photon.Realtime.RoomOptions() { MaxPlayers = 5, BroadcastPropsChangeToAll = true});
    }
}

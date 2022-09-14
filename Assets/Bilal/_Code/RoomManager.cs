using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button playButton;



    public List<PlayerItemController> playerItemList;
    public PlayerItemController playerItemPrefab;

    public Transform playerItemParents;

    private void Start()
    {

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["isSeeker"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        UpdatePlayerList();



        playButton.onClick.AddListener(OnClickPlayEvent);
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient )
        {
            playButton.gameObject.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(false);
        }
    }

    public void DetermineSeeker()
    {


        var playersList = playerItemList;

        Debug.Log($"Players List Gotten -> {playersList.Count}");

        var seekerIndex = Random.Range(0, playersList.Count);

        Debug.Log($"Seeker Index -> {seekerIndex}");

        var playerItem = playersList[seekerIndex];

        Debug.Log($"playerManager Found -> {playerItem}");

        //Debug.Log($"Seeker Dteremine {playerItem.photonView.ViewID}");

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["isSeeker"] = true;

        playerItem.player.SetCustomProperties(hash);

    }

    public void OnClickPlayEvent()
    {
         DetermineSeeker();



       
        

        PhotonNetwork.LoadLevel(2); // open game scene
    }

    void UpdatePlayerList()
    {
        foreach(PlayerItemController item in playerItemList)
        {
            Destroy(item.gameObject);
        }

        playerItemList.Clear();

        if(PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach(KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItemController newPlayeritem =   Instantiate(playerItemPrefab, playerItemParents);
            newPlayeritem.SetPlayerInfo(player.Value);

            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayeritem.ApplyLocalChanges();
            }

            playerItemList.Add(newPlayeritem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        
    }

}

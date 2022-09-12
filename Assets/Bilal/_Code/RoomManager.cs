using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public List<PlayerItemController> playerItemList;
    public PlayerItemController playerItemPrefab;

    public Transform playerItemParents;

    private void Start()
    {
        UpdatePlayerList();
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

}

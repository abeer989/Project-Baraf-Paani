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

    public void OnClickPlayEvent()
    {
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

}

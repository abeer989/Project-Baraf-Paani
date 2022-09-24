using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button playButton;
    [SerializeField] Button leaveBtn;

    [SerializeField] TextMeshProUGUI roomNameTxt;
    [SerializeField] TextMeshProUGUI playerTxt;

    public List<PlayerItemController> playerItemList;
    public PlayerItemController playerItemPrefab;

    public Transform playerItemParents;

    GameObject localPlayerObject;

    [SerializeField] GameObject playerPrefab_1;
    [SerializeField] GameObject playerPrefab_2;
    private void Start()
    {

        SpawnPlayer();

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["isSeeker"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        UpdatePlayerList();

        roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
        SetPlayerCountTxt(PhotonNetwork.CurrentRoom.PlayerCount);

        playButton.onClick.AddListener(OnClickPlayEvent);
        leaveBtn.onClick.AddListener(LeaveRoom);
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

    public void SpawnPlayer()
    {
        if(localPlayerObject!=null)
        {
            PhotonNetwork.Destroy(localPlayerObject);
        }

        var hash = PhotonNetwork.LocalPlayer.CustomProperties;

        if(hash.ContainsKey("playerAvatar"))
        {
            if ((int)hash["playerAvatar"] == 0)
            {
                Vector2 pos = new Vector2(Random.Range(-5, 5), Random.Range(-3, 0.2f));

                localPlayerObject = PhotonNetwork.Instantiate(playerPrefab_1.name, pos, Quaternion.identity);
            }
            else if ((int)hash["playerAvatar"] == 1)
            {
                Vector2 pos = new Vector2(Random.Range(-5, 5), Random.Range(-3, 0.2f));

                localPlayerObject = PhotonNetwork.Instantiate(playerPrefab_2.name, pos, Quaternion.identity);
            }
        }
        else
        {
            Vector2 pos = new Vector2(Random.Range(-5, 5), Random.Range(-3, 0.2f));

            localPlayerObject = PhotonNetwork.Instantiate(playerPrefab_1.name, pos, Quaternion.identity);
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
        // DetermineSeeker();


        //PhotonNetwork.LoadLevel(2); // open game scene

        StartCoroutine(StartGameCo());
    }

    IEnumerator StartGameCo()
    {
        DetermineSeeker();

        yield return new WaitForSeconds(1.5f);

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


            newPlayeritem.onCharacterChanged = SpawnPlayer;

            playerItemList.Add(newPlayeritem);
        }
    }


    public void SetPlayerCountTxt(int count)
    {
        playerTxt.text = $"Player: {count}";
    }    

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetPlayerCountTxt(PhotonNetwork.CurrentRoom.PlayerCount);
        

        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetPlayerCountTxt(PhotonNetwork.CurrentRoom.PlayerCount);

        UpdatePlayerList();
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        
    }

    public void LeaveRoom()
    {

        PhotonNetwork.LeaveRoom();
        
    }


    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("Master Client Switched");
    }

}

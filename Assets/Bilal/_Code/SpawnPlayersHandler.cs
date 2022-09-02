using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class SpawnPlayersHandler : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public List<PlayerManager> playersInRoom_List;

    public void SpawnPlayers()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        var GO = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);

        var pm = GO.GetComponent<PlayerManager>();

        //playersInRoom_List.Add(pm);

    }



    public PlayerManager GetPlayerWithViewId(int viewId)
    {
        var playerObject = playersInRoom_List.FirstOrDefault(x => x.photonView.ViewID == viewId); ;

        return playerObject;
    }



    

}

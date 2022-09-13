using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class SpawnPlayersHandler : MonoBehaviour
{
    public GameObject[] playerPrefabs;


    [SerializeField]float rMinX;
    [SerializeField]float rMaxX;
    [SerializeField]float rMinY;
    [SerializeField]float rMaxY;



    [SerializeField] float sMinX;
    [SerializeField] float sMaxX;
    [SerializeField] float sMinY;
    [SerializeField] float sMaxY;

    public List<PlayerManager> playersInRoom_List;

    [SerializeField] List<Transform> runnerSpawnLocations_List;
    [SerializeField] List<Transform> seekersSpawnLocations_List;


    private HashSet<int> occupeidRunnerLocations;
    private HashSet<int> occupaiedSeekerLocations;
    


    public void SpawnPlayers()
    {
        bool isSeeker = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isSeeker"];

        Debug.Log($" isSeeker = {isSeeker}");

        Vector2 randomPosition = Vector2.zero;

        if (isSeeker)
        {
            randomPosition = new Vector2(Random.Range(rMinX, rMaxX), Random.Range(rMinY, rMaxY));

        }
        else
        {
            randomPosition = new Vector2(Random.Range(sMinX, sMaxX), Random.Range(sMinY, sMaxY));
        }

        int index = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];

        Debug.Log(" Player AVatar Int "+index);


        GameObject playerTypeToSpawn = playerPrefabs[index];

        var GO = PhotonNetwork.Instantiate(playerTypeToSpawn.name, randomPosition, Quaternion.identity);



        var pm = GO.GetComponent<PlayerManager>();
        pm.isSeeker = isSeeker;
        pm.LockPlayer();
        playersInRoom_List.Add(pm);

    }


    public void SpawnAllPlayersInGameReadyLocations()
    {
        int runnerIndex = 0;
        int seekerIndex = 0;

        foreach(var player in  playersInRoom_List)
        {
            player.LockPlayer();

            if(player.isSeeker)
            {
                if (seekerIndex >= seekersSpawnLocations_List.Count)
                    seekerIndex = 0;

                player.transform.position = seekersSpawnLocations_List[seekerIndex].position;
                seekerIndex++;
            }
            else
            {
                if (runnerIndex >= runnerSpawnLocations_List.Count)
                    runnerIndex = 0;

                player.transform.position = runnerSpawnLocations_List[runnerIndex].position;
                runnerIndex++;

            }
        }
    }


    public PlayerManager GetPlayerWithViewId(int viewId)
    {
        var playerObject = playersInRoom_List.FirstOrDefault(x => x.photonView.ViewID == viewId); ;

        return playerObject;
    }



    

}

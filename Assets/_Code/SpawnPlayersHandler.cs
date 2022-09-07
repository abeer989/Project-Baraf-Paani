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

    [SerializeField] List<Transform> runnerSpawnLocations_List;
    [SerializeField] List<Transform> seekersSpawnLocations_List;


    private HashSet<int> occupeidRunnerLocations;
    private HashSet<int> occupaiedSeekerLocations;
    


    public void SpawnPlayers()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));



        var GO = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);

        var pm = GO.GetComponent<PlayerManager>();

        //playersInRoom_List.Add(pm);

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

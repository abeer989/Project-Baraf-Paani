using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour, IPunInstantiateMagicCallback
{
    public PlayerMovement playerMovementInstance;  // handles movement - Dash- Pickup
    public PlayerIndicatorHandler indicatorInstance;
    

    [SerializeField] Animator animController;

    public Player photonPlayer;
    public PhotonView photonView;

    public bool isSeeker = false;

    private void Start()
    {
        if (!photonView)
            photonView = GetComponent<PhotonView>();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {


        GameManager_Bilal.instance.spawnPlayersInstance.playersInRoom_List.Add(this);
    }
}

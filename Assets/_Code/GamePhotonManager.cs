using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GamePhotonManager : MonoBehaviour
{
    [SerializeField] GameManager_Bilal gameManagerInstance;

    [PunRPC]
    public void RPC_SetSeeker(int viewID)
    {
        gameManagerInstance.SetSeeker(viewID);
    }

    [PunRPC]
    public void RPC_SetFrozen(int viewID)
    {
        gameManagerInstance.FreezeTargetPlayer(viewID);
    }
}

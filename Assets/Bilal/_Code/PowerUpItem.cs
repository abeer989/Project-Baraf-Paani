using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviourPun
{

    
    public enum PowerUpType {
        IceGun,
        TwoTimes
    }

    public PowerUpType type;

    [PunRPC]
    public void RPC_SetActivateObject(bool state)
    {
        gameObject.SetActive(state);
    }


    public void SetActiveMe(bool state)
    {
        base.photonView.RPC(nameof(RPC_SetActivateObject), RpcTarget.All, state);
    }

}

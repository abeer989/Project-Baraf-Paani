using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PickableItem : MonoBehaviour
{
    [SerializeField] Collider2D rangeCollider;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] PhotonView pv;



    public void OnPickUp(int playerViewId, Vector2 holdPosition)
    {
        pv.RPC(nameof(RPC_OnPickedUp), RpcTarget.All, playerViewId, holdPosition);
    }

    public void OnDrop(Vector2 dropDirection)
    {
        pv.RPC(nameof(RPC_OnDrop), RpcTarget.All, dropDirection);
    }

    [PunRPC]
    public void RPC_OnPickedUp(int playerViewId, Vector2 holdPosition)
    {
        PhotonView pv = PhotonView.Find(playerViewId);

        if(pv.gameObject != null)
        {
            gameObject.transform.parent = pv.gameObject.transform;
            gameObject.transform.position = holdPosition;

            rangeCollider.enabled = false;
            rb.simulated = false;
        }

        
    }


    [PunRPC]
    public void RPC_OnDrop(Vector2 dropDirection)
    {
        gameObject.transform.parent = null;
        gameObject.transform.position = dropDirection;

        rangeCollider.enabled = true;
        rb.simulated = true;
    }

    public int GetViewId()
    {
        return pv.ViewID;
    }

}

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

    [SerializeField] int throwSpeed;


    private Vector2 moveDir;


    private bool isThrown = false;

    //void Update() => rb.velocity = moveDir * throwSpeed;

    //private void FixedUpdate()
    //{
    //    if(isThrown)
    //    {
    //        rb.velocity = moveDir * throwSpeed;
    //    }
    //}

    private void Update()
    {
        if(isThrown)
        {
            rb.velocity = moveDir * throwSpeed;
            
        }
    }

    public void OnPickUp(int playerViewId, Vector2 holdPosition)
    {
        pv.RPC(nameof(RPC_OnPickedUp), RpcTarget.All, playerViewId, holdPosition);
    }

    public void OnDrop(Vector2 dropDirection)
    {
        pv.RPC(nameof(RPC_OnDrop), RpcTarget.All, dropDirection);
    }

    public void OnThrow(Vector2 throwDirection)
    {
        pv.RPC(nameof(RPC_OnThrow), RpcTarget.All, throwDirection);
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


    [PunRPC]
    public void RPC_OnThrow(Vector2 dropDirection)
    {

        Debug.Log($"Drop Direction = {dropDirection} | {throwSpeed}  = {dropDirection * throwSpeed} ");

        gameObject.transform.parent = null;
       // gameObject.transform.position = dropDirection;

        rangeCollider.enabled = true;
        rb.simulated = true;


        rb.AddForce(dropDirection * throwSpeed);
        //moveDir = dropDirection;
        rb.drag = 10;
        //isThrown = true;
        //rb.freezeRotation = false;
    }



    /*
      Debug.Log($"direction Stats = {direction.x},{direction.y} | {direction.magnitude} | {direction.sqrMagnitude}");


        if (direction.magnitude==0)
        {
            return;

        }

        Debug.Log("Instantiating");
        IceBall_multi iceBallCont = Instantiate(iceBallPrefab, firePoint.position, Quaternion.identity);


        iceBallCont.moveDir = direction;
        iceBallCont.senderId = viewId;
     */

    public int GetViewId()
    {
        return pv.ViewID;
    }

}

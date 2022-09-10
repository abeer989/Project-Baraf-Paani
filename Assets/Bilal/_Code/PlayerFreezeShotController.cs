using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PlayerFreezeShotController : MonoBehaviour
{
    [SerializeField] IceBall_multi iceBallPrefab;

    [SerializeField] Transform pivot;
    [SerializeField] Transform firePoint;


    public void FireIceBall(Vector2 direction,int viewId)
    {
        Debug.Log($"direction Stats = {direction.x},{direction.y} | {direction.magnitude} | {direction.sqrMagnitude}");


        if (direction.magnitude==0)
        {
            return;

        }

        Debug.Log("Instantiating");
        IceBall_multi iceBallCont = Instantiate(iceBallPrefab, firePoint.position, Quaternion.identity);


        iceBallCont.moveDir = direction;
        iceBallCont.senderId = viewId;
    }


    [PunRPC]
    public void RPC_FireIceBall(Vector2 dir,int viewId)
    {
        FireIceBall(dir, viewId);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PickUpHandler : MonoBehaviour
{
    public Vector3 Direction { get; set; }

    [SerializeField] Transform holdSpot;
    [SerializeField] LayerMask pickUpMask;

    [Space]
    [SerializeField] float throwSpeed;

    GameObject itemHolding;
    

    public bool isHoldingItem;


    [SerializeField] PhotonView pv;
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.E))
    //    {
    //        if(itemHolding)
    //        {
    //            DropAction();
    //        }
    //        else
    //        {
    //            PickUpAction();
    //        }
    //    }
    //}

    public GameObject GetHoldingItem()
    {
        return itemHolding;
    }


    public void PickAndThrowFunction()
    {
        if(itemHolding)
        {
            DropAction();
            //ThrowAction();
        }
        else
        {
            PickUpAction();
        }
    }    


    public void DropAction()
    {
        //itemHolding.transform.position = transform.position + Direction;
        //itemHolding.transform.parent = null;

        //if (itemHolding.GetComponent<PickableItem>())
        //    itemHolding.GetComponent<PickableItem>().OnDrop();

        if(!itemHolding)
        {
            return;
        }

        var p = itemHolding.GetComponent<PickableItem>();

        p.OnDrop(transform.position + Direction);


        itemHolding = null;
    }

    public void ThrowAction()
    {

        Debug.Log($"Throw Direction = {transform.position} | {Direction}");

        if (!itemHolding)
        {
            return;
        }

        var p = itemHolding.GetComponent<PickableItem>();

        p.OnThrow((Direction*2));


        itemHolding = null;
    }



    public void PickUpAction()
    {
        Collider2D pickupItemColl = Physics2D.OverlapCircle(transform.position + Direction, .4f, pickUpMask);

        if (pickupItemColl)
        {
            var item = pickupItemColl.GetComponentInParent<PickableItem>();

            item.OnPickUp(pv.ViewID, holdSpot.position);


            itemHolding = pickupItemColl.gameObject;

            //pv.RPC(nameof(item.OnPickedUp),RpcTarget.All,pv.ViewID,holdSpot.position);


            //itemHolding = pickupItemColl.gameObject;
            //itemHolding.transform.position = holdSpot.position;
            //itemHolding.transform.parent = transform;

            //if (itemHolding.GetComponent<PickableItem>())
            //    itemHolding.GetComponent<PickableItem>().OnPickedUp();
        }
    }

    

}

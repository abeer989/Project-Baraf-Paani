using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandler : MonoBehaviour
{
    public Transform holdSpot;
    public LayerMask pickUpMask;

    public Vector3 Direction { get; set; }
    private GameObject itemHolding;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(itemHolding)
            {
                itemHolding.transform.position = transform.position + Direction;
                itemHolding.transform.parent = null;

                if (itemHolding.GetComponent<Rigidbody2D>())
                    itemHolding.GetComponent<Rigidbody2D>().simulated = true;

                itemHolding = null;
            }
            else
            {
                Collider2D pickupItemColl = Physics2D.OverlapCircle(transform.position + Direction, .4f, pickUpMask);

                if(pickupItemColl)
                {
                    itemHolding = pickupItemColl.gameObject;
                    itemHolding.transform.position = holdSpot.position;
                    itemHolding.transform.parent = transform;

                    if (itemHolding.GetComponent<Rigidbody2D>())
                        itemHolding.GetComponent<Rigidbody2D>().simulated = false;
                }
            }
        }
    }

}

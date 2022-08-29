using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandler : MonoBehaviour
{
    public Vector3 Direction { get; set; }

    [SerializeField] Transform holdSpot;
    [SerializeField] LayerMask pickUpMask;

    [Space]
    [SerializeField] float throwSpeed;

    GameObject itemHolding;
    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(itemHolding)
            {
                itemHolding.transform.parent = null;

                if (itemHolding.GetComponent<Rigidbody2D>())
                {
                    Rigidbody2D rb = itemHolding.GetComponent<Rigidbody2D>();
                    rb.velocity = Direction * throwSpeed;
                }

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
                }
            }
        }
    }

}

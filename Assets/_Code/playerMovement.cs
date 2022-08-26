using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    Vector2 movement;

    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spriteRend;

    PickUpHandler pickupHandlerInstance;

    private void Start()
    {
        pickupHandlerInstance = GetComponent<PickUpHandler>();
        pickupHandlerInstance.Direction = new Vector2(0, 0);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(movement.sqrMagnitude> 0.1f)
        {
            pickupHandlerInstance.Direction = movement.normalized;
        }

        if (movement.x < 0)
            spriteRend.flipX = true;
        else if(movement.x > 0)
            spriteRend.flipX = false;

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    Vector2 movement;

    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spriteRend;

    PickUpHandler pickupHandlerInstance;

    [Header("Dash Controls")]
    [SerializeField] private float _dashingVelocity;
    [SerializeField] private float _dashingTime = 0;
    [SerializeField] KeyCode dashingKey;

    Vector2 _dashingDir;
    bool _isDashing;
    bool _canDash = true;

    [SerializeField] TrailRenderer trailRenderer;

    PhotonView photonViewInstance;


    private void Start()
    {
        pickupHandlerInstance = GetComponent<PickUpHandler>();
        pickupHandlerInstance.Direction = new Vector2(0, 0);

        photonViewInstance = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(photonViewInstance.IsMine)
            Movement();
    }

    private void Movement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Dash(movement.x, movement.y);







        if (movement.sqrMagnitude > 0.1f)
        {
            pickupHandlerInstance.Direction = movement.normalized;
        }

        if (movement.x < 0)
            spriteRend.flipX = true;
        else if (movement.x > 0)
            spriteRend.flipX = false;

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if(!_isDashing)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if (_isDashing)
        {
            rb.velocity = _dashingDir.normalized * _dashingVelocity;
            return;
        }
    }

    private void Dash(float inputX, float inputY)
    {
        if (Input.GetKeyDown(dashingKey) && _canDash)
        {
            _isDashing = true;
            _canDash = false;
            trailRenderer.emitting = true;
            _dashingDir = new Vector2(inputX, inputY);

            if (_dashingDir == Vector2.zero)
            {
                _dashingDir = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashingCoroutine());
        }
    }

    private IEnumerator StopDashingCoroutine()
    {
        yield return new WaitForSeconds(_dashingTime);
        trailRenderer.emitting = false;
        _isDashing = false;
        _canDash = true;
    }
}

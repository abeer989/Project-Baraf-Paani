using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Movement Related")]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    Vector2 movement;


    [Header("Dash Controls")]
    [SerializeField] private float _dashingVelocity;
    [SerializeField] private float _dashingTime = 0;
    [SerializeField] KeyCode dashingKey;

    Vector2 _dashingDir;
    bool _isDashing;
    bool _canDash = true;

    [SerializeField] TrailRenderer trailRenderer;




    public Vector2 Movement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Dash(movement.x, movement.y);


        return movement;
  
    }

    #region Dash Functions

    public void MovementFixedUpdateFunction()
    {
        if (!_isDashing)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if (_isDashing)
        {
            rb.velocity = _dashingDir.normalized * _dashingVelocity;
            return;
        }
    }

    public void Dash(float inputX, float inputY)
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
    #endregion





}

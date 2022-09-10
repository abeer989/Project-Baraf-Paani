using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviour,IPunObservable
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

    private Vector2 networkPosition;
    private float networkRotation;

    private PhotonView pv;

    public KeyCode DashingKey { get => dashingKey;  }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public Vector2 Movement(bool isDashkeyPressed)
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(isDashkeyPressed)
            Dash(movement.x, movement.y);


        return movement;
  
    }

    #region Dash Functions

    public void MovementFixedUpdateFunction()
    {
        if (!pv.IsMine)
        {
            rb.position = Vector3.MoveTowards(GetComponent<Rigidbody>().position, networkPosition, Time.fixedDeltaTime);

        }

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
        if ( _canDash)
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

    public void StopMovement()
    {
        movement = Vector2.zero;
    }
    private IEnumerator StopDashingCoroutine()
    {
        yield return new WaitForSeconds(_dashingTime);
        trailRenderer.emitting = false;
        _isDashing = false;
        _canDash = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(rb.position);

        //    stream.SendNext(rb.velocity);
        //}
        //else
        //{
        //    networkPosition = (Vector2)stream.ReceiveNext();

        //    rb.velocity = (Vector2)stream.ReceiveNext();

        //    float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
        //    networkPosition += rb.velocity * lag;
        //}
        //// throw new System.NotImplementedException();
       
    }
    #endregion





}

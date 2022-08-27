using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHandler : MonoBehaviour
{
    [SerializeField] private float _dashingVelocity;
    [SerializeField] private float _dashingTime = 0;
    [SerializeField] KeyCode dashingKey; 

    Vector2 _dashingDir;
    bool _isDashing;
    bool _canDash;

    TrailRenderer trailRenderer;


    public void Dash(Rigidbody2D _rigidbody,bool dashInput, float inputX, float inputY)
    {
        if(dashInput && _canDash)
        {
            _isDashing = true;
            _canDash = false;
            trailRenderer.emitting = true;
            _dashingDir = new Vector2(inputX, inputY);

            if(_dashingDir == Vector2.zero)
            {
                _dashingDir = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashingCoroutine());
        }

        if(_isDashing)
        {
            _rigidbody.velocity = _dashingDir.normalized * _dashingVelocity;
            return;
        }


    }

    private IEnumerator StopDashingCoroutine()
    {
        yield return new WaitForSeconds(_dashingTime);
        trailRenderer.emitting = false;
        _isDashing = false;
    }

}

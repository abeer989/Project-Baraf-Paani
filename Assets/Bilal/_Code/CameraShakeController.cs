using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void ShakeCamera()
    {
        anim.SetTrigger("shake");
    }
}

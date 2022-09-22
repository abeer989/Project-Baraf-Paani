using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleRotateTween : MonoBehaviour
{
    [SerializeField] Transform targetTrasnform;

    [SerializeField] float rotatingDuration;

    Tween rotationTween;

    [SerializeField] Vector3 rotatevalue;

    private void Start()
    {
        targetTrasnform = gameObject.transform;
        rotationTween = targetTrasnform.DORotate(rotatevalue, rotatingDuration, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void OnEnable()
    {
        rotationTween = targetTrasnform.DORotate(rotatevalue, rotatingDuration, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        
    }

    private void OnDisable()
    {
        if(rotationTween != null)
        {
            rotationTween.Kill();
        }
    }

    private void OnDestroy()
    {
        if (rotationTween != null)
        {
            rotationTween.Kill();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicatorHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer indicatorSprite;

    [SerializeField] Color seekerColor;
    [SerializeField] Color runnerColor;

    [SerializeField] Color defaultColor;

    public void SetIndicator(bool isSeeker)
    {
        if(isSeeker)
        {
            indicatorSprite.color = seekerColor;
        }
        else
        {
            indicatorSprite.color = runnerColor;
        }
    }

    public void ResetIndicator()
    {
        indicatorSprite.color = defaultColor;
    }

}

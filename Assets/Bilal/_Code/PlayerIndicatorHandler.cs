using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicatorHandler : MonoBehaviour
{

    [Header("Player seeker/Runner Indicator")]

    [SerializeField] SpriteRenderer indicatorSprite;

    [SerializeField] Color seekerColor;
    [SerializeField] Color runnerColor;

    [SerializeField] Color defaultColor;


    [Header("Player Local Indicator")]
    [SerializeField] GameObject localIndicator;

    [Header("Player Action Indicator")]
    [SerializeField] GameObject actionIndicator;


    [Header("Player IceBruf Indicator")]
    [SerializeField] GameObject brufIndicator;


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



    public void SetActiveLocalIndicator(bool state)
    {
        localIndicator.SetActive(state);
    }


    public void SetACtiveActionIndicator(bool state)
    {
        actionIndicator.SetActive(state);
    }


    public void SetActiveBrufIndicator(bool state)
    {
        brufIndicator.SetActive(state);
    }
}

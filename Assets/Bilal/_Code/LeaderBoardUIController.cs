using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class LeaderBoardUIController : MonoBehaviour
{
    [SerializeField] CanvasGroup leaderBoardCanvasGroup;

    [SerializeField] Button backBtn;


    public Action onBackClick;

    [Header("TweenControls")]
    [SerializeField] float fadeDuration;


    private void Start()
    {
        backBtn.onClick.AddListener(delegate { onBackClick(); });
    }

    public void FadeOutPanel()
    {
        leaderBoardCanvasGroup.DOFade(0, fadeDuration);
        leaderBoardCanvasGroup.interactable = false;
        leaderBoardCanvasGroup.blocksRaycasts = false;
    }

    public void FadeInPanel()
    {
        leaderBoardCanvasGroup.DOFade(1, fadeDuration);
        leaderBoardCanvasGroup.interactable = true;
        leaderBoardCanvasGroup.blocksRaycasts = true;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
public class TitleUIManager : MonoBehaviour
{


    [SerializeField] CanvasGroup uiPanelcanvasGroup;
    

    [SerializeField] Button startBtn;
    [SerializeField] Button leaderBoardBtn;
    //[SerializeField] Button settingBtn;

    public Action onStartClick;
    public Action onLeaderBoardClick;

    

    [SerializeField] RectTransform titleRectTransform;

    [SerializeField] RectTransform titleBtnOptionsTransform;
    public Vector2 targetPos;
    public Vector2 defaultPos;

    public float titlePopDuration;
    public float optionPopDuration;

    public Ease titlePopEase;
    public Ease optionPopEase;

    public float fadeDuration;

    Sequence titleSequence;

    private void Start()
    {
        startBtn.onClick.AddListener(delegate { onStartClick(); });
        leaderBoardBtn.onClick.AddListener(delegate { onLeaderBoardClick(); });
    }

    public void InitializeTitleTween()
    {
        titleSequence = DOTween.Sequence();

        titleSequence.Append(titleRectTransform.DOScale(Vector2.one, titlePopDuration).SetEase(titlePopEase));
        titleSequence.Append(titleBtnOptionsTransform.DOAnchorPosY(targetPos.y, optionPopDuration).SetEase(optionPopEase));
        titleSequence.Append(titleRectTransform.DOScale(new Vector3(1.2f, 1.2f, 1), 3).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutCubic));

        titleSequence.Play();
    }

    

    public void FadeOutPanel()
    {
        uiPanelcanvasGroup.DOFade(0, fadeDuration);
        uiPanelcanvasGroup.interactable = false;
      //  uiPanelcanvasGroup.blocksRaycasts = false;

    }

    public void FadeInPanel()
    {
        uiPanelcanvasGroup.DOFade(1, fadeDuration);
        uiPanelcanvasGroup.interactable = true;
       // uiPanelcanvasGroup.blocksRaycasts = true;
    }

    public void OnDisable()
    {
        if(titleSequence !=null)
        {
            titleSequence.Kill();
        }

        
    }


}

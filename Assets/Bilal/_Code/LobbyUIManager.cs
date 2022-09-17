using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] GameObject loadingPnlGO;

    [SerializeField] CanvasGroup lobbyCanvasGroup;

    [SerializeField] TMP_InputField roomNameIF;
    [SerializeField] TMP_InputField playerNameIF;
    
    [SerializeField] Button createBtn;
    [SerializeField] Button joinBtn;
    [SerializeField] Button backBtn;

    [SerializeField] Image iceIcon;

    [SerializeField] TextMeshProUGUI statusTxt;

    public Action onJoinClick;
    public Action onCreateClick;
    public Action onBackClick;

    private Tween iceRotateTween;

    [Header("TweenControls")]
    [SerializeField] float rotateDuration;
    [SerializeField] float fadeDuration;
     

    private void Start()
    {
        // method Subscription
        createBtn.onClick.AddListener(delegate { onCreateClick(); });
        joinBtn.onClick.AddListener(delegate { onJoinClick(); });
        backBtn.onClick.AddListener(delegate { onBackClick(); });

        iceRotateTween = iceIcon.transform.DORotate(Vector3.zero, rotateDuration, RotateMode.FastBeyond360);

        

    }

    public void FadeOutPanel()
    {
        lobbyCanvasGroup.DOFade(0, fadeDuration);
        lobbyCanvasGroup.interactable = false;
        lobbyCanvasGroup.blocksRaycasts = false;
    }

    public void FadeInPanel()
    {
        lobbyCanvasGroup.DOFade(1, fadeDuration);
        lobbyCanvasGroup.interactable = true;
        lobbyCanvasGroup.blocksRaycasts = true;
    }


    public string GetRoomName()
    {
        return roomNameIF.text;
    }

    public string GetPlayerName()
    {
        return playerNameIF.text;
    }

    public void SetActiveLoadingPanel(bool state)
    {
        loadingPnlGO.SetActive(state);
    }

    public void SetStaticText(string text)
    {
        statusTxt.text = text;
    }


    public void ResetLobbyForm()
    {
        roomNameIF.text = "";
        playerNameIF.text = "";

        statusTxt.text = "";
    }

    public bool ValidateName()
    {
        if(string.IsNullOrEmpty( playerNameIF.text))
        {
            statusTxt.text = "Please Input Player Name.";

            return false;
        }

        return true;
    }

    private void OnEnable()
    {
        iceRotateTween = iceIcon.transform.DORotate(new Vector3(0,0,361), rotateDuration, RotateMode.FastBeyond360).SetLoops(-1,LoopType.Restart);

    }

    private void OnDisable()
    {
        if (iceRotateTween != null)
            iceRotateTween.Kill();
    }

}

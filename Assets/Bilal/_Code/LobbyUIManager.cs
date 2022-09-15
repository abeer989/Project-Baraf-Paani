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


    [SerializeField] TMP_InputField roomNameIF;
    [SerializeField] TMP_InputField playerNameIF;
    [SerializeField] Button createBtn;
    [SerializeField] Button joinBtn;
    [SerializeField] Image iceIcon;

    public Action onJoinClick;
    public Action onCreateClick;

    private Tween iceRotateTween;

    [Header("TweenControls")]
    [SerializeField] float duration;
     

    private void Start()
    {
        // method Subscription
        createBtn.onClick.AddListener(delegate { onCreateClick(); });
        joinBtn.onClick.AddListener(delegate { onJoinClick(); });

        iceRotateTween = iceIcon.transform.DORotate(Vector3.zero, duration, RotateMode.FastBeyond360);

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

    public bool ValidateName()
    {
        if(string.IsNullOrEmpty( playerNameIF.text))
        {
            return false;
        }

        return true;
    }

    private void OnEnable()
    {
        iceRotateTween = iceIcon.transform.DORotate(new Vector3(0,0,361), duration, RotateMode.FastBeyond360).SetLoops(-1,LoopType.Restart);

    }

    private void OnDisable()
    {
        if (iceRotateTween != null)
            iceRotateTween.Kill();
    }

}

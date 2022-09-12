using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] GameObject loadingPnlGO;


    [SerializeField] TMP_InputField roomNameIF;
    [SerializeField] TMP_InputField playerNameIF;
    [SerializeField] Button createBtn;
    [SerializeField] Button joinBtn;

    public Action onJoinClick;
    public Action onCreateClick;


    private void Start()
    {
        // method Subscription
        createBtn.onClick.AddListener(delegate { onCreateClick(); });
        joinBtn.onClick.AddListener(delegate { onJoinClick(); });

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
    
}

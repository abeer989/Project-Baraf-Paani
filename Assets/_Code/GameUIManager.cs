using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerCountText;

    [SerializeField] Button startGameBtn;

    [SerializeField] Button leaveBtn;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI WinText;

    [SerializeField] Button playAgainBtn;


    public System.Action onStartClicked;
    public System.Action onleaveBtnClicked;
    public System.Action onPlayAgainClicked;

    [Header("Timer UI")]
    [SerializeField] TextMeshProUGUI timerTxt;

    
    private void Start()
    {
        startGameBtn.onClick.AddListener(delegate { onStartClicked?.Invoke(); });
        playAgainBtn.onClick.AddListener(delegate { onPlayAgainClicked?.Invoke(); });
    }

    public void SetTimerText(int value)
    {
        timerTxt.text = $"Time Left: {value}";
    }

    public void ResetTimerTxt()
    {
        timerTxt.text = $"Time Left: -";
    }

    public void SetListOfPlayerText(string txt)
    {
        playerCountText.text = txt;
    }

    public void SetStartBtnInteractibility(bool state)
    {
        startGameBtn.interactable = state;
    }

    public void SetActiveGameOverPanel(bool state)
    {
        gameOverPanel.SetActive(state);
    }

    public void SetGameOverText(string txt)
    {
        WinText.text = txt;
    }

}

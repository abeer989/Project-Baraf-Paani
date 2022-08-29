using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerCountText;

    [SerializeField] Button startGameBtn;

    public System.Action onStartClicked;


    
    private void Start()
    {
        startGameBtn.onClick.AddListener(delegate { onStartClicked(); });
    }

    public void SetListOfPlayerText(string txt)
    {
        playerCountText.text = txt;
    }

    public void SetStartBtnInteractibility(bool state)
    {
        startGameBtn.interactable = state;
    }

}

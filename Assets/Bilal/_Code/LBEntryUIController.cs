using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LBEntryUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI posTxt;
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI scoreTxt;

    public void SetUpEntry(int pos, string name, int score,bool isMine = false)
    {
        posTxt.text = $"{pos}";
        nameTxt.text = $"{name}";
        scoreTxt.text = $"{score}";

        if(isMine)
        {
            posTxt.color = Color.blue;
            nameTxt.color = Color.blue;
            scoreTxt.color = Color.blue;

        }
    }
}

using TMPro;
using UnityEngine;

public class LeaderBoardEntry : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI serialNo;
    [SerializeField] TextMeshProUGUI playfabID;
    [SerializeField] TextMeshProUGUI score;

    public void Setup(string sNo, string _playfabID, string _score)
    {
        serialNo.SetText(sNo);
        playfabID.SetText(_playfabID);
        score.SetText(_score);
    }
}

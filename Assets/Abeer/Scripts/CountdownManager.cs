using TMPro;
using UnityEngine;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] float defaultTimeValue;

    float timer;

    void Start() => StartCoroutine(Timer());

    private IEnumerator Timer()
    {
        timer = defaultTimeValue;

        do
        {
            timer -= Time.deltaTime;
            FormatText();
            yield return null;
        } while (timer > 0);

        Debug.LogError("GAME OVER!"); // GAME OVER LOGIC
        Time.timeScale = 0;
    }

    private void FormatText()
    {
        int hours = (int)(timer / 3600) % 24;
        int minutes = (int)(timer / 60) % 60;
        int seconds = (int)(timer % 60);

        if (hours > 0 || minutes > 0 || seconds > 0)
        {
            string hourString = hours.ToString();
            string minString = minutes.ToString();
            string secString = seconds.ToString();

            if (hours < 10)
                hourString = "0" + hourString;            
            
            if (minutes < 10)
                minString = "0" + minString;            
            
            if (seconds < 10)
                secString = "0" + secString;

            timeText.SetText(hourString + ":" + minString + ":" + secString + "");
        }

        else
            timeText.SetText("00:00:00");
    }
}

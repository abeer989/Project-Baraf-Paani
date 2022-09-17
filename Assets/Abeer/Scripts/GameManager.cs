using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] TMP_Text scoreText;

    [Header("Countdown Timer")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject gameWonScreen;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] float defaultTimeValue;

    [Space]
    [SerializeField] string gameplaySceneName;

    int score;
    float timer;
    int noOfRunners;

    // public properties:
    public int Score
    {
        get { return score; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        DontDestroyOnLoad(this);

        Time.timeScale = 1;
    }

    void Start()
    {
        noOfRunners = FindObjectsOfType<NPCController>().Length;
        Debug.Log((noOfRunners / 2) + 1);
        StartCoroutine(Timer());
    }

    // public functions:
    public void UpdateScore(int fac)
    {
        score += fac;

        if (score <= 0)
            score = 0;

        scoreText.SetText($"RUNNERS CAUGHT: {score}");
    }

    // private functions:
    private IEnumerator Timer()
    {
        timer = defaultTimeValue;

        do
        {
            timer -= Time.deltaTime;
            FormatText();
            yield return null;
        } while (timer > 0);

        Time.timeScale = 0;

        if (score < ((noOfRunners / 2) + 1))
        {
            gameOverScreen.SetActive(true);
            PlayfabLeaderboardManager.instance.UpdateLeaderBoard(0);
        }

        else
        {
            gameWonScreen.SetActive(true);
            PlayfabLeaderboardManager.instance.UpdateLeaderBoard(1);
        }
    }

    public void Replay() => SceneManager.LoadScene(gameplaySceneName);

    public void Quit() => Application.Quit();

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

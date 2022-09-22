using TMPro;
using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class PlayfabLoginManager : MonoBehaviour
{
    public static PlayfabLoginManager instance;

    [Header("Username")]
    [SerializeField] GameObject namePanel;
    [SerializeField] TMP_InputField nameIPF;

    [SerializeField] GameObject questionPrompt;

    [SerializeField] string sceneToLoad;

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
    }

    public void Login()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            //CustomId = SystemInfo.deviceUniqueIdentifier,
            CustomId = "abc" + Random.Range(1, 200).ToString(),
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    public void OnSubmitNameButtonPressed()
    {
        var rqt = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameIPF.text,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(rqt, OnNameUpdated, OnError);
    }

    public void LoadGameplay() => SceneManager.LoadScene(sceneToLoad);

    #region Callbacks
    private void OnLoginSuccess(LoginResult result)
    {
        string name = null;


        if (result.InfoResultPayload.PlayerProfile != null) 
        {
            if (!string.IsNullOrEmpty(result.InfoResultPayload.PlayerProfile.DisplayName.Trim()))
                name = result.InfoResultPayload.PlayerProfile.DisplayName.Trim(); 
        }

        if (name == null)
        {
            Debug.Log("hehe");
            questionPrompt.SetActive(true);
        }

        else
            LoadGameplay();
    }

    private void OnNameUpdated(UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log("Username Updated");
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnError(PlayFabError obj) => Debug.Log(obj.GenerateErrorReport());
    #endregion
}

using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private Button _login;
    [SerializeField] private Text _output;

    private void Start()
    {
        _login.onClick.AddListener(Login);
    }

    private void OnDestroy()
    {
        _login.onClick.RemoveAllListeners();
    }

    private void Login()
    {
        _login.interactable = false;
        // Here we need to check whether TitleId property is configured in settings or not
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
         	* If not we need to assign it to the appropriate variable manually
         	* Otherwise we can just remove this if statement at all
        	*/
            PlayFabSettings.staticSettings.TitleId = "BA3B2";
        }
        var request = new LoginWithCustomIDRequest { CustomId = "GeekBrainsLesson3-1", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        _output.text = "Success!";
        _output.color = Color.green;
        _login.interactable = true;
    }

    private void OnLoginFailure(PlayFabError error)
    {
        _output.text = "Failure!";
        _output.color = Color.red;
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
        _login.interactable = true;
    }
}

using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    private const string AuthKey = "auth";

    [SerializeField] private Button _loginButton;
    [SerializeField] private Text _outputLabel;

    private void Start()
    {
        _loginButton.onClick.AddListener(Login);
    }

    private void OnDestroy()
    {
        _loginButton.onClick.RemoveAllListeners();
    }

    private void Login()
    {
        _loginButton.interactable = false;

        bool isCreateAccount = PlayerPrefs.HasKey(AuthKey);
        string id = PlayerPrefs.GetString(AuthKey, Guid.NewGuid().ToString());

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = id,
            CreateAccount = !isCreateAccount,
        },
        (success) => OnLoginSuccess(id),
        OnLoginFailure);
    }

    private void OnLoginSuccess(string id)
    {
        PlayerPrefs.SetString(AuthKey, id);
        _outputLabel.text = "Success!";
        _outputLabel.color = Color.green;
        _loginButton.interactable = true;
        Debug.Log($"Complete enter! Guid: {id}");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        _outputLabel.text = "Failure!";
        _outputLabel.color = Color.red;
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
        _loginButton.interactable = true;
    }
}

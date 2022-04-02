using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;

public class PlayfabClient
{
    private const string AuthKey = "auth";
    private static string _myPlayfabId;

    public static void Login(Action<LoginResult> successCallBack)
    {
        bool isCreateAccount = PlayerPrefs.HasKey(AuthKey);
        string id = PlayerPrefs.GetString(AuthKey, Guid.NewGuid().ToString());

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = id,
            CreateAccount = !isCreateAccount,
        },
        (success) =>
        {
            successCallBack(success);
            _myPlayfabId = success.PlayFabId;
        },
        OnLoginFailure);
    }

    public static void GetUserData(string name, string defaultValue, Action<string> callback)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = _myPlayfabId,
            Keys = null
        }, 
        result => {
            if (result.Data == null || !result.Data.ContainsKey(name))
                callback(defaultValue);
            else callback(result.Data[name].Value);
        },
        OnLoginFailure
        );
    }

    private static void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }
}


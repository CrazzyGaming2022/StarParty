using PlayFab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new PlayFab.ClientModels.LoginWithPlayFabRequest()
        {
            Username = _username,
            Password = _password
        },
        request =>
        {
            Debug.Log($"Username complete sign in: {_username}");
            EnterInGameScene();
        },
        error => Debug.Log($"Error: {error.ErrorMessage}"));
    }
}


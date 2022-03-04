using PlayFab;
using UnityEngine;
using UnityEngine.UI;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private LoadingView _loadingView;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        _loadingView.Show();
        PlayFabClientAPI.LoginWithPlayFab(new PlayFab.ClientModels.LoginWithPlayFabRequest()
        {
            Username = _username,
            Password = _password
        },
        request =>
        {
            Debug.Log($"Username complete sign in: {_username}");
            EnterInGameScene();
            _loadingView.Hide();
        },
        error =>
        {
            Debug.Log($"Error: {error.ErrorMessage}");
            _loadingView.Hide();
        });
    }
}


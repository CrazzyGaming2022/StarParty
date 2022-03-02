using PlayFab;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] private InputField _emailField;
    [SerializeField] private Button _createAccountButton;

    private string _email;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();
        _emailField.onValueChanged.AddListener(UpdateEmail);
        _createAccountButton.onClick.AddListener(CreateAccount);
    }

    private void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new PlayFab.ClientModels.RegisterPlayFabUserRequest()
        {
            Username = _username,
            Password = _password,
            Email = _email
        },
        request =>
        {
            Debug.Log($"Username register: {request.Username}");
            EnterInGameScene();
        },
        error => Debug.Log($"Error {error.ErrorMessage}"));
    }

    private void UpdateEmail(string email)
    {
        _email = email;
    }
}


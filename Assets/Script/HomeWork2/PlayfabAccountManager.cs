using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] private Text _titleLabel;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
        OnGetAccount,
        OnError);
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.Log($"Error {errorMessage}");
    }

    private void OnGetAccount(GetAccountInfoResult account)
    {
        _titleLabel.text = $"Username: {account.AccountInfo.Username}, ID: {account.AccountInfo.PlayFabId}";
    }
}

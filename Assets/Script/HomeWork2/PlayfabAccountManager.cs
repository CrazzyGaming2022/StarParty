using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] private Text _titleLabel;
    [SerializeField] private Button _deletePlayerButton;
    [SerializeField] private CatalogView _catalogView;

    private string _playfabId;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
        OnGetAccount,
        OnError);
        _deletePlayerButton.onClick.AddListener(DeletePlayer);
        _deletePlayerButton.interactable = false;
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(),
            OnGetCatalogSuccess,
            OnError);
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        InfoCatalog(result.Catalog);
        Debug.Log("Catalog download!");
    }

    private void InfoCatalog(List<CatalogItem> catalog)
    {
        _catalogView.DisplayCatalog(catalog);
    }

    private void DeletePlayer()
    {
        PlayFabAdminAPI.DeletePlayer(new PlayFab.AdminModels.DeletePlayerRequest()
        {
            PlayFabId = _playfabId
        },
        result => Debug.Log("Player deleted"),
        error => Debug.Log("Delete error"));
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.Log($"Error {errorMessage}");
    }

    private void OnGetAccount(GetAccountInfoResult account)
    {
        _titleLabel.text = $"Username: {account.AccountInfo.Username}"
            + $"\nID: {account.AccountInfo.PlayFabId}"
            + $"\nCreated: {account.AccountInfo.Created}"
            + $"\nEmail: {account.AccountInfo.PrivateInfo.Email}"
            + $"\nDisplay name: {account.AccountInfo.TitleInfo.DisplayName}"
            + $"\nLast login: {account.AccountInfo.TitleInfo.LastLogin}"
            + $"\nFirst login: {account.AccountInfo.TitleInfo.FirstLogin}";
        _playfabId = account.AccountInfo.PlayFabId;
        _deletePlayerButton.interactable = true;
    }
}

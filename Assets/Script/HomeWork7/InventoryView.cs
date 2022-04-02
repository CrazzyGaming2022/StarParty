using Photon.Pun.Demo.PunBasics;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    private const string MEDPACK_ID = "medpack_id";
    private const string CREDITS = "CR";

    private string _medpackInstanceId;

    [SerializeField] private Text _moneyLabel;
    [SerializeField] private Text _medpackLabel;

    private void Start()
    {
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                    GetInventorySuccess,
                    OnFailure);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = MEDPACK_ID,
                VirtualCurrency = CREDITS,
                Price = 100
            },
            result => UpdateInventory(),
            OnFailure);

        if (Input.GetKeyDown(KeyCode.H))
            PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest()
            {
                ConsumeCount = 1,
                ItemInstanceId = _medpackInstanceId
            },
            OnConsumeMedpack,
            OnFailure);

    }

    private void OnConsumeMedpack(ConsumeItemResult result)
    {
        UpdateInventory();
        PlayerManager.LocalPlayerInstance.GetComponent<PlayerManager>().Health = 1.0f;
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void GetInventorySuccess(GetUserInventoryResult result)
    {
        var medpack = result.Inventory.FirstOrDefault(x => x.ItemId == MEDPACK_ID);
        if (medpack != null)
        {
            _medpackLabel.text = medpack.RemainingUses.ToString();
            _medpackInstanceId = medpack.ItemInstanceId;
        }
        else
        {
            _medpackLabel.text = "0";
        }

        var credits = result.VirtualCurrency.First(x => x.Key == CREDITS);
        _moneyLabel.text = credits.Value.ToString();
    }
}


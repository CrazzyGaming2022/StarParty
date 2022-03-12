using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatalogView : MonoBehaviour
{
    [SerializeField] private ItemView _itemPrefab;
    [SerializeField] private Transform _content;

    public void DisplayCatalog(IEnumerable<CatalogItem> items)
    {
        foreach (var item in items)
        {
            var itemView = Instantiate(_itemPrefab, _content);
            var priceItem = item.VirtualCurrencyPrices.First();
            string price = $"{priceItem.Value} {priceItem.Key}";
            itemView.SetData(item.DisplayName, item.ItemClass, price);
        }
    }
}


using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] private Text _displayNameLablel;
    [SerializeField] private Text _classLabel;
    [SerializeField] private Text _priceLabel;

    public void SetData(string displayName, string @class, string price)
    {
        _displayNameLablel.text = displayName;
        _classLabel.text = @class;
        _priceLabel.text = price;
    }
}

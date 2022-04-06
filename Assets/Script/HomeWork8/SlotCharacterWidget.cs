using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotCharacterWidget : MonoBehaviour
{
    [SerializeField] private Button _slotButton;
    [SerializeField] private Button _fightButton;
    [SerializeField] private GameObject _emptySlot;
    [SerializeField] private GameObject _infoCharacterSlot;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _levelLabel;
    [SerializeField] private TMP_Text _goldLabel;
    [SerializeField] private TMP_Text _damageLabel;
    [SerializeField] private TMP_Text _healthLabel;
    [SerializeField] private TMP_Text _expirienceLabel;

    public Button SlotButton => _slotButton;
    public Button FightButton => _fightButton;

    public string CharacterId { get; private set; }

    public void ShowEmptySlot()
    {
        _emptySlot.SetActive(true);
        _infoCharacterSlot.SetActive(false);
    }

    public void ShowCharacerInfoSlot(string characterId, string name, int level, int gold, int damage, int health, int expirience)
    {
        CharacterId = characterId;
        _nameLabel.SetText(name);
        _levelLabel.SetText($"Level: {level}");
        _goldLabel.SetText($"Gold: {gold}");
        _damageLabel.SetText($"Damage: {damage}");
        _healthLabel.SetText($"Health: {health}");
        _expirienceLabel.SetText($"Expirience: {expirience}");

        _emptySlot.SetActive(false);
        _infoCharacterSlot.SetActive(true);
    }
}

using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] private Text _titleLabel;
    [SerializeField] private GameObject _newCharacterPanel;
    [SerializeField] private Button _creareCharacterButton;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private List<SlotCharacterWidget> _slots;

    private string _characterName;
    private int _index;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        GetCharacters();

        foreach (var slot in _slots)
        {
            slot.SlotButton.onClick.AddListener(ShowCreateCharacterPanel);
            slot.FightButton.onClick.AddListener(() => Fight(slot.CharacterId));
        }

        _creareCharacterButton.onClick.AddListener(CreateCharacterWithItem);
        _inputField.onValueChanged.AddListener(OnNamedChanged);
    }

    private void Fight(string characterId)
    {
        int gotExpirience = 0;
        if (Random.value > 0.5f)
        {
            gotExpirience = 5;
            Debug.Log("Win");
        }
        else
        {
            gotExpirience = 1;
            Debug.Log("Lose");
        }
        Debug.Log($"Got {gotExpirience} expirience.");
        PlayFabClientAPI.GetCharacterStatistics( new GetCharacterStatisticsRequest() { CharacterId = characterId },
                result =>
                {
                    int expirience = result.CharacterStatistics["Expirience"];
                    UpdateExpirience(characterId, expirience + gotExpirience);
                },
                OnError);
    }

    private void UpdateExpirience(string characterId, int expirience)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest()
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>()
            {
                {"Expirience", expirience }
            },
        },
            result =>
            {
                GetCharacters();
            },
            OnError);
    }

    private void OnDestroy()
    {
        foreach (var slot in _slots)
            slot.SlotButton.onClick.RemoveAllListeners();

        _creareCharacterButton.onClick.RemoveAllListeners();
        _inputField.onValueChanged.RemoveAllListeners();
    }

    private void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                ShowCharacterSlotButton(result.Characters);
            },
            OnError);
    }

    private void ShowCharacterSlotButton(List<CharacterResult> characters)
    {
        foreach (var slot in _slots)
            slot.ShowEmptySlot();

        _index = 0;
        for (int i = 0; i < _slots.Count && i < characters.Count; i++)
        {
            PlayFabClientAPI.GetCharacterStatistics(
                new GetCharacterStatisticsRequest()
                {
                    CharacterId = characters[i].CharacterId,
                },
                result =>
                {
                    int level = result.CharacterStatistics["Level"];
                    int gold = result.CharacterStatistics["Gold"];
                    int damage = result.CharacterStatistics["Damage"];
                    int health = result.CharacterStatistics["Health"];
                    int expirience = result.CharacterStatistics["Expirience"];
                    _slots[_index].ShowCharacerInfoSlot(characters[_index].CharacterId, characters[_index].CharacterName, level, gold, damage, health, expirience);
                    _index++;
                },
                OnError);
        }
            
    }

    private void OnNamedChanged(string changeName)
    {
        _characterName = changeName;
    }

    private void CreateCharacterWithItem()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest()
        {
            CharacterName = _characterName,
            ItemId = "base_ship"
        },
        result =>
        {
            UpdateCharacterStatistics(result.CharacterId);
        },
        OnError);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest()
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>()
            {
                { "Level", 1 },
                {"Gold", 0 },
                {"Damage", 5 },
                {"Health", 100 },
                {"Expirience", 0 }
            },
        },
            result =>
            {
                Debug.Log("Complete!");
                CloseCreateCharacterPanel();
                GetCharacters();
                },
            OnError);
    }

    private void CloseCreateCharacterPanel()
    {
        _newCharacterPanel.SetActive(false);
    }

    private void ShowCreateCharacterPanel()
    {
        _newCharacterPanel.SetActive(true);
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.Log($"Error {errorMessage}");
    }

    private void OnGetAccount(GetAccountInfoResult account)
    {
        _titleLabel.text = $"Username: {account.AccountInfo.Username}";
    }
}

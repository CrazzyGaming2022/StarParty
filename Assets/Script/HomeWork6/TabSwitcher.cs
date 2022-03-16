using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _lobbyPanel;
    [SerializeField] private GameObject _friendsPanel;
    [SerializeField] private GameObject _matchmakingPanel;

    [SerializeField] private Button _toLobbyButton;
    [SerializeField] private Button _toFriendsButton;
    [SerializeField] private Button _toMatchmakingButton;

    private void Start()
    {
        _toLobbyButton.onClick.AddListener(SwitchToLobby);
        _toFriendsButton.onClick.AddListener(SwitchToFriends);
        _toMatchmakingButton.onClick.AddListener(SwitchToMatchmaking);
    }

    private void SwitchToMatchmaking()
    {
        _lobbyPanel.SetActive(false);
        _friendsPanel.SetActive(false);
        _matchmakingPanel.SetActive(true);
    }

    private void SwitchToFriends()
    {
        _lobbyPanel.SetActive(false);
        _friendsPanel.SetActive(true);
        _matchmakingPanel.SetActive(false);
    }

    private void SwitchToLobby()
    {
        _lobbyPanel.SetActive(true);
        _friendsPanel.SetActive(false);
        _matchmakingPanel.SetActive(false);
    }
}


using Photon.Realtime.Demo;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingView : MonoBehaviour
{
    private const int LEVEL = 10;

    [SerializeField] private ConnectAndJoinRandomLb _connectAndJoinRandomLb;
    [SerializeField] private InputField _minLevel;
    [SerializeField] private InputField _maxLevel;
    [SerializeField] private Button _joinRoomButton;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Text _roomNameLabel;

    private void Start()
    {
        _joinRoomButton.onClick.AddListener(JoinRoom);
        _createRoomButton.onClick.AddListener(CreateRoom);
    }

    private void OnDestroy()
    {
        _joinRoomButton.onClick.RemoveAllListeners();
    }

    private void CreateRoom()
    {
        _connectAndJoinRandomLb.RoomCreated += OnRoomCreated;
        _connectAndJoinRandomLb.CreateRoom(LEVEL);
    }

    private void OnRoomCreated(Photon.Realtime.Room room)
    {
        _connectAndJoinRandomLb.RoomCreated += OnRoomCreated;
        _roomNameLabel.text = $"Room name: {room.Name}";
        _createRoomButton.interactable = false;
    }

    private void JoinRoom()
    {
        int minLevel = int.Parse(_minLevel.text);
        int maxLevel = int.Parse(_maxLevel.text);
        _connectAndJoinRandomLb.JoinRoom(minLevel, maxLevel);
    }
}


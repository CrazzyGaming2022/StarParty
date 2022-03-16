using Photon.Realtime.Demo;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomView : MonoBehaviour
{
    [SerializeField] private ConnectAndJoinRandomLb _connectAndJoinRandomLb;
    [SerializeField] private InputField _friendsInput;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Text _roomNameLabel;

    private void Start()
    {
        _createRoomButton.onClick.AddListener(CreateRoom);
    }

    private void OnDestroy()
    {
        _createRoomButton.onClick.RemoveAllListeners();
    }

    private void CreateRoom()
    {
        string[] friends = _friendsInput.text.Split(',');
        _connectAndJoinRandomLb.RoomCreated += OnRoomCreated;
        _connectAndJoinRandomLb.CreateRoom(friends);
    }

    private void OnRoomCreated(Photon.Realtime.Room room)
    {
        _createRoomButton.interactable = false;
        _roomNameLabel.text = $"Room name: {room.Name}";
        _connectAndJoinRandomLb.RoomCreated -= OnRoomCreated;
    }
}


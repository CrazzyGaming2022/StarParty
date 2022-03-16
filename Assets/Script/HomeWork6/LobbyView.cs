using Photon.Realtime;
using Photon.Realtime.Demo;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LobbyView : MonoBehaviour
{
    [SerializeField] private ConnectAndJoinRandomLb _connectAndJoinRandomLb;
    [SerializeField] private Transform _parrent;
    [SerializeField] private RoomView _roomPrefab;

    private void Start()
    {
        _connectAndJoinRandomLb.RoomUpdated += OnRomUpdated;
    }

    private void OnRomUpdated(List<RoomInfo> rooms)
    {
        Clear();

        foreach (var room in rooms)
        {
            var roomView = Instantiate(_roomPrefab, _parrent);
            roomView.Initialize(room.Name);
            roomView.Connect += OnRoomConnect;
        }
    }

    private void Clear()
    {
        for (int i = 0; i < _parrent.childCount; i++)
        {
            var roomView = _parrent.GetChild(i).GetComponent<RoomView>();
            roomView.Connect -= OnRoomConnect;
            Destroy(roomView.gameObject);
        }
    }

    private void OnRoomConnect(RoomView roomView)
    {
        _connectAndJoinRandomLb.ConnectToRoom(roomView.RoomName);
    }

    private void CloseRoom()
    {
        _connectAndJoinRandomLb.CloseRoom();
    }
}


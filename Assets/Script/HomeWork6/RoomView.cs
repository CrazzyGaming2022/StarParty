using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomView : MonoBehaviour
{
    public event Action<RoomView> Connect;

    [SerializeField] private Text _nameLabel;
    [SerializeField] private Button _connectButton;

    public string RoomName => _nameLabel.text;

    public void Initialize(string roomName)
    {
        _nameLabel.text = roomName;
    }

    private void Start()
    {
        _connectButton.onClick.AddListener(OnConnect);
    }

    private void OnConnect()
    {
        Connect?.Invoke(this);
    }
}


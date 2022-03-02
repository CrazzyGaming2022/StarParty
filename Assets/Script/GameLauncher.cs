using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class GameLauncher : MonoBehaviourPunCallbacks
{
	[SerializeField] private Button _connectButton;
	[SerializeField] private Text _connectLabel;

	string gameVersion = "1";

	void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	private void Start()
    {
		_connectButton.onClick.AddListener(Connect);
    }

	public void Connect()
	{
		_connectButton.interactable = false;
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
		}
	}

    private void Disconnect()
    {
		_connectButton.interactable = false;
		PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
	{
		_connectButton.onClick.RemoveAllListeners();
		_connectButton.onClick.AddListener(Disconnect);
		_connectLabel.text = "Disconnect";
		Debug.Log("OnConnectedToMaster() was called by PUN");
		_connectButton.interactable = true;
	}

    public override void OnDisconnected(DisconnectCause cause)
    {
		_connectButton.onClick.RemoveAllListeners();
		_connectButton.onClick.AddListener(Connect);
		_connectLabel.text = "Connect";
		base.OnDisconnected(cause);
		_connectButton.interactable = true;
    }
}
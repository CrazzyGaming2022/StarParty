using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class MyLauncher : MonoBehaviourPunCallbacks
{
	[SerializeField] private Button _connect;
	[SerializeField] private Text _connectText;

	/// <summary>
	/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
	/// </summary>
	string gameVersion = "1";

	/// <summary>
	/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
	/// </summary>
	void Awake()
	{
		// #Critical
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	private void Start()
    {
		_connect.onClick.AddListener(Connect);
    }

	/// <summary>
	/// Start the connection process.
	/// - If already connected, we attempt joining a random room
	/// - if not yet connected, Connect this application instance to Photon Cloud Network
	/// </summary>
	public void Connect()
	{
		_connect.interactable = false;
		// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
		if (PhotonNetwork.IsConnected)
		{
			// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			// #Critical, we must first and foremost connect to Photon Online Server.
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
		}
	}

    private void Disconnect()
    {
		_connect.interactable = false;
		PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
	{
		_connect.onClick.RemoveAllListeners();
		_connect.onClick.AddListener(Disconnect);
		_connectText.text = "Disconnect";
		Debug.Log("OnConnectedToMaster() was called by PUN");
		_connect.interactable = true;
	}

    public override void OnDisconnected(DisconnectCause cause)
    {
		_connect.onClick.RemoveAllListeners();
		_connect.onClick.AddListener(Connect);
		_connectText.text = "Connect";
		base.OnDisconnected(cause);
		_connect.interactable = true;
    }
}
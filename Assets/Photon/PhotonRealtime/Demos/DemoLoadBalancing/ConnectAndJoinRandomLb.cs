// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectAndJoinRandomLb.cs" company="Exit Games GmbH"/>
// <summary>Prototyping / sample code for Photon Realtime.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Realtime.Demo
{
    public class ConnectAndJoinRandomLb : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
    {
        private const string MAP_FIGHT_KEY = "map";
        private const string AI_FIGHT_EKY = "ai";
        private const string NEW_AI_FIGHT_KEY = "new_ai";
        private const string CLASS = "C0";
        private const string LEVEL = "C1";

        [SerializeField]
        private AppSettings appSettings = new AppSettings();
        private LoadBalancingClient _lbc;

        private ConnectionHandler ch;
        public Text StateUiText;

        public void Start()
        {
            this._lbc = new LoadBalancingClient();
            this._lbc.AddCallbackTarget(this);

            if (!this._lbc.ConnectUsingSettings(appSettings))
            {
                Debug.LogError("Error while connecting");
            }

            this.ch = this.gameObject.GetComponent<ConnectionHandler>();
            if (this.ch != null)
            {
                this.ch.Client = this._lbc;
                this.ch.StartFallbackSendAckThread();
            }
        }

        public void Update()
        {
            if (_lbc != null)
            {
                _lbc.Service();

                Text uiText = this.StateUiText;
                string state = _lbc.State.ToString();
                if (uiText != null && !uiText.text.Equals(state))
                    uiText.text = $"User id: {_lbc.UserId}";
            }
        }


        public void OnConnected()
        {
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            var lobby = new TypedLobby("lobby", LobbyType.SqlLobby);
            _lbc.OpJoinLobby(lobby);
            //var options = new RoomOptions()
            //{
            //    MaxPlayers = 12,
            //    CustomRoomPropertiesForLobby = new[] { AI_FIGHT_EKY, NEW_AI_FIGHT_KEY },
            //    CustomRoomProperties = new Hashtable { { AI_FIGHT_EKY, 1 }, { NEW_AI_FIGHT_KEY, 2 } }
            //};
            //var enterRoomParams = new EnterRoomParams
            //{
            //    RoomOptions = options,
            //    ExpectedUsers = new string[] {"23434344"}
            //};
            //_lbc.OpCreateRoom(enterRoomParams);

            //_lbc.OpFindFriends(new[] { "34343434" });
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionListReceived");
            regionHandler.PingMinimumOfRegions(this.OnRegionPingCompleted, null);
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        public void OnJoinedLobby()
        {
            //Host
            var optionRoom = new RoomOptions();
            optionRoom.CustomRoomProperties = new Hashtable() { { LEVEL, 10 } };
            optionRoom.CustomRoomPropertiesForLobby = new[] { LEVEL };
            var enterRoomParams = new EnterRoomParams();
            enterRoomParams.RoomOptions = optionRoom;

            _lbc.OpCreateRoom(enterRoomParams);

            //Client
            var sqlLobbyFilter = $"{LEVEL} BETWEEN {10} AND 100";
            var options = new OpJoinRandomRoomParams
            {
                SqlLobbyFilter = sqlLobbyFilter
            };
            _lbc.OpJoinRandomRoom(options);
        }

        public void OnLeftLobby()
        {
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            Debug.Log(_lbc.CurrentRoom.PlayerCount);

            foreach (var player in _lbc.CurrentRoom.Players.Values)
            {
                Debug.Log(player.UserId);
            }
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
            this._lbc.OpCreateRoom(new EnterRoomParams());
        }

        public void OnLeftRoom()
        {
        }


        /// <summary>A callback of the RegionHandler, provided in OnRegionListReceived.</summary>
        /// <param name="regionHandler">The regionHandler wraps up best region and other region relevant info.</param>
        private void OnRegionPingCompleted(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionPingCompleted " + regionHandler.BestRegion);
            Debug.Log("RegionPingSummary: " + regionHandler.SummaryToCache);
            this._lbc.ConnectToRegionMaster(regionHandler.BestRegion.Code);
        }
    }
}
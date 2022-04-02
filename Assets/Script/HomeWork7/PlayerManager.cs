// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerManager.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the networked player instance
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace Photon.Pun.Demo.PunBasics
{
	#pragma warning disable 649

    /// <summary>
    /// Player manager.
    /// Handles fire Input and Beams.
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Tooltip("The current Health of our player")]
        public float Health = 1f;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;


        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        private GameObject _playerUiPrefab;

        [Tooltip("The Beams GameObject to control")]
        [SerializeField]
        private GameObject beams;

        private bool IsFiring;


        public void Awake()
        {
            if (beams == null)
                Debug.LogError("<Color=Red><b>Missing</b></Color> Beams Reference.", this);
            else
                beams.SetActive(false);

            if (photonView.IsMine)
                LocalPlayerInstance = gameObject;

            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                    _cameraWork.OnStartFollowing();
            }
            else
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
            }

            if (_playerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(_playerUiPrefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

            PlayfabClient.GetUserData("Health", "1", value => Health = float.Parse(value));

			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

		public override void OnDisable()
		{
            base.OnDisable();
			UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
		}

        public void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();

                if (Health <= 0f)
                    GameManager.Instance.LeaveRoom();
            }

            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
                return;

            if (!other.name.Contains("Beam"))
                return;

            Health -= 0.1f;
        }

        public void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
                return;

            if (!other.name.Contains("Beam"))
                return;

            Health -= 0.1f * Time.deltaTime;
        }

        private void OnLevelWasLoaded(int level)
        {
            CalledOnLevelWasLoaded(level);
        }

        private void CalledOnLevelWasLoaded(int level)
        {
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
                transform.position = new Vector3(0f, 5f, 0f);

            GameObject _uiGo = Instantiate(_playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

		private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
		{
			CalledOnLevelWasLoaded(scene.buildIndex);
		}

        private void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                IsFiring = true;
            }

            if (Input.GetButtonUp("Fire1"))
            {
                IsFiring = false;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else
            {
                IsFiring = (bool)stream.ReceiveNext();
                Health = (float)stream.ReceiveNext();
            }
        }
    }
}
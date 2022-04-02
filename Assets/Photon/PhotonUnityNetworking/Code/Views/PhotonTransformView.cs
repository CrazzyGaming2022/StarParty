// ----------------------------------------------------------------------------
// <copyright file="PhotonTransformView.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
//   Component to synchronize Transforms via PUN PhotonView.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


namespace Photon.Pun
{
    using UnityEngine;

    [AddComponentMenu("Photon Networking/Photon Transform View")]
    [HelpURL("https://doc.photonengine.com/en-us/pun/v2/gameplay/synchronization-and-state")]
    public class PhotonTransformView : MonoBehaviourPun, IPunObservable
    {
        private float _distance;
        private float _angle;

        private Vector3 _direction;
        private Vector3 _networkPosition;
        private Vector3 _storedPosition;

        private Quaternion _networkRotation;

        public bool _synchronizePosition = true;
        public bool _synchronizeRotation = true;
        public bool _synchronizeScale = false;

        [Tooltip("Indicates if localPosition and localRotation should be used. Scale ignores this setting, and always uses localScale to avoid issues with lossyScale.")]
        public bool _useLocal;

        bool _firstTake = false;

        public void Awake()
        {
            _storedPosition = transform.localPosition;
            _networkPosition = Vector3.zero;

            _networkRotation = Quaternion.identity;
        }

        private void Reset()
        {
            _useLocal = true;
        }

        void OnEnable()
        {
            _firstTake = true;
        }

        public void Update()
        {
            if (!photonView.IsMine)
            {
                if (_useLocal)

                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, _networkPosition, _distance  * Time.deltaTime * PhotonNetwork.SerializationRate);
                    transform.localRotation = Quaternion.RotateTowards(transform.localRotation, _networkRotation, _angle * Time.deltaTime * PhotonNetwork.SerializationRate);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _networkPosition, _distance * Time.deltaTime * PhotonNetwork.SerializationRate);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, _networkRotation, _angle * Time.deltaTime *  PhotonNetwork.SerializationRate);
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                if (_synchronizePosition)
                    SendPosition(stream);

                if (_synchronizeRotation)
                {
                    if (_useLocal)
                        stream.SendNext(transform.localRotation);
                    else
                        stream.SendNext(transform.rotation);
                }

                if (_synchronizeScale)
                    stream.SendNext(transform.localScale);
            }
            else
            {
                if (_synchronizePosition)
                    ReceivePosition(stream, info);

                if (_synchronizeRotation)
                    ReceiveRotation(stream);

                if (_synchronizeScale)
                    transform.localScale = (Vector3)stream.ReceiveNext();

                if (_firstTake)
                    _firstTake = false;
            }
        }

        private void ReceiveRotation(PhotonStream stream)
        {
            _networkRotation = (Quaternion)stream.ReceiveNext();

            if (_firstTake)
            {
                _angle = 0f;

                if (_useLocal)
                    transform.localRotation = _networkRotation;
                else
                    transform.rotation = _networkRotation;
            }
            else
            {
                if (_useLocal)
                    _angle = Quaternion.Angle(transform.localRotation, _networkRotation);
                else
                    _angle = Quaternion.Angle(transform.rotation, _networkRotation);
            }
        }

        private void ReceivePosition(PhotonStream stream, PhotonMessageInfo info)
        {
            _networkPosition = (Vector3)stream.ReceiveNext();
            _direction = (Vector3)stream.ReceiveNext();

            if (_firstTake)
            {
                if (_useLocal)
                    transform.localPosition = _networkPosition;
                else
                    transform.position = _networkPosition;

                _distance = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _networkPosition += _direction * lag;
                if (_useLocal)
                    _distance = Vector3.Distance(transform.localPosition, _networkPosition);
                else
                    _distance = Vector3.Distance(transform.position, _networkPosition);
            }
        }

        private void SendPosition(PhotonStream stream)
        {
            if (_useLocal)
            {
                _direction = transform.localPosition - _storedPosition;
                _storedPosition = transform.localPosition;
                stream.SendNext(transform.localPosition);
                stream.SendNext(_direction);
            }
            else
            {
                _direction = transform.position - _storedPosition;
                _storedPosition = transform.position;
                stream.SendNext(transform.position);
                stream.SendNext(_direction);
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerAnimatorManager.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the networked player Animator Component controls.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
	public class PlayerAnimatorManager : MonoBehaviourPun 
	{
        [SerializeField] private float _directionDampTime = 0.25f;
        
		private Animator _animator;

	    private void Start () 
	    {
	        _animator = GetComponent<Animator>();
	    }
	        
	    private void Update () 
	    {
	        if( photonView.IsMine == false && PhotonNetwork.IsConnected == true )
	            return;

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);			

            if (stateInfo.IsName("Base Layer.Run"))
                if (Input.GetButtonDown("Fire2")) 
					_animator.SetTrigger("Jump"); 
           
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if( vertical < 0 )
                vertical = 0;

			_animator.SetFloat("Speed", horizontal * horizontal + vertical * vertical);
            _animator.SetFloat( "Direction", horizontal, _directionDampTime, Time.deltaTime );
	    }

	}
}
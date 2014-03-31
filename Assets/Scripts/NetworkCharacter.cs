/*
 * This class is used to update character positions etc on the network 
 */ 

using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
	float lastUpdateTime;

	Animator anim;
	bool gotFirstUpdate = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		/* If the tranform is someone else's, update it. Lerp is used to smooth the network movement */
		if(photonView.isMine) {

		}
		else {
			transform.position = Vector3.Lerp (transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
		}	
	}

	/*
	 * This method is used to send and receive player positions, animations etc from the network 
	 */
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting) {
			//Send our position to network
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(anim.GetFloat("Speed2"));
			stream.SendNext(anim.GetBool("Jumping"));
		}
		else {
			//Receive other player's position


			realPosition = (Vector3) stream.ReceiveNext();
			realRotation = (Quaternion) stream.ReceiveNext();
			anim.SetFloat ("Speed2", (float) stream.ReceiveNext());
			anim.SetBool ("Jumping", (bool) stream.ReceiveNext());

			
			if(!gotFirstUpdate) {
				transform.position = realPosition;
				transform.rotation = realRotation;
				gotFirstUpdate = true;
			}
		}
	}
}

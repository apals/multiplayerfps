/*
 * Controls the flag in what is supposed to be a capture the flag-mode. A bit premature to actually be used
 */

using UnityEngine;
using System.Collections;

public class FlagController : MonoBehaviour {

	bool onGround;
	public Vector3 startPos;

	// Use this for initialization
	void Start () {
		onGround = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	 * If a player collides with this flag, he picks it up
	 */ 
	void OnTriggerEnter(Collider c) {
		Debug.Log("On trigger enter FLAG: " + c);
		if(c.tag != "Player") 
			return;

		/* A player picked up the flag */
		GetComponent<PhotonView>().RPC ("FlagPickup", PhotonTargets.All);
	}

	/*
	 * A remote procedure call to let all connected players that the flag has been reset 
	 */
	[RPC]
	public void Reset() {
		transform.parent = null;
		transform.position = startPos;
	}
	/*
	 * A remote procedure call to let all connected players that the flag has been picked up 
	 */
	[RPC]
	public void FlagPickup() {
		Debug.Log ("the flag has been picked up");
	}
}

/*
 * This class handles bomb behaviour, i.e. explode when touched 
 */

using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

	float speed = 2f;
	float duration = 0.1f;
	ImpactReceiver cc;
	float timeSinceSpawn;
	public GameObject explosion;
	public string owner;

	// Use this for initialization
	void Start () {
		timeSinceSpawn = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceSpawn += Time.deltaTime;
	}

	/*
	 * If a player and a bomb collides, make the bomb explode and apply impact to player 
	 * Send this information to all the connected players */
	void OnTriggerEnter(Collider other) {
		if(timeSinceSpawn < 1)
			return;
		other.GetComponent<Health>().TakeDamage(100, owner);
		cc = other.GetComponent<ImpactReceiver>();
		cc.AddImpact(Vector3.up, 100f);
		PhotonNetwork.Instantiate ("explosion_asteroid", transform.position, transform.rotation, 0);
	//	PhotonNetwork.Destroy(gameObject);
		gameObject.GetPhotonView().RPC ("DestroyBomb", PhotonTargets.All);

	}

	/*
	 * Lets all the connected players know that the bomb has been destroyed
	 */ 
	[RPC]
	void DestroyBomb() {
		if(gameObject.GetPhotonView().isMine) {
			PhotonNetwork.Destroy(gameObject);
		}
	}

	/*
	 * Sets the bomb's owner. If a bomb kills someone, the owner is needed to display the kill and update the scores
	 */ 
	[RPC]
	public void SetOwner(string name) {
		Debug.Log ("Owner: " + name);
		owner = name;
	}


}

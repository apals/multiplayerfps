/*
 * Add this behaviour to all living characters that are able to be killed.
 */ 

using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Health : MonoBehaviour {

	bool dead = false;
	public float hitPoints = 100f;
	public float currentHitPoints;
	public AudioClip death;


	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
	}
	
	// Update is called once per frame
	void Update () {
	}

	/*
	 * If a player takes damage, update his health and let all connected players know
	 */
	[RPC]
	public void TakeDamage(float amount, string shooter) {
		Debug.Log ("TAKEDAMAGE " + name);

		currentHitPoints -= amount;

		if(currentHitPoints <= 0) {
			Die(shooter);
		}
	}

	/*
	 * Set the object's health. Used for respawning
	 */
	[RPC]
	public void SetHealth(float amount) {
		currentHitPoints = amount;
	}

	/* 
	 * A player dies. Update scores and show who killed him in the top right corner of the game
	 */
	void Die(string shooter) {
		if(GetComponent<PhotonView>().instantiationId == 0) {
			Destroy (gameObject);
			return;
		}

			if(!dead)
				AudioSource.PlayClipAtPoint(death, transform.position);
			PhotonView pv = GetComponent<PhotonView>();

			if(!pv.isMine || dead) 
				return;

			/* If the killed player was carrying a flag, reset it */
			FlagController fc = GetComponentInChildren<FlagController>();
			if(fc != null)
				fc.GetComponent<PhotonView>().RPC("Reset", PhotonTargets.All);
			

			/* Display the player X killed player Y message */
			GameObject.FindGameObjectWithTag("GameController").GetPhotonView().RPC("AddInfoMessage_RPC", PhotonTargets.All, shooter + " killed " + PhotonNetwork.player.name);
			dead = true;
					/* Updates the score of the killer */
			foreach(PhotonPlayer killer in PhotonNetwork.playerList) {
				if(killer.name == shooter && shooter != PhotonNetwork.player.name) {
					Hashtable old_props = killer.customProperties;
					Hashtable props = new Hashtable();
					props["Kills"] = (int)old_props["Kills"] + 1;
                   	killer.SetCustomProperties( props );
	                   
				}
			}

			/* Updates the amount of deaths of the killed player */
			Hashtable oldProps = PhotonNetwork.player.customProperties;
			Hashtable newProps = new Hashtable();
			newProps["Deaths"] = (int)oldProps["Deaths"] + 1;
			PhotonNetwork.player.SetCustomProperties( newProps );

			/* Make the player respawn after 8 seconds */
			Invoke ("Respawn", 8f);

            
		//	((MonoBehaviour) gameObject.GetComponent("MouseLook")).enabled = false;
			//	((MonoBehaviour) myPlayerGO.GetComponent("CharacterMotor")).enabled = true;
		//	((MonoBehaviour) gameObject.GetComponent("PlayerMovement")).enabled = false;
		//	((MonoBehaviour) gameObject.GetComponent("PlayerShooting")).enabled = false;


	}

	/* Set the respawning player's health to 100 */
	private void Respawn() {
		Debug.Log ("RESPAWN");
		GetComponent<PhotonView>().RPC ("SetHealth", PhotonTargets.All, 100f);
		dead = false;
	//	((MonoBehaviour) gameObject.GetComponent("MouseLook")).enabled = true;
		//	((MonoBehaviour) myPlayerGO.GetComponent("CharacterMotor")).enabled = true;
	//	((MonoBehaviour) gameObject.GetComponent("PlayerMovement")).enabled = true;
		//((MonoBehaviour) gameObject.GetComponent("PlayerShooting")).enabled = true;
		//((MonoBehaviour) gameObject.GetComponent("MouseLook")).enabled = true;
		//((MonoBehaviour) gameObject.GetComponent("PlayerMovement")).enabled = true;
		//((MonoBehaviour) gameObject.GetComponent("PlayerShooting")).enabled = true;
		//standByCamera.enabled = false;
		//gameObject.transform.FindChild("Main Camera").gameObject.SetActive(true);

	}

}

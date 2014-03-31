/*
 * This class lets player drop bombs
 */

using UnityEngine;
using System.Collections;

public class PlayerBomb : MonoBehaviour {

	int droppedBombs;
	GameObject bomb;
	Object[] bombs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/* If right clicked */
		if (Input.GetMouseButtonDown(1)) {
			droppedBombs = 0;

			bombs = GameObject.FindGameObjectsWithTag("Bomb");
			Debug.Log (bombs.Length);

			
			/* The player should only be able to drop 2 bombs at a time,
			 * so count them before dropping another bomb */
			foreach(GameObject go in bombs) {
				Debug.Log (go);
				Debug.Log (go.GetComponent<BombController>() + " BOMBCONTROLLER");
				if(go.GetComponent<BombController>().owner == PhotonNetwork.player.name){
					droppedBombs++;

					if(droppedBombs > 2)
						return;
				}
			}
			/* Instantiate the bomb on the network */
			bomb = PhotonNetwork.Instantiate("Bomb", transform.position, transform.rotation, 0);
			//((MonoBehaviour) bomb.GetComponent("BombController")).enabled = true;
			bomb.GetPhotonView().RPC("SetOwner", PhotonTargets.All, PhotonNetwork.player.name);

		}
	}

}

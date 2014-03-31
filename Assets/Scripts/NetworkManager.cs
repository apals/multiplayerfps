/*
 * This class handles player connections etc. Lets player join rooms and chat with each other
 */ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviour {

	public Camera standByCamera;
	SpawnSpot[] spawnSpots;
	GameObject myPlayerGO;

    public bool offlineMode = false;
	bool connecting = false;

	List<string> chatMessages;
	int maxChatMessages = 5;

	List<string> infoMessages;
	int maxInfoMessages = 5;

	// Use this for initialization
	void Start () {
        spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
		PhotonNetwork.player.name = PlayerPrefs.GetString ("Username", "biggest noob eu");
		chatMessages = new List<string>();
		infoMessages = new List<string>();

		Hashtable props = new Hashtable();
		props.Add("Kills", 0);
		props.Add("Deaths", 0);
		PhotonNetwork.player.SetCustomProperties( props );
	}

	void OnDestroy() {
		PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
	}

	public void AddInfoMessage(string msg) {
		GetComponent<PhotonView>().RPC ("AddInfoMessage_RPC", PhotonTargets.All, msg);
	}

	[RPC]
	void AddInfoMessage_RPC(string msg) {
		while(infoMessages.Count >= maxInfoMessages) {
			infoMessages.RemoveAt (0);
		}
		infoMessages.Add (msg);
	}

	public void AddChatMessage(string msg) {
		GetComponent<PhotonView>().RPC ("AddChatMessage_RPC", PhotonTargets.All, msg);
	}
	
	[RPC]
	void AddChatMessage_RPC(string msg) {
		while(chatMessages.Count >= maxChatMessages) {
			chatMessages.RemoveAt (0);
        }
        chatMessages.Add (msg);
    }

	void Connect() {
    	PhotonNetwork.ConnectUsingSettings("MultiFPS v002");   
	}

	/*
	 * GUI Stuff. Shows the player's number of kills and deaths, ping, connection state etc, chat messages.
	 */ 	 
	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString ());
		if(myPlayerGO != null) {
			GUILayout.Label ("HP: " + myPlayerGO.GetComponent<Health>().currentHitPoints);
		}
		GUILayout.Label ("Kills: " + PhotonNetwork.player.customProperties["Kills"]);
		GUILayout.Label ("Deaths: " + PhotonNetwork.player.customProperties["Deaths"]);

		if(!PhotonNetwork.connected && !connecting) {

			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();

			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			GUILayout.Label ("Username: ");
			PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
			GUILayout.EndHorizontal();


			if(GUILayout.Button("Single Player")) {
				connecting = true;
				PhotonNetwork.offlineMode = true;
				OnJoinedLobby ();
			}

			if(GUILayout.Button("Multi Player")) {
				connecting = true;
				Connect();
			}

			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();	
		}

		if(PhotonNetwork.connected && !connecting) {


			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
			GUILayout.BeginVertical ();
			GUILayout.FlexibleSpace();
			foreach(string msg in chatMessages) {
				GUILayout.Label(msg);
			}
			GUILayout.EndVertical ();
			GUILayout.EndArea();

			
			
			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical ();


			foreach(string msg in infoMessages) {
				GUILayout.Label(msg);
            }
			GUILayout.EndVertical();
            GUILayout.EndHorizontal();
			GUILayout.EndArea();
            
            
        }
	}

	void OnJoinedLobby() {
		Debug.Log ("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed() {
		Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom (null);
	}

	void OnJoinedRoom() {
		Debug.Log ("OnJoinedRoom");
		connecting = false;
		SpawnMyPlayer();
		//SpawnBot();
	}

	/* 
	 * Spawns the player on the network
	 */
	public void SpawnMyPlayer() {
		AddInfoMessage("Spawning player: " + PhotonNetwork.player.name);

		if(spawnSpots == null) 
			Debug.LogError("WTF EROR OMG NOB");

		SpawnSpot mySpawnSpot = spawnSpots[Random.Range (0, spawnSpots.Length)];
		Debug.Log ("SpawnMyPlayer");        
        myPlayerGO = (GameObject) PhotonNetwork.Instantiate ("PlayerController", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		standByCamera.enabled = false;

		// IMPORTANT 
		/* BY DEFAULT, A LOT OF PLAYER BEHAVIOUR IS DISABLED AT START. THIS IS BECAUSE IF THEY WERE 
		 * ENABLED, ONE PLAYER'S MOVES WOULD AFFECT EVERY PLAYER. THEREFORE THEY ARE ENABLED AFTER 
		 * CLIENT START
		 */ 

		((MonoBehaviour) myPlayerGO.GetComponent("MouseLook")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent("PlayerMovement")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent("PlayerShooting")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent("PlayerChat")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent("Health")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("PlayerBomb")).enabled = true;
		(myPlayerGO.GetComponent<LineRenderer>()).enabled = true;

		myPlayerGO.transform.FindChild("Main Camera").gameObject.SetActive(true);
	}

	void SpawnBot() {
		SpawnSpot mySpawnSpot = spawnSpots[Random.Range (0, spawnSpots.Length)];
		GameObject bot = (GameObject) PhotonNetwork.Instantiate ("Bot", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);

	}



}

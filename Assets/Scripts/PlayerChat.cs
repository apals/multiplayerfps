/* 
 * This class handles player chatting. 
 * A player presses Y to open the chat box.
 */

using UnityEngine;
using System.Collections;

public class PlayerChat : MonoBehaviour {
	PhotonView pv;
	bool chatBoxOpen = false;
	string msg = "";
	// Use this for initialization
	void Start () {
		Input.eatKeyPressOnTextFieldFocus = false;
		pv = GameObject.FindGameObjectWithTag("GameController").GetPhotonView();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Y)) {
			OpenChatBox();
		}

		if(Input.GetKeyDown (KeyCode.L)) {
			Debug.Log (PhotonNetwork.player.customProperties["Kills"] );
		}
	}

	void OnGUI() {
		/* Display chat box */
		if(chatBoxOpen) {
			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();

			GUI.SetNextControlName("ChatBox");
			msg = GUILayout.TextField(msg);
			GUI.FocusControl("ChatBox");

			GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndArea();

			/* Send the written message to all connected players */
			if(Input.GetKeyDown (KeyCode.Return)) {
				GUIUtility.keyboardControl = 0;
				pv.RPC ("AddChatMessage_RPC", PhotonTargets.All, PhotonNetwork.player.name + ": " + msg);
				chatBoxOpen = false;
				msg = "";
                
            }
        }
   
	}

	void OpenChatBox() {
		chatBoxOpen = true;
	}
}

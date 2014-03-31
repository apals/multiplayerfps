/*
 * This class is only used to let the player hide the cursor by pressing escape
 */

using UnityEngine;
using System.Collections;

public class hidecursor : MonoBehaviour {

	bool paused = false;

	// Use this for initialization
	void Start () {
		//Screen.showCursor = false;
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!paused)
			Screen.lockCursor = true;

		if(Input.GetKeyDown(KeyCode.Escape)) {
			paused = !paused;
		}
	
	}
}

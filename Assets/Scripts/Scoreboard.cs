/* 
 * Scoreboard. Known bugs: 2 players with the same name will both get points if one of them kills a player.
 * This is due to name being used as an identifier in the hashmap.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Scoreboard : MonoBehaviour {
	
	public GUISkin guiSkin;
	public Font titleFont;
	public Texture2D background;
	public bool display = false;
	
	class Line {
		public string name;
		public int kills;
		public int deaths;
		
		public Line(string n, int k, int d) {
			name = n;
			kills = k;
			deaths = d;
		}
	}
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.Tab)) {
			display = !display;
		}
	}
	
	void OnGUI() {
		if(!display || !PhotonNetwork.connected) 
			return;
		
		List<Line> lines = new List<Line>();
		
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			Hashtable props = player.customProperties;
			Line l = new Line(player.name, (int)props["Kills"], (int)props["Deaths"]);
			lines.Add(l);
		}
		
		// TODO: Sort.
		
		GUI.skin = guiSkin;
		GUIStyle rightTitle = new GUIStyle(guiSkin.GetStyle("label"));
		rightTitle.alignment = TextAnchor.MiddleRight;
		rightTitle.font = titleFont;
		GUIStyle leftTitle = new GUIStyle(guiSkin.GetStyle("label"));
		leftTitle.font = titleFont;
		
		Rect win = new Rect(Screen.width/2 - 160, Screen.height/2-200, 320, 480);
		GUI.DrawTexture(win, background);
		win = new Rect(Screen.width/2 - 150, Screen.height/2-200, 300, 480);
		
		GUILayout.BeginArea( win );
		GUILayout.BeginVertical();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Scoreboard", leftTitle);

		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name", 	GUILayout.Width(135));
		GUILayout.Label("Kills", 	GUILayout.Width(55));
		GUILayout.Label("Deaths", 	GUILayout.Width(55));
		GUILayout.EndHorizontal();
		
		foreach(Line l in lines) {
			GUILayout.BeginHorizontal();
			GUILayout.Label(l.name, 				GUILayout.Width(150));
			GUILayout.Label(l.kills.ToString(), 	GUILayout.Width(55));
			GUILayout.Label(l.deaths.ToString(), 	GUILayout.Width(55));
			GUILayout.EndHorizontal();
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		
	}
}

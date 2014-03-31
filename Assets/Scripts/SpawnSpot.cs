/* 
 * This class will be used to determine whether a spawn spot is usable for a player, i.e. if he 
 * is in team A the player should not be able to spawn on team B's side
 */ 

using UnityEngine;
using System.Collections;

public class SpawnSpot : MonoBehaviour {

	public int teamID = 0;
}

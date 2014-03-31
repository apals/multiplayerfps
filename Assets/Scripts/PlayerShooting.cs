/*
 * This class handles player shooting. As of writing this, player shooting only consists of pink lines. 
 * Todo: make the player shoot projectiles instead of pink lines
 */

using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

	public float fireRate = 20f;
	float cooldown = 0;
	public float damage = 25f;
	public LineRenderer line;
	public Material lineMaterial;
	public AudioClip shoot; 

	bool paused = false;

	// Use this for initialization	
	void Start () {
		/* Set the look of the line */
		line.SetVertexCount(2);
		line.renderer.material = lineMaterial;
		line.SetWidth(0.05f, 0.05f);        
    }
    
    // Update is called once per frame
	void Update () {
		/* Cooldown is used to prevent the user from shooting to often */
		cooldown -= Time.deltaTime;

	//	if(cooldown < fireRate/0.01) 
		//	line.enabled = false;

		/* If the user clicks mouse1, try to fire */
		if(Input.GetButton("Fire1")) {
			Fire();
		}


		/* The code below in this method handles cursor locking. The user's mouse pointer should not move outside the
		 * game screen 
		 */
		if(Input.GetKeyDown(KeyCode.Escape)) {
			paused = !paused;
		}

		if(!paused)
			Screen.lockCursor = true;
		else 	
			Screen.lockCursor = false;
	}

	void Fire() {
		if(cooldown > 0) {
			return;
		}
		//AudioSource.PlayClipAtPoint(shoot, transform.position);
		//Debug.Log ("Fire");

		/* Shoots a ray from the player */
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		Vector3 hitPoint;
		Transform hitTransform = FindClosestHitObject(ray, out hitPoint);
        
        if(hitTransform != null) {
		//	Debug.Log ("I hit: " + hitTransform.transform.name);
			//Special effect at hit location?
			//mb DoExplosionAt(hitPoint);

			/* Draw a line from the player */
			gameObject.GetComponent<PhotonView>().RPC ("Beam", PhotonTargets.All, transform.position + new Vector3(0, 1.2f, 0), hitPoint);

			/* Get information about a potentially hit player */
			Health h = hitTransform.GetComponent<Health>();

			while(h == null && hitTransform.parent) {
				hitTransform = hitTransform.parent;
				h = hitTransform.GetComponent<Health>();
			}

			/* If a player is hit, make the hit player take damage */
			if(h != null) {
				//h.TakeDamage(damage);
				//tell EVERYONE (.ALL) that the crate is taking damage
				h.GetComponent<PhotonView>().RPC ("TakeDamage", PhotonTargets.AllBuffered, damage, PhotonNetwork.player.name);
			}
		}
		

		cooldown = fireRate;
	}

	/*
	 * If a ray hits multiple targets, this method chooses and returns the one closest to the origin 
	 */ 
	Transform FindClosestHitObject(Ray ray, out Vector3 hitPoint) {

		RaycastHit[] hits = Physics.RaycastAll (ray);
		Transform closestHit = null;
		float distance = 0;
		hitPoint = Vector3.zero;

		foreach(RaycastHit hit in hits) {
			if(hit.transform != this.transform && (closestHit == null || hit.distance < distance)) {
				//Hit something that is not us && either the first thing or something with at a lesser distance

				closestHit = hit.transform;
				distance = hit.distance;
				hitPoint = hit.point;
			}
		}


		return closestHit;
	}

	/* 
	 * Draw a line from orig to dest 
	 */
	[RPC]
	public void Beam(Vector3 orig, Vector3 dest) {
	//	Debug.Log ("Beam:" + orig + " " + dest);

		
		line.enabled = true;
		line.SetPosition(0, orig - new Vector3(0f, 0.1f, 0));
		line.SetPosition(1, dest);
	//	Debug.Log ("LineRenderer renderar at:" + orig + " : till : " + dest);
		Invoke ("RemoveLine", fireRate/2);
	}

	private void RemoveLine() {
		line.enabled = false;
	}



}

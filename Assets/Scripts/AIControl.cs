/* 
 * A very premature AI. Extremely bad, and not used. Update with A*-algorithms.
 */ 

using UnityEngine;
using System.Collections;

public class AIControl : MonoBehaviour {
	
	
	float speed = 3f;
	float jumpSpeed = 9f;
	float verticalVelocity = 0;
	GameObject target;
	
	Vector3 direction = Vector3.zero;
	CharacterController cc;
	Animator anim;
	
	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
		target = GameObject.FindGameObjectWithTag("Player");
		Debug.Log ("TARget start: " + target);
	}
	
	// Update is called once per frame
	void Update () {
		target = GameObject.FindGameObjectWithTag("Player");
		if(target != null)  {
			direction = target.transform.position - transform.position;
			Debug.Log ("Target: " + target.name);
		}
				
	}
	
	void FixedUpdate() {
		Vector3 dist = direction * speed * Time.deltaTime;
		
		if(cc.isGrounded && verticalVelocity < 0) {
			verticalVelocity = Physics.gravity.y * Time.deltaTime;
		}
		else { 
			if(Mathf.Abs(verticalVelocity) > 0) {
			}			
		}
		
		
		verticalVelocity += Physics.gravity.y * Time.deltaTime;
		
		dist.y = verticalVelocity * Time.deltaTime;
		
		cc.Move (dist);
		Debug.Log ("Bot moves: " + dist);
	}
}

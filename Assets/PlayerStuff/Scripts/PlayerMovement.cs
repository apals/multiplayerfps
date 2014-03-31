/*
 * This class handles Player movement
 */

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


	float speed = 10f;
	float jumpSpeed = 9f;
	float verticalVelocity = 0;

	Vector3 direction = Vector3.zero;
	CharacterController cc;
	Animator anim;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		/* When the mouse is moved the player is rotated accordingly */
		direction = transform.rotation * new Vector3(Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical")).normalized;

		if(direction.magnitude > 1f) {
			direction = direction.normalized;
		}

		anim.SetFloat ("Speed2", direction.magnitude);


		if(cc.isGrounded && Input.GetButton ("Jump")) {
			verticalVelocity = jumpSpeed;
		}


	}

	/* Called once per physics tick */
	void FixedUpdate() {
		Vector3 dist = direction * speed * Time.deltaTime;

		/* Checks if the player is falling/jumping and changes the animation accordingly */
		if(cc.isGrounded && verticalVelocity < 0) {
			anim.SetBool ("Jumping", false);
			verticalVelocity = Physics.gravity.y * Time.deltaTime;
		}
		else { 
			if(Mathf.Abs(verticalVelocity) > 0) {
				anim.SetBool ("Jumping", true);
			}			
		}

		/* Calculates the players vertical velocity (i.e. falling or jumping) */
		verticalVelocity += Physics.gravity.y * Time.deltaTime;

		dist.y = verticalVelocity * Time.deltaTime;

		cc.Move (dist);
	}
}

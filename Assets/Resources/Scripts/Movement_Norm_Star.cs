﻿using UnityEngine;
using System.Collections;

public class Movement_Norm_Star : MonoBehaviour {
	
	//Inspector Variables
	private float playerSpeed = 2f; 
	//speed player moves

	private Vector2 movTarget;
	// where the player is to move towards

	private Vector2 dampSpeed = Vector2.zero;
	// the dampspeed for smoothdamping player movement

	private float smoothTime = 0.5f;
	// the smoothdamping delay

	private Vector2 velTarget;
	// the target velocity based on the differenct between player position and movTarget

	void Start()
	{
		
	}

	void Update () 
	{
		// calibrate movTarget with player position
		movTarget.x = transform.position.x;
		movTarget.y = transform.position.y;

		// Player Movement
		moveFunct();

		//Player Turning w/ mouse
		rotate();

	}

	void moveFunct()
	{
		// add to movTarget based on key input
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {//Press up arrow key to move forward on the Y AXIS
			movTarget += new Vector2(0f, playerSpeed * Time.deltaTime / (transform.localScale.x));

		} 
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {//Press down arrow key to move backward on the Y AXIS
			movTarget += new Vector2(0f, -playerSpeed * Time.deltaTime / (transform.localScale.x));

		} 
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {//Press right arrow key to move forward on the X AXIS
			movTarget += new Vector2(playerSpeed * Time.deltaTime / (transform.localScale.x), 0f);

		} 
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {//Press left arrow key to move backward on the X AXIS
			movTarget += new Vector2(-playerSpeed * Time.deltaTime / (transform.localScale.x), 0f);

		} 

		// if no movement input, set velocity target to zero
		if (! Input.GetKey(KeyCode.UpArrow) && ! Input.GetKey(KeyCode.DownArrow) && ! Input.GetKey(KeyCode.RightArrow) && ! Input.GetKey(KeyCode.LeftArrow)
			&& ! Input.GetKey(KeyCode.W) && ! Input.GetKey(KeyCode.A) && ! Input.GetKey(KeyCode.S) && ! Input.GetKey(KeyCode.D)){
			velTarget = new Vector2 (0f, 0f);
		}

		// set target velocity
		velTarget.x = (movTarget.x - transform.position.x)/Time.deltaTime;
		velTarget.y = (movTarget.y - transform.position.y)/Time.deltaTime;

		// accelerate the player to the target velocity with smoothdamp
		GetComponent<Rigidbody2D> ().velocity = Vector2.SmoothDamp (GetComponent<Rigidbody2D> ().velocity, velTarget, ref dampSpeed, smoothTime);

	}

	void rotate() {
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = Input.mousePosition - pos;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
	}
}

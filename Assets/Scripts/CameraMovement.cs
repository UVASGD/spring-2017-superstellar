using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour {
	
	public GameObject player;       //sets reference to player game object
	public GameObject bg;			//sets reference to game map
	
	float mapX;
	float mapY;
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;
	
	private Vector3 offset;         //Private variable to store the offset distance between the player and camera
	private float offsetZoom;

	private float targetOrtho;
	private float smoothSpeed = 5.0f;

	private Shooting_Controls_edit shootScript;
	private List<float> starSize = new List<float>(13);
	
	// Use this for initialization
	void Start () 
	{
		targetOrtho = Camera.main.orthographicSize;
		mapX = bg.transform.localScale.x;
		mapY = bg.transform.localScale.y;
		
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = transform.position - player.transform.position;
		
		float vertExtent = Camera.main.orthographicSize;    
		float horzExtent = vertExtent * Screen.width / Screen.height;
		
		// Calculations assume map is position at the origin
		minX = horzExtent - mapX / 2.0f;
		maxX = mapX / 2.0f - horzExtent;
		minY = vertExtent - mapY / 2.0f;
		maxY = mapY / 2.0f - vertExtent;

		shootScript = player.GetComponent<Shooting_Controls_edit>();
		starSize = shootScript.starSizes;


	}
	
	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.on;
		Vector3 v3 = player.transform.position;
		
		v3.x = Mathf.Clamp(v3.x, minX, maxX);
		v3.y = Mathf.Clamp(v3.y, minY, maxY);
		
		transform.position = v3 + offset;


		//Zoom when star changes size
		int starClass = shootScript.starType;
		targetOrtho = starSize [starClass - 1]*5f;
		Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
	}



}

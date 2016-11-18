using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMovement : Photon.MonoBehaviour {

	private GameObject player;       //sets reference to player game object
	public GameObject bg;			//sets reference to game map

	// map dimensions
	float mapX;
	float mapY;

	// bounds that camera is limited to
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;
	
	private Vector3 offset;         //Private variable to store the offset distance between the player and camera

	// camera zoom level and the speed the camera changes zoom
	private float targetOrtho = 5f;
	private float smoothSpeed = 5.0f;

	// accesses player data
	private Shooting_Controls_edit shootScript;
	private List<float> starSize = new List<float>(13);

	private GameObject parent;

	private Transform cameraTransform;

	void OnEnable()
	{
		this.transform.SetParent(GameObject.Find("Player(Clone)").transform);
		parent = this.transform.parent.gameObject;

		player = parent.transform.FindChild ("Star").gameObject;
	

		PhotonView photonView = parent.GetPhotonView ();
		if (photonView != null && !photonView.isMine) {
//			Debug.Log ("entered disable camera");
			this.enabled = false;
			return;
		} else {
			//			Debug.Log ("enabled camera : " + photonView.ownerId);
//			Camera.main.transform.tag = "Untagged";
//			this.GetComponent<Camera>().transform.tag = "MainCamera";
		}

		//		Debug.Log (this.photonView.isMine);
		if (!cameraTransform && Camera.main)
			cameraTransform = Camera.main.transform;
		if (!cameraTransform) {
			enabled = false;
		}

		bg = GameObject.Find ("Background");
		mapX = bg.transform.localScale.x;
		mapY = bg.transform.localScale.y;

		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = cameraTransform.position - transform.position;
		//
		float vertExtent = Camera.main.orthographicSize;
		float horzExtent = vertExtent * Screen.width / Screen.height;

		// Calculations assume map is position at the origin
		minX = horzExtent - mapX / 2.0f;
		maxX = mapX / 2.0f - horzExtent;
		minY = vertExtent - mapY / 2.0f;
		maxY = mapY / 2.0f - vertExtent;

		//m_CameraTransformCamera = cameraTransform.GetComponent<Camera> ();
	}

	// Use this for initialization
	void Start () 
	{
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

		shootScript = parent.gameObject.GetComponentInChildren<Shooting_Controls_edit>();
		starSize = shootScript.starSizes;
	}
	

	void Update () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.on;
		Vector3 v3 = player.transform.position;

		v3.x = Mathf.Clamp(v3.x, minX, maxX);
		v3.y = Mathf.Clamp(v3.y, minY, maxY);
		
		transform.position = v3 + offset;

		//Zoom when star changes size
		int starClass = shootScript.starType;

		targetOrtho = starSize[starClass - 1]*5f;

		this.GetComponent<Camera>().orthographicSize = Mathf.MoveTowards (this.GetComponent<Camera>().orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
	}



}

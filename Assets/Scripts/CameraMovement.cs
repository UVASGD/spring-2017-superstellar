using UnityEngine;
using System.Collections;

public class CameraMovement : Photon.MonoBehaviour {

	public Transform cameraTransform;
//	private Camera m_CameraTransformCamera;
//	public GameObject player;       //sets reference to player game object
	public GameObject bg;			//sets reference to game map

	float mapX;
	float mapY;
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;



	private Vector3 offset;         //Private variable to store the offset distance between the player and camera


	// Use this for initialization
	void Start () 
	{
		bg = GameObject.Find ("Background");
		mapX = bg.transform.localScale.x;
		mapY = bg.transform.localScale.y;
//
//		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = cameraTransform.position - transform.position;
//
		float vertExtent = Camera.main.orthographicSize;    
		float horzExtent = vertExtent * Screen.width / Screen.height;
//
//		// Calculations assume map is position at the origin
		minX = horzExtent - mapX / 2.0f;
		maxX = mapX / 2.0f - horzExtent;
		minY = vertExtent - mapY / 2.0f;
		maxY = mapY / 2.0f - vertExtent;
//
	}

	void OnEnable()
	{
		if (this.photonView != null && !this.photonView.isMine) {
//			Debug.Log("entered disable");
			this.enabled = false;
			return;
		}

//		Debug.Log (this.photonView.isMine);
		if (!cameraTransform && Camera.main)
			cameraTransform = Camera.main.transform;
		if (!cameraTransform) {
//			Debug.Log ("Please assign a camera to the ThirdPersonCamera script.");
			enabled = false;
		}

		bg = GameObject.Find ("Background");
		mapX = bg.transform.localScale.x;
		mapY = bg.transform.localScale.y;
		//
		//		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = cameraTransform.position - transform.position;
		//
		float vertExtent = Camera.main.orthographicSize;    
		float horzExtent = vertExtent * Screen.width / Screen.height;
		//
		//		// Calculations assume map is position at the origin
		minX = horzExtent - mapX / 2.0f;
		maxX = mapX / 2.0f - horzExtent;
		minY = vertExtent - mapY / 2.0f;
		maxY = mapY / 2.0f - vertExtent;

//		m_CameraTransformCamera = cameraTransform.GetComponent<Camera> ();
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		if (this.enabled) {
			updatePosition();
//			PhotonView photonView = PhotonView.Get (this);
//			photonView.RPC ("updatePosition", PhotonTargets.All, null);
		}
	}
		

	void updatePosition() {

		Vector3 v3 = transform.position;

		v3.x = Mathf.Clamp(v3.x, minX, maxX);
		v3.y = Mathf.Clamp(v3.y, minY, maxY);

		cameraTransform.position = v3 + offset;
	} 

}

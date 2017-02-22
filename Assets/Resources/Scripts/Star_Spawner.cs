 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star_Spawner : Photon.MonoBehaviour {

	private GameObject container;
	public GameObject starSpawn; // item to spawn
	public GameObject bg; // background object

	// map dimensions
	private float mapX;
	private float mapY;

	// bounds that spawner is limited to
	private int maxX;
	private int maxY;

	public float speed  = 0.5f; // frequency that item is spawned
	public int numSpawn = 1000; // max number of item to exist at once
	private List<GameObject> spawnedStuff = new List<GameObject>(); // holds all the spawned items


	private float nextActionTime = 0.0f;
	public float period = 0.1f;

	private PhotonView ScenePhotonView;


	void Start () {

		container = GameObject.Find("BG Stars");

		// set spawner bounds
		mapX = bg.transform.localScale.x;
		mapY = bg.transform.localScale.y;

		maxX = (int) mapX / 2;
		maxY = (int) mapY / 2;


		Random.seed = 80;

//		InvokeRepeating ("Generate", 0, speed);

		ScenePhotonView = this.GetComponent<PhotonView> ();
	}

	void Update() {
			if (Time.time > nextActionTime) {
				nextActionTime += period;
				ScenePhotonView.RPC ("Generate", PhotonTargets.All);
			}
	}
	

	[PunRPC]
	void Generate (){
		if (PhotonNetwork.isMasterClient) {
			Debug.Log ("generating bg stars");
			// check to see if max spawn is reached
			if (spawnedStuff.Count < numSpawn) {

				// generate random spawn location
				int x = Random.Range (-maxX, maxX);
				int y = Random.Range (-maxY, maxY);
				Vector3 target = new Vector3 (x, y, 0);

				// spawn star and give it random rotation, torque, and force, then add it to the list
				GameObject spawnedStar = PhotonNetwork.InstantiateSceneObject (starSpawn.name, target, Quaternion.identity, 0, null);
				spawnedStar.transform.SetParent (container.transform);

				spawnedStar.GetComponent<Rigidbody2D> ().AddTorque (Random.Range (-100, 100));
				int RandAngle = Random.Range (0, 360);
				spawnedStar.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
				spawnedStuff.Add (spawnedStar);

			} else {

				// generate random number and see if the item in the list has been destroyed -> if so, replace it by spawning a star
				int index = Random.Range (0, numSpawn);
				if (spawnedStuff [index] == null) {

					spawnedStuff.Remove (spawnedStuff [index]);
					int x = Random.Range (-maxX, maxX);
					int y = Random.Range (-maxY, maxY);

					Vector3 target = new Vector3 (x, y, 0);

					GameObject spawnedStar = Instantiate (starSpawn, target, Quaternion.identity) as GameObject;
					spawnedStar.transform.SetParent (container.transform);

					spawnedStar.GetComponent<Rigidbody2D> ().AddTorque (Random.Range (-100, 100));
					int RandAngle = Random.Range (0, 360);
					spawnedStar.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
					spawnedStuff.Add (spawnedStar);

		
				}
			}
		}
	}
}

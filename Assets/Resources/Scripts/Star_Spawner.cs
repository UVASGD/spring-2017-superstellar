using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star_Spawner : Photon.MonoBehaviour {


	public GameObject starSpawn; // item to spawn
	public GameObject starGreenSpawn;
	public GameObject starRedSpawn;
	public GameObject starBlueSpawn;
	public GameObject blazarSpawn;
	public GameObject magnetarSpawn;
	public GameObject cometSpawn;
	public GameObject rogueSpawn;
	public GameObject bg; // background object

	private GameObject container;

	// map dimensions
	private float mapX;
	private float mapY;

	// bounds that spawner is limited to
	private int maxX;
	private int maxY;

	public float starSpeed; // frequency that item is spawned
	public int numStarSpawn; // max number of item to exist at once
	private List<GameObject> spawnedStars = new List<GameObject>(); // holds all the spawned items

	public float blazarSpeed; // frequency that item is spawned
	public int numBlazarSpawn; // max number of item to exist at once
	private List<GameObject> spawnedBlazars = new List<GameObject>(); // holds all the spawned items

	public float magnetarSpeed; // frequency that item is spawned
	public int numMagnetarSpawn; // max number of item to exist at once
	private List<GameObject> spawnedMagnetars = new List<GameObject>(); // holds all the spawned items

	public float cometSpeed; // frequency that item is spawned
	public int numCometSpawn; // max number of item to exist at once
	private List<GameObject> spawnedComets = new List<GameObject>(); // holds all the spawned items

	public float rogueSpeed; // frequency that item is spawned
	public int numRogueSpawn; // max number of item to exist at once
	private List<GameObject> spawnedRogues = new List<GameObject>(); // holds all the spawned items

	public float chanceToSpawnGreen;
	public float chanceToSpawnRed;
	public float chanceToSpawnBlue;

	public float periodBG;
	public float periodBlazar;
	public float periodMagnetar;
	public float periodComet;
	public float periodRogue;

	private float nextActionTime = 3.0f;
	private float nextBlazarTime = 0.5f;
	private float nextMagnetarTime = 1.0f;
	private float nextCometTime = 1.5f;
	private float nextRogueTime = 2.0f;

	private PhotonView ScenePhotonView;

	public bool makeStars;
	public bool makeBlazar;
	public bool makeMagnetar;
	public bool makeComet;
	public bool makeRogue;

	public bool testSpawn;
	public float testX;
	public float testY;
	private Vector3 testLocation;


	void Start () {
		
		ScenePhotonView = this.GetComponent<PhotonView> ();

		container = GameObject.Find("BG Stars");

		// set spawner bounds
		mapX = bg.transform.localScale.x;
		mapY = bg.transform.localScale.y;

		maxX = (int) mapX / 2 - 25;
		maxY = (int) mapY / 2 - 25;

//		Random.seed = 80;

	}

	void Update() {
		if (PhotonNetwork.isMasterClient) {
			testLocation = new Vector3 (testX, testY, 0f);
			if (Time.time > nextActionTime && makeStars) {
				nextActionTime += periodBG;
				GenerateBGStar (); //ScenePhotonView.RPC ("GenerateBGStar", PhotonTargets.All);
			}
			if (Time.time > nextBlazarTime && makeBlazar) {
				nextBlazarTime += periodBlazar;
//			ScenePhotonView.RPC ("GenerateBlazar", PhotonTargets.All);
				GenerateBlazar ();
			}
			if (Time.time > nextMagnetarTime && makeMagnetar) {
				nextMagnetarTime += periodMagnetar;
//			ScenePhotonView.RPC ("GenerateMagnetar", PhotonTargets.All);
				GenerateMagnetar ();
			}
			if (Time.time > nextCometTime && makeComet) {
				nextCometTime += periodComet;
//			ScenePhotonView.RPC ("GenerateComet", PhotonTargets.All);
				GenerateComet ();
			}
			if (Time.time > nextRogueTime && makeRogue) {
				nextRogueTime += periodRogue;
//			ScenePhotonView.RPC ("GenerateRogue", PhotonTargets.All);
				GenerateRogue ();
			}
		}
		foreach (GameObject star in spawnedStars) {
			Debug.Log (star.GetComponent<Rigidbody2D> ().angularVelocity);
		}
	}
		

	void GenerateBGStar () {
		// check to see if max spawn is reached
		if (spawnedStars.Count < numStarSpawn) {

			// generate random spawn location
			int x = Random.Range (-maxX, maxX);
			int y = Random.Range (-maxY, maxY);
			Vector3 target = new Vector3 (x, y, 0);
//				Debug.Log (x + ", " + y);

			if (testSpawn) {
				target = testLocation;
			}

			// spawn star and give it random rotation, torque, and force, then add it to the list
			GameObject spawnedStar;
			int n = Random.Range (0, numStarSpawn);

			if (n <= chanceToSpawnBlue * numStarSpawn) { //spawn green star
				spawnedStar = PhotonNetwork.InstantiateSceneObject (starBlueSpawn.name, target, Quaternion.identity, 0, null);
			} else if (n <= chanceToSpawnRed * numStarSpawn) { //spawn green star
				spawnedStar = PhotonNetwork.InstantiateSceneObject (starRedSpawn.name, target, Quaternion.identity, 0, null);
			} else if (n <= chanceToSpawnGreen * numStarSpawn) { //spawn green star
				spawnedStar = PhotonNetwork.InstantiateSceneObject (starGreenSpawn.name, target, Quaternion.identity, 0, null);
			} else { //spawn normal star
				spawnedStar = PhotonNetwork.InstantiateSceneObject (starSpawn.name, target, Quaternion.identity, 0, null);
			}
			spawnedStar.GetComponent<Rigidbody2D> ().AddTorque (Random.Range (-100, 100));
			int RandAngle = Random.Range (0, 360);
			spawnedStar.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
			spawnedStar.transform.SetParent (container.transform);
			spawnedStars.Add (spawnedStar);
		} 
		else {

			// generate random number and see if the item in the list has been destroyed -> if so, replace it by spawning a star
			int index = Random.Range (0, numStarSpawn);
			if (spawnedStars [index] == null) {

				spawnedStars.Remove (spawnedStars [index]);
				int x = Random.Range (-maxX, maxX);
				int y = Random.Range (-maxY, maxY);

				Vector3 target = new Vector3 (x, y, 0);

				if (testSpawn) {
					target = testLocation;
				}

				GameObject spawnedStar;
				int n = Random.Range (0, numStarSpawn);
				if (n <= chanceToSpawnBlue * numStarSpawn) { //spawn green star
					spawnedStar = PhotonNetwork.InstantiateSceneObject (starBlueSpawn.name, target, Quaternion.identity, 0, null);
				} else if (n <= chanceToSpawnRed * numStarSpawn) { //spawn green star
					spawnedStar = PhotonNetwork.InstantiateSceneObject (starRedSpawn.name, target, Quaternion.identity, 0, null);
				} else if (n <= chanceToSpawnGreen * numStarSpawn) { //spawn green star
					spawnedStar = PhotonNetwork.InstantiateSceneObject (starGreenSpawn.name, target, Quaternion.identity, 0, null);
				} else { //spawn normal star
					spawnedStar = PhotonNetwork.InstantiateSceneObject (starSpawn.name, target, Quaternion.identity, 0, null);
				}

				spawnedStar.GetComponent<Rigidbody2D> ().AddTorque (Random.Range (-100, 100));
				int RandAngle = Random.Range (0, 360);
				spawnedStar.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
				spawnedStar.transform.SetParent (container.transform);
				spawnedStars.Add (spawnedStar);
			}
		}

	}
		
	void GenerateBlazar () {
	// check to see if max spawn is reached
	//Debug.Log(spawnedBlazars.Count);
	if (spawnedBlazars.Count < numBlazarSpawn) {

		// generate random spawn location
		int x = Random.Range (-maxX+5, maxX-5);
		int y = Random.Range (-maxY+5, maxY-5);
		Vector3 target = new Vector3 (x, y, 0);

		if (testSpawn) {
			target = testLocation;
		}

		// spawn star and give it random rotation, torque, and force, then add it to the list
		//Debug.Log("Blazar Pre-Spawned");
		GameObject spawnedBlazar = PhotonNetwork.InstantiateSceneObject (blazarSpawn.name, target, Quaternion.identity, 0, null);
		spawnedBlazar.transform.SetParent (container.transform);
		Rigidbody2D myRigidBody;
		myRigidBody = spawnedBlazar.GetComponentInChildren<Rigidbody2D>();
		myRigidBody.AddTorque (Random.Range (-100, 100));
		int RandAngle = Random.Range (0, 360);
		myRigidBody.AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
		spawnedBlazars.Add (spawnedBlazar);
		//Debug.Log("Blazar Added to List");

	} else {

		// generate random number and see if the item in the list has been destroyed -> if so, replace it by spawning a star
		int index = Random.Range (0, numBlazarSpawn);
		if (spawnedBlazars [index] == null) {

			spawnedBlazars.Remove (spawnedBlazars [index]);
			int x = Random.Range (-maxX, maxX);
			int y = Random.Range (-maxY, maxY);

			Vector3 target = new Vector3 (x, y, 0);

			if (testSpawn) {
				target = testLocation;
			}

			GameObject spawnedBlazar = PhotonNetwork.InstantiateSceneObject (blazarSpawn.name, target, Quaternion.identity, 0, null);
			spawnedBlazar.transform.SetParent (container.transform);
			Rigidbody2D myRigidBody;
			myRigidBody = spawnedBlazar.GetComponentInChildren<Rigidbody2D>();
			myRigidBody.AddTorque (Random.Range (-100, 100));
			int RandAngle = Random.Range (0, 360);
			myRigidBody.AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
			spawnedBlazars.Add (spawnedBlazar);


		}
	}
	}
		
	void GenerateMagnetar () {
		// check to see if max spawn is reached
		if (spawnedMagnetars.Count < numMagnetarSpawn) {

			// generate random spawn location
			int x = Random.Range (-maxX, maxX);
			int y = Random.Range (-maxY, maxY);
			Vector3 target = new Vector3 (x, y, 0);

			if (testSpawn) {
				target = testLocation;
			}

			// spawn star and give it random rotation, torque, and force, then add it to the list
			GameObject spawnedMagnetar = PhotonNetwork.InstantiateSceneObject (magnetarSpawn.name, target, Quaternion.identity, 0, null);
			spawnedMagnetar.transform.SetParent (container.transform);
			Rigidbody2D myRigidBody;
			myRigidBody = spawnedMagnetar.GetComponentInChildren<Rigidbody2D>();
			myRigidBody.AddTorque (Random.Range (-100, 100));
			int RandAngle = Random.Range (0, 360);
			myRigidBody.AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
			spawnedMagnetars.Add (spawnedMagnetar);

		} else {

			// generate random number and see if the item in the list has been destroyed -> if so, replace it by spawning a star
			int index = Random.Range (0, numMagnetarSpawn);
			if (spawnedMagnetars [index] == null) {

				spawnedMagnetars.Remove (spawnedMagnetars [index]);
				int x = Random.Range (-maxX, maxX);
				int y = Random.Range (-maxY, maxY);

				Vector3 target = new Vector3 (x, y, 0);

				if (testSpawn) {
					target = testLocation;
				}

				GameObject spawnedMagnetar = PhotonNetwork.InstantiateSceneObject (magnetarSpawn.name, target, Quaternion.identity, 0, null);
				spawnedMagnetar.transform.SetParent (container.transform);
				Rigidbody2D myRigidBody;
				myRigidBody = spawnedMagnetar.GetComponentInChildren<Rigidbody2D>();
				myRigidBody.AddTorque (Random.Range (-100, 100));
				int RandAngle = Random.Range (0, 360);
				myRigidBody.AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
				spawnedMagnetars.Add (spawnedMagnetar);


			}
		}
	}


	void GenerateComet () {
		// check to see if max spawn is reached
		if (spawnedComets.Count < numCometSpawn) {

			// generate random spawn location
			int x = Random.Range (-maxX, maxX);
			int y = Random.Range (-maxY, maxY);
			Vector3 target = new Vector3 (x, y, 0);

			if (testSpawn) {
				target = testLocation;
			}

			// spawn star and give it random rotation, torque, and force, then add it to the list
			GameObject spawnedComet = PhotonNetwork.InstantiateSceneObject (cometSpawn.name, target, Quaternion.identity, 0, null);
			spawnedComet.transform.SetParent (container.transform);
			Rigidbody2D myRigidBody;
			myRigidBody = spawnedComet.GetComponentInChildren<Rigidbody2D>();
			myRigidBody.AddTorque (Random.Range (-100, 100));
			int RandAngle = Random.Range (0, 360);
			myRigidBody.AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
			spawnedComets.Add (spawnedComet);

		} else {

			// generate random number and see if the item in the list has been destroyed -> if so, replace it by spawning a star
			int index = Random.Range (0, numCometSpawn);
			if (spawnedComets [index] == null) {

				spawnedComets.Remove (spawnedComets [index]);
				int x = Random.Range (-maxX, maxX);
				int y = Random.Range (-maxY, maxY);

				Vector3 target = new Vector3 (x, y, 0);

				if (testSpawn) {
					target = testLocation;
				}

				GameObject spawnedComet = PhotonNetwork.InstantiateSceneObject (cometSpawn.name, target, Quaternion.identity, 0, null);
				spawnedComet.transform.SetParent (container.transform);
				Rigidbody2D myRigidBody;
				myRigidBody = spawnedComet.GetComponentInChildren<Rigidbody2D>();
				myRigidBody.AddTorque (Random.Range (-100, 100));
				int RandAngle = Random.Range (0, 360);
				myRigidBody.AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
				spawnedComets.Add (spawnedComet);


			}
		}
	}
		
	void GenerateRogue () {
		// check to see if max spawn is reached
		if (spawnedRogues.Count < numRogueSpawn) {

			// generate random spawn location
			int x = Random.Range (-maxX, maxX);
			int y = Random.Range (-maxY, maxY);
			Vector3 target = new Vector3 (x, y, 0);

			if (testSpawn) {
				target = testLocation;
			}

			// spawn star and give it random rotation, torque, and force, then add it to the list
			GameObject spawnedRogue = PhotonNetwork.InstantiateSceneObject (rogueSpawn.name, target, Quaternion.identity, 0, null);
			spawnedRogue.transform.SetParent (container.transform);
			Rigidbody2D myRigidBody;
			myRigidBody = spawnedRogue.GetComponentInChildren<Rigidbody2D>();
			myRigidBody.AddTorque (Random.Range (-100, 100));
			int RandAngle = Random.Range (0, 360);
			myRigidBody.AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
			spawnedRogues.Add (spawnedRogue);

		} else {

			// generate random number and see if the item in the list has been destroyed -> if so, replace it by spawning a star
			int index = Random.Range (0, numRogueSpawn);
			if (spawnedRogues [index] == null) {

				spawnedRogues.Remove (spawnedRogues [index]);
				int x = Random.Range (-maxX, maxX);
				int y = Random.Range (-maxY, maxY);

				Vector3 target = new Vector3 (x, y, 0);

				if (testSpawn) {
					target = testLocation;
				}

				GameObject spawnedRogue = PhotonNetwork.InstantiateSceneObject (rogueSpawn.name, target, Quaternion.identity, 0, null);
				spawnedRogue.transform.SetParent (container.transform);
				Rigidbody2D myRigidBody;
				myRigidBody = spawnedRogue.GetComponentInChildren<Rigidbody2D>();
				myRigidBody.AddTorque (Random.Range (-100, 100));
				int RandAngle = Random.Range (0, 360);
				myRigidBody.AddForce (new Vector2 (Mathf.Sin (RandAngle * Mathf.Deg2Rad), -Mathf.Cos (RandAngle * Mathf.Deg2Rad)) * Random.Range (10, 200));
				spawnedRogues.Add (spawnedRogue);


			}
		}
	}
}
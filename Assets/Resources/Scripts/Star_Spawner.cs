using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star_Spawner : MonoBehaviour {

	public GameObject starSpawn;
	private float speed  = 0.5f;

	private List<GameObject> spawnedStuff = new List<GameObject> (100);

	// Use this for initialization
	void Start () {

		spawnedStuff = new List<GameObject>{ null, null, null, null, null, null, null, null, null, null,
			null, null, null, null, null, null, null, null, null, null,
			null, null, null, null, null, null, null, null, null, null,
			null, null, null, null, null, null, null, null, null, null,
			null, null, null, null, null, null, null, null, null, null,
			null, null, null, null, null, null, null, null, null, null,
			null, null, null, null, null, null, null, null, null, null,
			null, null, null, null, null, null, null, null, null, null,
			null, null, null, null, null, null, null, null, null, null,
			null, null, null, null, null, null, null, null, null, null};

		InvokeRepeating ("Generate", 0, speed);
	
	}
	


	void Generate (){

		int index = Random.Range (0, 99);

		if (spawnedStuff[index] == null){

			int x = Random.Range (0, Camera.main.pixelWidth);
			int y = Random.Range (0, Camera.main.pixelHeight);

			Vector3 target = Camera.main.ScreenToWorldPoint (new Vector3 (x, y, 0));
			target.z = 0;

			GameObject spawnedStar = Instantiate (starSpawn, target, Quaternion.identity) as GameObject;
			//spawnedStar.GetComponent<Rigidbody2D> ().MoveRotation (Random.Range (-180, 180));
			spawnedStar.GetComponent<Rigidbody2D> ().AddTorque (Random.Range (-100, 100));
			int RandAngle = Random.Range (0, 360);
			spawnedStar.GetComponent<Rigidbody2D> ().AddForce (new Vector2(Mathf.Sin (RandAngle * Mathf.Deg2Rad),  -Mathf.Cos (RandAngle * Mathf.Deg2Rad))*Random.Range (10, 200));
			spawnedStuff [index] = spawnedStar;

		
		}
			
	}
}

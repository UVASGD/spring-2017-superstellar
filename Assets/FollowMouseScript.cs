using UnityEngine;
using System.Collections;

public class FollowMouseScript : MonoBehaviour {
	public Vector3 UltimatePosition = new Vector3(0,0,0);
	float X, Y;
	
	
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		X = Input.mousePosition.x;
		Y = Input.mousePosition.y;
		Vector3 pos = Input.mousePosition;
		pos.z = transform.position.z - Camera.main.transform.position.z;
		transform.position = Camera.main.ScreenToWorldPoint(pos);
		
	}
}

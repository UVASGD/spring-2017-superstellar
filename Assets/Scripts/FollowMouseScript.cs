using UnityEngine;
using System.Collections;

public class FollowMouseScript : MonoBehaviour {

	public float smoothTime;
	private Vector3 velocity = Vector3.zero;
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		followMouse ();
		rotate ();
	}

	void followMouse() {
		Vector3 pos = Input.mousePosition;
		pos.z = transform.position.z - Camera.main.transform.position.z;
		Vector3 target = Camera.main.ScreenToWorldPoint(pos);
		target.x = Mathf.Clamp (target.x, -500.0f, 500.0f);
		target.y = Mathf.Clamp (target.y, -500.0f, 500.0f);
		transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
	}

	void rotate() {
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = Input.mousePosition - pos;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.back); 
	}

}

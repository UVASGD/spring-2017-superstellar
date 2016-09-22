using UnityEngine;
using System.Collections;

public class FollowMouseScript : MonoBehaviour {

	public float smoothTime; //sets lag for star behind mouse
	public GameObject bg; //determines bounds for map
	private Vector3 velocity = Vector3.zero;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		followMouse ();
		rotate ();
	}

	//makes star follow mouse (works fine)
	void followMouse() {
		Vector3 pos = Input.mousePosition;
		pos.z = transform.position.z - Camera.main.transform.position.z;
		Vector3 target = Camera.main.ScreenToWorldPoint(pos);
		target.x = Mathf.Clamp (target.x, -bg.transform.localScale.x/2.0f, bg.transform.localScale.x/2.0f);
		target.y = Mathf.Clamp (target.y, -bg.transform.localScale.y/2.0f, bg.transform.localScale.y/2.0f);
		transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
	}

	//rotates top of star to point towards mouse (hypothetically)
	void rotate() {
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = Input.mousePosition - pos;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
	}

}

using UnityEngine;

public class Controls: MonoBehaviour
{
	public GameObject projectile;

	private Vector3 top = Vector3.up;
	private Vector3 righttop = new Vector3(0.95105651629f,0.30901699437f,0);
	private Vector3 rightbot = new Vector3(0.58778525229f,-0.80901699437f,0);
	private Vector3 leftbot = new Vector3(-0.58778525229f,-0.80901699437f,0);
	private Vector3 lefttop = new Vector3(-0.95105651629f,0.30901699437f,0);


	void Update( )
	{
		int[] points = getOrientation (transform.rotation);

		if (Input.GetKeyDown (KeyCode.Alpha1))

			Shoot (points[0]);

		if( Input.GetKeyDown( KeyCode.Alpha2 ) )

			Shoot (points[1]);

		if (Input.GetKeyDown (KeyCode.Alpha3))

			Shoot (points[2]);

		if( Input.GetKeyDown( KeyCode.Alpha4 ) )

			Shoot (points[3]);

		if( Input.GetKeyDown( KeyCode.Alpha5 ) )

			Shoot (points[4]);
	}

	void Shoot(int point) {

		GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer> (); 
		SpriteRenderer sr = proj.GetComponent<SpriteRenderer> (); 
		Rigidbody rb = proj.GetComponent<Rigidbody> ();

		switch (point) {

			case 1:
				spr.sprite = Resources.Load<Sprite>("star_removed");
				sr.sprite = Resources.Load<Sprite>("star_top");
				rb.AddForce(top * 100f);
				break;
			case 2:
				spr.sprite = Resources.Load<Sprite>("star_removed_right_top");
				sr.sprite = Resources.Load<Sprite>("star_righttop");
				rb.AddForce(righttop * 100f);
				break;
			case 3:
				spr.sprite = Resources.Load<Sprite>("star_removed_right_bottom");
				sr.sprite = Resources.Load<Sprite>("star_rightbot");
				rb.AddForce(rightbot * 100f);
				break;
			case 4:
				spr.sprite = Resources.Load<Sprite>("star_removed_left_bottom");
				sr.sprite = Resources.Load<Sprite>("star_leftbot");
				rb.AddForce(leftbot * 100f);
				break;
			case 5:
				spr.sprite = Resources.Load<Sprite>("star_removed_left_top");
				sr.sprite = Resources.Load<Sprite>("star_lefttop");
				rb.AddForce(lefttop * 100f);
				break;
		}
		sr.enabled = true;
	}

	int[] getOrientation(Quaternion q) {

		int[] points = new int[5];
		float angle = q.eulerAngles.z;

		if (!(angle < Mathf.Clamp(angle,0.0f,72.0f) || angle > Mathf.Clamp(angle,0.0f,72.0f))) {
			points = new int[] {1,2,3,4,5};
		} else if (!(angle < Mathf.Clamp(angle,72.0f,144.0f) || angle > Mathf.Clamp(angle,72.0f,144.0f))) {
			points = new int[] {5,1,2,3,4};
		} else if (!(angle < Mathf.Clamp(angle,144.0f,216.0f) || angle > Mathf.Clamp(angle,144.0f,216.0f))) {
			points = new int[] {4,5,1,2,3};
		} else if (!(angle < Mathf.Clamp(angle,216.0f,288.0f) || angle > Mathf.Clamp(angle,216.0f,288.0f))) {
			points = new int[] {3,4,5,1,2};
		} else if (!(angle < Mathf.Clamp(angle,288.0f,360.0f) || angle > Mathf.Clamp(angle,288.0f,360.0f))) {
			points = new int[] {2,3,4,5,1};
		}

		return points;

	}
}
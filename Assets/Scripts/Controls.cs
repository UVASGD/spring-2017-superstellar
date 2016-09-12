using UnityEngine;

public class Controls: MonoBehaviour
{
	public GameObject projectile;
	public float velocity;


	//Direction Vectors for projectiles
	private Vector3 top = Vector3.up;
	private Vector3 righttop = new Vector3(0.95105651629f,0.30901699437f,0);
	private Vector3 rightbot = new Vector3(0.58778525229f,-0.80901699437f,0);
	private Vector3 leftbot = new Vector3(-0.58778525229f,-0.80901699437f,0);
	private Vector3 lefttop = new Vector3(-0.95105651629f,0.30901699437f,0);


	//Real-time update. Put conditions you always want to check for here
	void Update( )
	{
		int[] points = getOrientation (transform.rotation);

		if (Input.GetKeyDown (KeyCode.Alpha1))

			Shoot (points[0]); //fires "top" point

		if( Input.GetKeyDown( KeyCode.Alpha2 ) )

			Shoot (points[1]); //fires "right top" point

		if (Input.GetKeyDown (KeyCode.Alpha3))

			Shoot (points[2]); //fires "right bottom" point

		if( Input.GetKeyDown( KeyCode.Alpha4 ) )

			Shoot (points[3]); //fires "left bottom" point

		if( Input.GetKeyDown( KeyCode.Alpha5 ) )

			Shoot (points[4]); //fires "left top" point
	}


	//Creates projectile and shoots it in appropriate direction
	void Shoot(int point) {

		//clones existing projectile gameobject
		GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer> (); 
		SpriteRenderer sr = proj.GetComponent<SpriteRenderer> (); 
		Rigidbody2D rb = proj.GetComponent<Rigidbody2D> ();

		//switch statement determines which sprite to use for star and projectile
		//also adds force to make projectile move
		switch (point) {

			case 1:
				spr.sprite = Resources.Load<Sprite>("star_removed");
				sr.sprite = Resources.Load<Sprite>("star_top");
				rb.AddForce(top * velocity);
				break;
			case 2:
				spr.sprite = Resources.Load<Sprite>("star_removed_right_top");
				sr.sprite = Resources.Load<Sprite>("star_righttop");
				rb.AddForce(righttop * velocity);
				break;
			case 3:
				spr.sprite = Resources.Load<Sprite>("star_removed_right_bottom");
				sr.sprite = Resources.Load<Sprite>("star_rightbot");
				rb.AddForce(rightbot * velocity);
				break;
			case 4:
				spr.sprite = Resources.Load<Sprite>("star_removed_left_bottom");
				sr.sprite = Resources.Load<Sprite>("star_leftbot");
				rb.AddForce(leftbot * velocity);
				break;
			case 5:
				spr.sprite = Resources.Load<Sprite>("star_removed_left_top");
				sr.sprite = Resources.Load<Sprite>("star_lefttop");
				rb.AddForce(lefttop * velocity);
				break;
		}
		sr.enabled = true; //enable sprite render, projectile shows up
	}

	//Decides orientation of rotating star
	int[] getOrientation(Quaternion q) {

		int[] points = new int[5];
		float angle = q.eulerAngles.z;

		//Top point rotates to right 72 degrees. Increments that every if statement
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

		//return correct order of direction vectors
		return points;

	}
}
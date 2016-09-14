using UnityEngine;

public class Controls: MonoBehaviour
{
	public GameObject projectile;
	public float lifetime = 2.0f;

	
	//Direction Vectors for projectiles
	private Vector2 top = Vector2.up;
	private Vector2 righttop = new Vector2(0.95105651629f,0.30901699437f);
	private Vector2 rightbot = new Vector2(0.58778525229f,-0.80901699437f);
	private Vector2 leftbot = new Vector2(-0.58778525229f,-0.80901699437f);
	private Vector2 lefttop = new Vector2(-0.95105651629f,0.30901699437f);
	
	
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
			spr.sprite = Resources.Load<Sprite>("red");
			sr.sprite = Resources.Load<Sprite>("redonly");
			rb.AddForce(top * 100.0f);
			break;
		case 2:
			spr.sprite = Resources.Load<Sprite>("yellow");
			sr.sprite = Resources.Load<Sprite>("yelonly");
			rb.AddForce(righttop * 100.0f);
			break;
		case 3:
			spr.sprite = Resources.Load<Sprite>("green");
			sr.sprite = Resources.Load<Sprite>("gonly");
			rb.AddForce(rightbot * 100.0f);
			break;
		case 4:
			spr.sprite = Resources.Load<Sprite>("missing1");
			sr.sprite = Resources.Load<Sprite>("bluonly");
			rb.AddForce(leftbot * 100.0f);
			break;
		case 5:
			spr.sprite = Resources.Load<Sprite>("purple");
			sr.sprite = Resources.Load<Sprite>("missing4");
			rb.AddForce(lefttop * 100.0f);
			break;
		}

		/**
		 * ADDING COLLIDER INTERFERES WITH PROJECTILE MOTION
		*/

//		PolygonCollider2D pc = proj.AddComponent<PolygonCollider2D> ();
//		pc.density = 0;


		sr.enabled = true; //enable sprite render, projectile shows up
		Destroy(proj, lifetime);

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
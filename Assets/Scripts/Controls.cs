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
		getOrientation(transform.rotation);
		
		if (Input.GetKeyDown (KeyCode.Alpha1))
			
			Shoot (1); //fires "top" point
		
		if( Input.GetKeyDown( KeyCode.Alpha2 ) )
			
			Shoot (2); //fires "right top" point
		
		if (Input.GetKeyDown (KeyCode.Alpha3))
			
			Shoot (3); //fires "right bottom" point
		
		if( Input.GetKeyDown( KeyCode.Alpha4 ) )
			
			Shoot (4); //fires "left bottom" point
		
		if( Input.GetKeyDown( KeyCode.Alpha5 ) )
			
			Shoot (5); //fires "left top" point
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
	void getOrientation(Quaternion q) {
		
		float angle = q.eulerAngles.z;
	
		float topAngle = (Mathf.Atan2 (0,1) * Mathf.Rad2Deg) + angle;
		float righttopAngle = (Mathf.Atan2 (0.95105651629f,0.30901699437f) * Mathf.Rad2Deg) + angle;
		float rightbotAngle = (Mathf.Atan2 (0.58778525229f,-0.80901699437f) * Mathf.Rad2Deg) + angle;
		float leftbotAngle = (Mathf.Atan2 (-0.58778525229f,-0.80901699437f) * Mathf.Rad2Deg) + angle;
		float lefttopAngle = (Mathf.Atan2 (-0.95105651629f,0.30901699437f) * Mathf.Rad2Deg) + angle;

		top.x = -(Mathf.Sin(topAngle * Mathf.Deg2Rad));
		top.y = (Mathf.Cos(topAngle * Mathf.Deg2Rad));
		righttop.x = -(Mathf.Sin(righttopAngle * Mathf.Deg2Rad));
		righttop.y = (Mathf.Cos(righttopAngle * Mathf.Deg2Rad));
		rightbot.x = -(Mathf.Sin(rightbotAngle * Mathf.Deg2Rad));
		rightbot.y = (Mathf.Cos(rightbotAngle * Mathf.Deg2Rad));
		leftbot.x = -(Mathf.Sin(leftbotAngle * Mathf.Deg2Rad));
		leftbot.y = (Mathf.Cos(leftbotAngle * Mathf.Deg2Rad));
		lefttop.x = -(Mathf.Sin(lefttopAngle * Mathf.Deg2Rad));
		lefttop.y = (Mathf.Cos(lefttopAngle * Mathf.Deg2Rad));
	}
}
using UnityEngine;

public class Controls: MonoBehaviour
{
	public GameObject projectile;
	public float lifetime = 2.0f;
	public float projForce = 10000.0f;


	private static PhotonView ScenePhotonView;

	//Direction Vectors for projectiles
	private Vector2 top = Vector2.up;
	private Vector2 righttop = new Vector2(0.95105651629f,0.30901699437f);
	private Vector2 rightbot = new Vector2(0.58778525229f,-0.80901699437f);
	private Vector2 leftbot = new Vector2(-0.58778525229f,-0.80901699437f);
	private Vector2 lefttop = new Vector2(-0.95105651629f,0.30901699437f);

	private float topAngle = (Mathf.Atan2 (0,1) * Mathf.Rad2Deg);
	private float righttopAngle = (Mathf.Atan2 (0.95105651629f, 0.30901699437f) * Mathf.Rad2Deg);
	private float rightbotAngle = (Mathf.Atan2 (0.58778525229f,-0.80901699437f) * Mathf.Rad2Deg);
	private float leftbotAngle = (Mathf.Atan2 (-0.58778525229f,-0.80901699437f) * Mathf.Rad2Deg);
	private float lefttopAngle = (Mathf.Atan2 (-0.95105651629f,0.30901699437f) * Mathf.Rad2Deg);

	
	//Real-time update. Put conditions you always want to check for here
	void Update( )
	{

		ScenePhotonView = this.GetComponent<PhotonView>();

		getOrientation(transform.rotation);
		
		if (Input.GetKeyDown (KeyCode.Alpha1))
			
			Shot (1); //fires "top" point
		
		if( Input.GetKeyDown( KeyCode.Alpha2 ) )
			
			Shot (2); //fires "right top" point
		
		if (Input.GetKeyDown (KeyCode.Alpha3))
			
			Shot (3); //fires "right bottom" point
		
		if( Input.GetKeyDown( KeyCode.Alpha4 ) )
			
			Shot (4); //fires "left bottom" point
		
		if( Input.GetKeyDown( KeyCode.Alpha5 ) )
			
			Shot (5); //fires "left top" point
	}


	void Shot(int point)
	{
		Debug.Log("Shoot: " + point);
		ScenePhotonView.RPC("Shoot", PhotonTargets.All, point);
	}

	[PunRPC]
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
			spr.sprite = Resources.Load<Sprite> ("Colorful/missingred");
			sr.sprite = Resources.Load<Sprite> ("Colorful/redonly");
			rb.AddForce (top * projForce);
			rb.MoveRotation (topAngle - 90);
			break;
		case 2:
			spr.sprite = Resources.Load<Sprite>("Colorful/missingyellow");
			sr.sprite = Resources.Load<Sprite>("Colorful/yelonly");
			rb.AddForce(righttop * projForce);
			rb.MoveRotation (topAngle - 90 - 72);
			break;
		case 3:
			spr.sprite = Resources.Load<Sprite>("Colorful/missinggreen");
			sr.sprite = Resources.Load<Sprite>("Colorful/gonly");
			rb.AddForce(rightbot * projForce);
			rb.MoveRotation (topAngle - 90 - 144);
			break;
		case 4:
			spr.sprite = Resources.Load<Sprite>("Colorful/missingblue");
			sr.sprite = Resources.Load<Sprite>("Colorful/bluonly");
			rb.AddForce(leftbot * projForce);
			rb.MoveRotation (topAngle - 90 - 216);
			break;
		case 5:
			spr.sprite = Resources.Load<Sprite>("Colorful/missingpurple");
			sr.sprite = Resources.Load<Sprite>("Colorful/purponly");
			rb.AddForce(lefttop * projForce);
			rb.MoveRotation (topAngle - 90 - 288);
			break;
		}

		PolygonCollider2D pc = proj.AddComponent<PolygonCollider2D> ();
		pc.density = 0;
		pc.isTrigger = true;

		sr.enabled = true; //enable sprite render, projectile shows up
		Destroy(proj, lifetime);

	}
	
	//Decides orientation of rotating star
	void getOrientation(Quaternion q) {
		
		float angle = q.eulerAngles.z;
	
		topAngle = (Mathf.Atan2 (0,1) * Mathf.Rad2Deg) + angle + 90;
		righttopAngle = topAngle - 72 + 180;
		rightbotAngle = topAngle - 144 + 180;
		leftbotAngle = topAngle - 216 + 180;
		lefttopAngle = topAngle - 288 + 180;

		top.y = -(Mathf.Cos(topAngle * Mathf.Deg2Rad));
		top.x = (Mathf.Sin(topAngle * Mathf.Deg2Rad));
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

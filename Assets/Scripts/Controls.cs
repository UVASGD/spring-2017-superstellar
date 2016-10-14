using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls: MonoBehaviour
{
	public GameObject projectile;
	public float lifetime;
	public float projForce;

	public float starPointNum;
//	public List<GameObject> starpoints = new List<GameObject>();
	public float reloadTime;
	private List<SpriteRenderer> spri = new List<SpriteRenderer>();
	private List<int> canShoot = new List<int> (new int[12]);
	private List<int> autoShoot = new List<int> (new int[12]);
	public bool autoShootAll;
	
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


	void Start() 
	{
		for (int i = 0; i < starPointNum; i++) {
			canShoot [i] = 1;
//			if (autoShootAll)
//				autoShoot [i] = 1;
		}
	}
	//Real-time update. Put conditions you always want to check for here
	void Update( )
	{
		Debug.Log (autoShoot [0]);

		getOrientation(transform.rotation);

		if (Input.GetKeyDown (KeyCode.F))
		{
			if (autoShootAll)
				autoShootAll = false;
			else {
				autoShootAll = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha1) && Input.GetKeyDown (KeyCode.E))
		{
			if (autoShoot [0] == 1)
				autoShoot [0] = 0;
			else if (autoShoot [0] == 0){
				autoShoot [0] = 1;
			}
		}


		if ((Input.GetKeyDown (KeyCode.Alpha1) || autoShoot [0] == 1 || autoShootAll) && canShoot [0] == 1)

			Shoot (1); //fires "top" point

		if( (Input.GetKeyDown( KeyCode.Alpha2 ) || autoShootAll) && canShoot [1] == 1)

			Shoot (2); //fires "right top" point

		if ((Input.GetKeyDown (KeyCode.Alpha3) || autoShootAll) && canShoot [2] == 1)

			Shoot (3); //fires "right bottom" point

		if( (Input.GetKeyDown( KeyCode.Alpha4 ) || autoShootAll) && canShoot [3] == 1)

			Shoot (4); //fires "left bottom" point

		if( (Input.GetKeyDown( KeyCode.Alpha5 ) || autoShootAll) && canShoot [4] == 1)

			Shoot (5); //fires "left top" point
	
	}
	
	
	//Creates projectile and shoots it in appropriate direction
	void Shoot(int point) {
		
		//clones existing projectile gameobject
		GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer> (); 
		SpriteRenderer sr = proj.GetComponent<SpriteRenderer> (); 
		Rigidbody2D rb = proj.GetComponent<Rigidbody2D> ();

		spri.Add(spr);
		canShoot [point - 1] = 0;

		//switch statement determines which sprite to use for star and projectile
		//also adds force to make projectile move
		switch (point) {
			
		case 1:
			spr.sprite = Resources.Load<Sprite> ("missingred");
			sr.sprite = Resources.Load<Sprite> ("redonly");
			rb.AddForce (top * projForce);
			rb.MoveRotation (topAngle - 90);
			break;
		case 2:
			spr.sprite = Resources.Load<Sprite>("missingyellow");
			sr.sprite = Resources.Load<Sprite>("yelonly");
			rb.AddForce(righttop * projForce);
			rb.MoveRotation (topAngle - 90 - 72);
			break;
		case 3:
			spr.sprite = Resources.Load<Sprite>("missinggreen");
			sr.sprite = Resources.Load<Sprite>("gonly");
			rb.AddForce(rightbot * projForce);
			rb.MoveRotation (topAngle - 90 - 144);
			break;
		case 4:
			spr.sprite = Resources.Load<Sprite>("missingblue");
			sr.sprite = Resources.Load<Sprite>("bluonly");
			rb.AddForce(leftbot * projForce);
			rb.MoveRotation (topAngle - 90 - 216);
			break;
		case 5:
			spr.sprite = Resources.Load<Sprite>("missingpurple");
			sr.sprite = Resources.Load<Sprite>("purponly");
			rb.AddForce(lefttop * projForce);
			rb.MoveRotation (topAngle - 90 - 288);
			break;
		}

		PolygonCollider2D pc = proj.AddComponent<PolygonCollider2D> ();
		pc.density = 0;
		pc.isTrigger = true;

		sr.enabled = true; //enable sprite render, projectile shows up
		StartCoroutine(reload(spr,reloadTime, point - 1));
		Destroy(proj, lifetime);

	}

	IEnumerator reload(SpriteRenderer sprIndex, float delayTime, int strPt)
	{
		yield return new WaitForSeconds (delayTime);
		spri [spri.FindIndex (d=>d == sprIndex)].sprite = Resources.Load<Sprite> ("colorful");
		spri.Remove (sprIndex);
		canShoot [strPt] = 1;
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
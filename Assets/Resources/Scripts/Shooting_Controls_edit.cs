using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooting_Controls_edit: MonoBehaviour
{
	public GameObject projectile;
	public GameObject starPointSprite;
	private float lifetime = 2.0f;
	private float projForce = 500.0f;
	private int starPointNum = 5;
	private List<GameObject> starpoints = new List<GameObject>();
	private float reloadTime = 2.0f;
	private List<SpriteRenderer> spri = new List<SpriteRenderer>();
	private List<int> canShoot = new List<int> (12);
	private List<int> autoShoot = new List<int> (12);
	private bool autoShootAll = false;
	private List<int> shootOnMouse = new List<int> (12);
	
	//Direction Vectors for projectiles
	private List<Vector2> pointVectList = new List<Vector2>(12); 
	private List<float> pointAngles = new List<float>(12);
	private List<float> pointAngles2 = new List<float>(12);


	void Start() {
		canShoot = new List<int>{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
		autoShoot = new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		shootOnMouse = new List<int>{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
		resetShooting (transform.rotation, starPointNum);
	}



	//Real-time update. Put conditions you always want to check for here
	void Update( )
	{
		redrawStar(transform.rotation, starPointNum);

		List<KeyCode> shootKeys = new List<KeyCode> (12);
		shootKeys = new List<KeyCode>{KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
			KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, KeyCode.Minus, KeyCode.Equals};


		if (Input.GetKeyDown (KeyCode.F))
		{
			if (autoShootAll)
				autoShootAll = false;
			else {
				autoShootAll = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.Y) && starPointNum > 1)
		{
			
			starPointNum = starPointNum - 1;
			resetShooting (transform.rotation, starPointNum);
		}

		if (Input.GetKeyDown (KeyCode.U) && starPointNum < 12)
		{
			
			starPointNum = starPointNum + 1;
			resetShooting (transform.rotation, starPointNum);
		}



		for (int i = 0; i < 12; i++) {
			if (starPointNum > i) {

				if (Input.GetKeyDown (shootKeys [i]) && Input.GetKey (KeyCode.E)) {
					if (autoShoot [i] == 0)
						autoShoot [i] = 1;
					else if (autoShoot [i] == 1) {
						autoShoot [i] = 0;
					}
				}
				if (Input.GetKeyDown (shootKeys [i]) && Input.GetKey (KeyCode.Q)) {
					if (shootOnMouse [i] == 0)
						shootOnMouse [i] = 1;
					else if (shootOnMouse [i] == 1) {
						shootOnMouse [i] = 0;
					}
				}

				if (((Input.GetKeyDown (shootKeys [i]) || autoShoot [i] == 1 || autoShootAll || Input.GetKeyDown(KeyCode.R))||((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && shootOnMouse [i] == 1)) && canShoot [i] == 1)
					Shoot (i + 1);
			} else {
				canShoot [i] = -1;
				autoShoot [i] = -1;
				shootOnMouse [i] = -1;
			}

		}



		/*if ((Input.GetKeyDown (KeyCode.Alpha1) || autoShoot [0] == 1 || autoShootAll) && canShoot [0] == 1)
			
			Shoot (1); //fires "top" point
		
		if( (Input.GetKeyDown( KeyCode.Alpha2 ) || autoShootAll) && canShoot [1] == 1)
			
			Shoot (2); //fires "right top" point
		
		if ((Input.GetKeyDown (KeyCode.Alpha3) || autoShootAll) && canShoot [2] == 1)
			
			Shoot (3); //fires "right bottom" point
		
		if( (Input.GetKeyDown( KeyCode.Alpha4 ) || autoShootAll) && canShoot [3] == 1)
			
			Shoot (4); //fires "left bottom" point
		
		if( (Input.GetKeyDown( KeyCode.Alpha5 ) || autoShootAll) && canShoot [4] == 1)
			
			Shoot (5); //fires "left top" point
			*/
	}
	
	
	//Creates projectile and shoots it in appropriate direction
	void Shoot(int point) {
		
		//clones existing projectile gameobject
		GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		SpriteRenderer spr = starpoints [point - 1].GetComponent<SpriteRenderer> (); 
		SpriteRenderer sr = proj.GetComponent<SpriteRenderer> (); 
		Rigidbody2D rb = proj.GetComponent<Rigidbody2D> ();
		spri.Add(spr);
		canShoot [point - 1] = 0;
		
		//switch statement determines which sprite to use for star and projectile
		//also adds force to make projectile move
		switch (point) {
			
		case 1:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[0] + 90);
			sr.sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce (pointVectList[0]*projForce);
			break;
		case 2:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[1] + 90);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[1]*projForce);
			break;
		case 3:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[2] + 90);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[2]*projForce);
			break;
		case 4:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[3] + 90);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[3]*projForce);
			break;
		case 5:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[4] + 90);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[4]*projForce);
			break;
		case 6:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[5] + 90);
			sr.sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce (pointVectList[5]*projForce);
			break;
		case 7:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[6] + 90);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[6]*projForce);
			break;
		case 8:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[7] + 90);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[7]*projForce);
			break;
		case 9:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[8] + 90);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[8]*projForce);
			break;
		case 10:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[9] + 90);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[9]*projForce);
			break;
		case 11:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[10] + 90);
			sr.sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce (pointVectList[10]*projForce);
			break;
		case 12:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[11] + 90);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[11]*projForce);
			break;
		
		}

		/**
		 * ADDING COLLIDER INTERFERES WITH PROJECTILE MOTION
		*/

//		PolygonCollider2D pc = proj.AddComponent<PolygonCollider2D> ();
//		pc.density = 0;


		sr.enabled = true; //enable sprite render, projectile shows up
		StartCoroutine(reload(spr,reloadTime, point - 1, starPointNum));
		Destroy(proj, lifetime);


	}

	IEnumerator reload(SpriteRenderer sprIndex, float delayTime, int strPt, int strPtN)
	{
		yield return new WaitForSeconds (delayTime);
		if (strPtN == starPointNum) {
			spri [spri.FindIndex (d => d == sprIndex)].sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White");
			spri.Remove (sprIndex);
			canShoot [strPt] = 1;
		}
	}

	void redrawStar(Quaternion q, int numPoints) {
			
			float angle = q.eulerAngles.z;

			float topAngle = angle + 90;
			pointAngles.Clear ();
			pointVectList.Clear ();
			pointAngles.Add(topAngle);
			for(int i = 1; i < starPointNum; i++)
			{
			pointAngles.Add(topAngle - i * (360 / numPoints));
			}

			for(int i = 0; i < starPointNum; i++)
			{
			pointVectList.Add(new Vector2(Mathf.Sin (pointAngles [i] * Mathf.Deg2Rad),  -Mathf.Cos (pointAngles [i] * Mathf.Deg2Rad)));
			}
		}

	void resetShooting(Quaternion q, int numPoints){
		autoShootAll = false;
		spri.Clear ();
		int oldPt = starpoints.Count;
		for (int i = 0; i < oldPt; i++) {
			Destroy (starpoints [i]);
		}
		float angle2 = q.eulerAngles.z;
		float topAngle2 = angle2 + 90;
		starpoints.Clear ();
		pointAngles2.Clear ();
		pointAngles2.Add(topAngle2);
		for(int i = 1; i < numPoints; i++)
		{
			pointAngles2.Add(topAngle2 - i * (360 / numPoints));
		}
		for(int i = 0; i < numPoints; i++)
		{
			GameObject newPt = Instantiate(starPointSprite, transform.position, Quaternion.identity) as GameObject;
			newPt.transform.parent = transform;
			//newPt.transform.localScale = transform.localScale;
			Rigidbody2D rbs = newPt.GetComponent<Rigidbody2D> ();
			rbs.MoveRotation (pointAngles2 [i] + 90);
			starpoints.Add (newPt);
			canShoot [i] = 1;
			autoShoot [i] = 0;
			shootOnMouse [i] = 1;
		}
	}
	
	//Decides orientation of rotating star
	/*void getOrientation(Quaternion q) {
		
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
	}*/
}
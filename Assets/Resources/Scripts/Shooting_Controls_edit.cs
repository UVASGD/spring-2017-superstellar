using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooting_Controls_edit: MonoBehaviour
{
	//sprites -> projectile and non-projected point
	public GameObject projectile;
	public GameObject starPointSprite;

	//variables
	private float lifetime = 2.0f;
	private float projForce = 500.0f;
	private int starPointNum = 5;
	private int starMass = 0;
	private int maxPointHealth = 10;
	private int maxPointDam = 10;
	private int maxPlayerHealth = 100;
	private int maxPlayerDam = 100;
	private int playerRegen = 1;

	//holds the non-projected points
	private List<GameObject> starpoints = new List<GameObject>();

	//time to regen point
	private float reloadTime = 2.0f;

	//holds the non-projected points in limbo after projectiles shot
	private List<SpriteRenderer> spri = new List<SpriteRenderer>();

	//shooting conditions
	private List<int> canShoot = new List<int> (12);
	private List<int> autoShoot = new List<int> (12);
	private bool autoShootAll = false;
	private List<int> shootOnMouse = new List<int> (12);

	//class variables
	private List<Material> starMats = new List<Material> (13);
	private List<float> projSpeeds = new List<float> (13);
	private List<float> projLife = new List<float> (13);
	private List<float> projRegen = new List<float> (13);
	private List<int> starPtClass = new List<int> (13);
	public List<float> starSizes = new List<float> (13);
	private List<int> starPtHealth = new List<int> (13);
	private List<int> starPtDam = new List<int> (13);
	private List<int> starBodyHealth = new List<int> (13);
	private List<int> starBodyRegen = new List<int> (13);
	private List<int> starBodyDam = new List<int> (13);
		
	public int starType = 1;
	
	//Direction Vectors for projectiles
	private List<Vector2> pointVectList = new List<Vector2>(12); 
	private List<float> pointAngles = new List<float>(12);
	private List<float> pointAngles2 = new List<float>(12);



	void Start() {
		canShoot = new List<int>{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
		autoShoot = new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		shootOnMouse = new List<int>{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
		starMats = new List<Material>{ Resources.Load<Material> ("Materials/Normal_Star_Yellow"), Resources.Load<Material> ("Materials/Star_D_Red"),
			Resources.Load<Material> ("Materials/Star_G_Red"),Resources.Load<Material> ("Materials/Star_SG_Red"),Resources.Load<Material> ("Materials/Star_D_White"),
			Resources.Load<Material> ("Materials/Star_SG_Blue"),Resources.Load<Material> ("Materials/Star_S_Nova"),Resources.Load<Material> ("Materials/Star_HG_Blue"),
			Resources.Load<Material> ("Materials/Star_Neutron"),Resources.Load<Material> ("Materials/Star_H_Nova"),Resources.Load<Material> ("Materials/Star_B_Hole"),
			Resources.Load<Material> ("Materials/Star_Quasar"),Resources.Load<Material> ("Materials/Star_Pulsar")};
		projSpeeds = new List<float>{ 300f, 400f, 400f, 500f, 500f, 300f, 700f, 600f, 900f, 800f, 300f, 500f, 1000f };
		projLife = new List<float>{ 2f, 3f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };
		projRegen = new List<float>{ 5f, 0.75f, 1f, 0.75f, 0.5f, 1.25f, 0.5f, 0.5f, 0.5f, 0.75f, 0.75f, 0.5f, 0.25f };
		starPtClass = new List<int>{ 5, 4, 6, 8, 4, 10, 6, 12, 3, 7, 11, 9, 2 };
		starPtHealth = new List<int>{ 10, 5, 20, 8, 5, 100, 6, 12, 3, 7, 11, 9, 2 };
		starSizes = new List<float>{ 1f, 0.95f, 1.22f, 1.58f, 0.87f, 2f, 1.12f, 2.25f, 0.77f, 1.18f, 0.89f, 0.79f, 0.71f };
		starPtDam = new List<int>{ 5, 4, 6, 8, 4, 10, 6, 12, 3, 7, 11, 9, 2 };
		starBodyHealth = new List<int>{ 5, 4, 6, 8, 4, 10, 6, 12, 3, 7, 11, 9, 2 };
		starBodyRegen = new List<int>{ 5, 4, 6, 8, 4, 10, 6, 12, 3, 7, 11, 9, 2 };
		starBodyDam = new List<int>{ 5, 4, 6, 8, 4, 10, 6, 12, 3, 7, 11, 9, 2 };
		upgradeStar (1);
	}



	//Real-time update. Put conditions you always want to check for here
	void Update( )
	{
		redrawStar(transform.rotation, starPointNum);

		healthRegen (playerRegen);

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

		if (Input.GetKeyDown (KeyCode.Y) && starType > 1)
		{
			
			//starPointNum = starPointNum - 1;
			starType = starType - 1;
			//resetShooting (transform.rotation, starPointNum);
			upgradeStar(starType);
		}

		if (Input.GetKeyDown (KeyCode.U) && starType < 13)
		{
			
			//starPointNum = starPointNum + 1;
			starType = starType + 1;
			//resetShooting (transform.rotation, starPointNum);
			upgradeStar(starType);
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
		proj.transform.localScale = new Vector3(starSizes [starType - 1]*0.6f,starSizes [starType - 1]*1f,starSizes [starType - 1]*0.5f);
		proj.GetComponent<Renderer> ().material = starMats [starType - 1];
		proj.GetComponent<Health_Management> ().Health = maxPointHealth;
		proj.GetComponent<CollisionHandler> ().damage_to_give = maxPointDam;
		SpriteRenderer spr = starpoints [point - 1].GetComponent<SpriteRenderer> (); 
		SpriteRenderer sr = proj.GetComponent<SpriteRenderer> (); 
		Rigidbody2D rb = proj.GetComponent<Rigidbody2D> ();
		spri.Add(spr);
		canShoot [point - 1] = 0;
		spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
		rb.MoveRotation (pointAngles[point - 1] + 90);
		sr.sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White_Lineat60");
		rb.AddForce (pointVectList[point - 1]*projForce);
		GetComponent<Rigidbody2D> ().AddForce (-pointVectList [point - 1] * projForce/10f);


		/**
		 * ADDING COLLIDER INTERFERES WITH PROJECTILE MOTION
		*/

//		PolygonCollider2D pc = proj.AddComponent<PolygonCollider2D> ();
//		pc.density = 0;


		sr.enabled = true; //enable sprite render, projectile shows up
		StartCoroutine(reload(starpoints [point - 1],spr,reloadTime, point - 1, starPointNum));
		Destroy(proj, lifetime);


	}

	IEnumerator reload(GameObject strPont,SpriteRenderer sprIndex, float delayTime, int strPt, int strPtN)
	{
		yield return new WaitForSeconds (delayTime);
		if (strPtN == starPointNum) {
			spri [spri.FindIndex (d => d == sprIndex)].sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White");
			spri.Remove (sprIndex);
			strPont.GetComponent<Health_Management> ().Health = maxPointHealth;
			strPont.GetComponent<CollisionHandler> ().damage_to_give = maxPointDam;
			canShoot [strPt] = 1;
			strPont.GetComponent<Collider2D>().enabled = true;
		}
	}

	public void destroyStarPoint(GameObject starIndex){
		if (canShoot [starpoints.FindIndex(d => d == starIndex)] == 1){
		starIndex.GetComponent<Collider2D> ().enabled = false;
		canShoot [starpoints.FindIndex(d => d == starIndex)] = 0;
		spri.Add (starIndex.GetComponent<SpriteRenderer> ());
		starIndex.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
		StartCoroutine(reload(starIndex,starIndex.GetComponent<SpriteRenderer>(),reloadTime, starpoints.FindIndex(d => d == starIndex), starPointNum));
	
		}
	}

	public void healthRegen(int starRegen){
		GetComponent<Health_Management> ().Health = Mathf.MoveTowards (GetComponent<Health_Management> ().Health, maxPlayerHealth, starRegen * Time.deltaTime);
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
			newPt.transform.localScale = new Vector3(starSizes [starType - 1]*0.6f,starSizes [starType - 1]*1f,starSizes [starType - 1]*0.5f);
			newPt.GetComponent<Renderer> ().material = starMats [starType - 1];

			//newPt.transform.localScale = transform.localScale;
			//Rigidbody2D rbs = newPt.GetComponent<Rigidbody2D> ();
			//rbs.MoveRotation (pointAngles2 [i] + 90);
			newPt.transform.RotateAround(transform.position,Vector3.forward, (pointAngles2 [i] + 90));
			newPt.transform.parent = transform;
			newPt.GetComponent<Health_Management> ().Health = maxPointHealth;
			newPt.GetComponent<CollisionHandler> ().damage_to_give = maxPointDam;
			starpoints.Add (newPt);
			canShoot [i] = 1;
			autoShoot [i] = 0;
			shootOnMouse [i] = 1;
		}
	}

	void upgradeStar(int starGrade){

		transform.localScale = new Vector3(starSizes [starGrade - 1]*0.5f,starSizes [starGrade - 1]*0.5f,starSizes [starGrade - 1]*0.5f);
		GetComponent<Renderer> ().material = starMats [starType - 1];
		starPointNum = starPtClass [starGrade - 1];
		maxPointHealth = starPtHealth [starGrade - 1];
		maxPointDam = starPtDam [starGrade - 1];
		maxPlayerHealth = starBodyHealth [starGrade - 1];
		maxPlayerDam = starBodyDam [starGrade - 1];
		playerRegen = starBodyRegen [starGrade - 1];
		GetComponent<Health_Management> ().Health = maxPlayerHealth;
		GetComponent<CollisionHandler> ().damage_to_give = maxPlayerDam;
		resetShooting (transform.rotation, starPointNum);
		lifetime = projLife [starGrade - 1];
		projForce = projSpeeds [starGrade - 1];
		reloadTime = projRegen [starGrade - 1];



	}

	public void AddMass(int points){
		starMass += points;
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
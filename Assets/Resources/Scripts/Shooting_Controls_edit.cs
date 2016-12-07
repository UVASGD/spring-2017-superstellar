﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooting_Controls_edit: Photon.MonoBehaviour
{

	private static PhotonView ScenePhotonView;

	//sprites -> projectile and non-projected point
	public GameObject projectile;
	public GameObject starPointSprite;

	//variables
	private float lifetime = 2.0f;
	// how long projectiles stay on screen

	private float projForce = 500.0f;
	// how much force the projectiles are given when shot

	private int starPointNum = 5;
	// how many points the star has

	public int starMass = 0;
	// the mass of the star (the score in this game)

	private int maxPointHealth = 10;
	// how much health the projectiles have

	private int maxPointDam = 10;
	// how much damage the projectiles give when they collide with something

	public int maxPlayerHealth = 100;
	// how much health the player has at full health

	private int maxPlayerDam = 100;
	// how much damage the player gives when they collide with something

	private float playerRegen = 1;
	// how quickly the player regenerates their health

	private float reloadTime = 2.0f;
	//time to regen point

	public string playerTag;



	private List<GameObject> starpoints = new List<GameObject>();
	//holds the non-projected points

	private List<SpriteRenderer> spri = new List<SpriteRenderer>();
	//holds the non-projected points' spriterenderer after projectiles shot (as a to-do list for the reload function)



	//shooting conditions
	private List<int> canShoot = new List<int> (12);
	// determines whether a starpoint can be shot

	private List<int> autoShoot = new List<int> (12);
	// determines whether an individual starpoint will automatically fire after regenerating

	private bool autoShootAll = false;
	// determines whether all starpoints will automatically fire all at once after they regenerate

	private List<int> shootOnMouse = new List<int> (12);
	// determines whether an individual starpoint will fire on mouse click or spacebar



	//class variables
	private List<Material> starMats = new List<Material> (13);
	// holds the materials of each star class

	private List<float> projSpeeds = new List<float> (13);
	// holds the values for the speed of shot starpoints for each star class

	private List<float> projLife = new List<float> (13);
	// holds the values for lifetime of shot starpoints for each star class

	private List<float> projRegen = new List<float> (13);
	// holds the values for how fast starpoints regenerate for each star class

	private List<int> starPtClass = new List<int> (13);
	// holds the values for the number of starpoints for each star class

	public List<float> starSizes = new List<float> (13);
	// holds the values for the localscale size for each star class

	private List<int> starPtHealth = new List<int> (13);
	// holds the values for the health of starpoints for each star class

	private List<int> starPtDam = new List<int> (13);
	// holds the values for the damage inflicted by starpoints for each star class

	private List<int> starBodyHealth = new List<int> (13);
	// holds the values for the max health of the player for each star class

	private List<float> starBodyRegen = new List<float> (13);
	// holds the values for the health regeneration of the player for each star class

	private List<int> starBodyDam = new List<int> (13);
	// holds the values for the damage inflicted by the player starbody for each star class
		
	public int starType = 1;
	// the class of the star


	//Direction Vectors for projectiles
	private List<Vector2> pointVectList = new List<Vector2>(12);
	// the direction vectors for projectiles

	private List<float> pointAngles = new List<float>(12);
	// the angles at which starpoints are shot

	private List<float> pointAngles2 = new List<float>(12);
	// the angles at which starpoints are regenerated

//	void OnEnable()
//	{
//		Debug.Log ("Shooting Controls");
//		if (this.photonView != null && !this.photonView.isMine) {
//			Debug.Log ("disabled controls : " + this.photonView.ownerId);
//			this.enabled = false;
//			return;
//		} else {
//			Debug.Log("I am player "+ this.photonView.ownerId);
//		}
//	}

	void Start() {
		Debug.Log (this.photonView.viewID);

		Debug.Log (this.name);
		canShoot = new List<int>{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
		autoShoot = new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		shootOnMouse = new List<int>{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };


		this.tag = this.GetComponent<PhotonView> ().viewID.ToString();
		playerTag = this.tag;

		starpoints = GetComponent<StarManager>().starpoints;
		starSizes = GetComponent<StarManager>().starSizes;

		// set class values
		starMats = new List<Material>{ Resources.Load<Material> ("Materials/Normal_Star_Yellow"), Resources.Load<Material> ("Materials/Star_D_Red"),
			Resources.Load<Material> ("Materials/Star_G_Red"),Resources.Load<Material> ("Materials/Star_SG_Red"),Resources.Load<Material> ("Materials/Star_D_White"),
			Resources.Load<Material> ("Materials/Star_SG_Blue"),Resources.Load<Material> ("Materials/Star_S_Nova"),Resources.Load<Material> ("Materials/Star_HG_Blue"),
			Resources.Load<Material> ("Materials/Star_Neutron"),Resources.Load<Material> ("Materials/Star_H_Nova"),Resources.Load<Material> ("Materials/Star_B_Hole"),
			Resources.Load<Material> ("Materials/Star_Quasar"),Resources.Load<Material> ("Materials/Star_Pulsar")};
		projSpeeds = new List<float>{ 300f, 400f, 500f, 600f, 500f, 600f, 700f, 800f, 800f, 600f, 600f, 500f, 700f };
		projLife = new List<float>{ 2f, 3f, 2f, 2f, 1.5f, 2f, 1f, 2f, 0.75f, 1f, 1.5f, 1.5f, 0.5f };
		projRegen = new List<float>{ 2f, 1.5f, 2f, 2.5f, 0.5f, 2f, 1f, 2.5f, 0.25f, 0.75f, 1.5f, 1f, 0.125f };
		starPtClass = new List<int>{ 5, 4, 6, 8, 4, 10, 6, 12, 3, 7, 11, 9, 2 };
		starPtHealth = new List<int>{ 10, 15, 20, 30, 20, 50, 20, 70, 10, 30, 20, 30, 5 };
		starSizes = new List<float>{ 1f, 0.95f, 1.22f, 1.58f, 0.87f, 2f, 1.12f, 2.25f, 0.77f, 1.18f, 0.89f, 0.79f, 0.71f };
		starPtDam = new List<int>{ 10, 8, 15, 30, 15, 40, 30, 60, 8, 40, 30, 40, 5 };
		starBodyHealth = new List<int>{ 100, 150, 200, 300, 100, 350, 75, 500, 100, 50, 150, 200, 50 };
		starBodyRegen = new List<float>{ 0.1f, 0.2f, 0.05f, 0.03f, 0.3f, 0.02f, 0.3f, 0.01f, 0.4f, 0.4f, 0.3f, 0.2f, 0.5f };
		starBodyDam = new List<int>{ 20, 15, 40, 60, 15, 70, 30, 100, 10, 30, 40, 50, 5 };

		Debug.Log ("SHOOTING CONTROLS");
		if (this.photonView != null && !this.photonView.isMine) {
			//			Debug.Log ("disabled controls : " + this.photonView.ownerId);
			this.enabled = false;
			return;
		} else {
			//			Debug.Log("I am player "+ this.photonView.ownerId);
		}
			
		ScenePhotonView = this.GetComponent<PhotonView>();
		// set initial shooting conditions

	}


	void Update( )
	{

		ScenePhotonView.RPC("redrawStar", PhotonTargets.All, transform.rotation, starPointNum);
		// calculate the directions to shoot projectiles at that instant
//		redrawStar(transform.rotation, starPointNum);

		ScenePhotonView.RPC("healthRegen", PhotonTargets.All, playerRegen);
		// regenerate player health
//		healthRegen (playerRegen);

		// set values for keys that can be used for shooting
		List<KeyCode> shootKeys = new List<KeyCode> (12);
		shootKeys = new List<KeyCode>{KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
			KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, KeyCode.Minus, KeyCode.Equals};

		// toggle autoshooting for all points
		if (Input.GetKeyDown (KeyCode.F))
		{
			if (autoShootAll)
				autoShootAll = false;
			else {
				autoShootAll = true;
			}
		}

		// check to see if stars can be shot
		for (int i = 0; i < 12; i++) {

			// checks to see if starpoints exist
			if (starPointNum > i) {

				// set autoshooting for individual starpoints
				if (Input.GetKeyDown (shootKeys [i]) && Input.GetKey (KeyCode.E)) {
					if (autoShoot [i] == 0)
						autoShoot [i] = 1;
					else if (autoShoot [i] == 1) {
						autoShoot [i] = 0;
					}
				}

				// set shooting by mouse/spacebar for individual starpoints
				if (Input.GetKeyDown (shootKeys [i]) && Input.GetKey (KeyCode.Q)) {
					if (shootOnMouse [i] == 0)
						shootOnMouse [i] = 1;
					else if (shootOnMouse [i] == 1) {
						shootOnMouse [i] = 0;
					}
				}

				// check conditions to see if starpoint can be shot
				if (((Input.GetKeyDown (shootKeys [i]) || autoShoot [i] == 1 || autoShootAll || Input.GetKey (KeyCode.R)) || ((Input.GetMouseButton (0) || Input.GetKey (KeyCode.Space)) && shootOnMouse [i] == 1)) && canShoot [i] == 1) {
//					Debug.Log ("starSizes: " + starSizes.Count);
					ScenePhotonView.RPC ("Shoot", PhotonTargets.All, i + 1);
				}
			} 

			// non-existant starpoints can't be shot
			else {
				canShoot [i] = -1;
				autoShoot [i] = -1;
				shootOnMouse [i] = -1;
			}

		}
			
	}



	[PunRPC]
	//Creates projectile and shoots it in appropriate direction
	void Shoot(int point) {


		Debug.Log (this.photonView.ownerId);

		//clones existing projectile gameobject
		GameObject proj = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;

		proj.tag = playerTag;

		// sets projectile size, material, health, damage, and accesses its rigidbody and spriterenderer
		Debug.Log ("starSizes: " + starSizes.Count);
		proj.transform.localScale = new Vector3(starSizes [starType - 1]*0.6f,starSizes [starType - 1]*1f,starSizes [starType - 1]*0.5f);
		proj.GetComponent<Renderer> ().material = starMats [starType - 1];
		proj.GetComponent<Health_Management> ().Health = maxPointHealth;
		proj.GetComponent<CollisionHandler> ().damage_to_give = maxPointDam;
		SpriteRenderer sr = proj.GetComponent<SpriteRenderer> ();
		Rigidbody2D rb = proj.GetComponent<Rigidbody2D> ();

		// designates which starpoints are being shot so that their sprite can be changed and they won't collide anymore
		SpriteRenderer spr = starpoints [point - 1].GetComponent<SpriteRenderer> (); 
		spri.Add(spr);
		spr.GetComponent<Collider2D> ().enabled = false;

		// makes it so that the projectile can't immediately shoot again
		canShoot [point - 1] = 0;

		// loads proper sprites -> projectile triangle sprite and empty "nub" sprite
		spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
		sr.sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White60");

		// shoots projectile at proper rotation, direction, and speed, and gives recoil to player
		proj.transform.RotateAround(transform.position,Vector3.forward, (pointAngles [point-1] + 90));
		Debug.Log (projForce);
		rb.AddForce (pointVectList[point - 1]*projForce);
		GetComponent<Rigidbody2D> ().AddForce (-pointVectList [point - 1] * projForce/10f);

		// tells starpoint regeneration function to run

//		ScenePhotonView.RPC("reload", PhotonTargets.All, starpoints [point - 1],spr,reloadTime, point - 1, starType);
		StartCoroutine(reload(starpoints [point - 1],spr,reloadTime, point - 1, starType));

		PolygonCollider2D pc = proj.AddComponent<PolygonCollider2D> ();
		pc.density = 0;
		pc.isTrigger = true;

		// destroys projectile
		Destroy(proj, lifetime);


	}


	[PunRPC]
	// regenerate starpoints after they were shot off or destroyed
	IEnumerator reload(GameObject strPont,SpriteRenderer sprIndex, float delayTime, int strPt, int strClassN)
	{
		yield return new WaitForSeconds (delayTime);

		// checks to make sure the star class is still the same
		if (strClassN == starType) {

			// reloads the un-shot starpoint into the proper spriterenderer, and then removes it from the to-do list of spriterenderers
			spri [spri.FindIndex (d => d == sprIndex)].sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White");
			spri.Remove (sprIndex);

			// gives the regenerated point max health and damage
			strPont.GetComponent<Health_Management> ().Health = maxPointHealth;
			strPont.GetComponent<CollisionHandler> ().damage_to_give = maxPointDam;

			// sets the starpoint as able to be shot and able to collide with objects
			canShoot [strPt] = 1;
			strPont.GetComponent<Collider2D>().enabled = true;
		}
	}

	[PunRPC]
	// kills un-shot starpoints when their health reaches 0
	public void destroyStarPoint(GameObject starIndex){

		// makes sure the starpoint still exists (ie it wasn't just shot)
		if (canShoot [starpoints.FindIndex(d => d == starIndex)] == 1){

			// turns off collider, disables shooting of that starpoint, adds the spriterenderer to the regen to-do list, and turns the sprite into the "nub"
			starIndex.GetComponent<Collider2D> ().enabled = false;
			canShoot [starpoints.FindIndex(d => d == starIndex)] = 0;
			spri.Add (starIndex.GetComponent<SpriteRenderer> ());
			starIndex.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");

			// tells the point to regenerate
			StartCoroutine(reload(starIndex,starIndex.GetComponent<SpriteRenderer>(),reloadTime, starpoints.FindIndex(d => d == starIndex), starType));
	
		}
	}

	[PunRPC]
	// tells the health manager to regenerate player health over time
	public void healthRegen(float starRegen){
		GetComponent<Health_Management> ().Health = Mathf.MoveTowards (GetComponent<Health_Management> ().Health, maxPlayerHealth, starRegen * Time.deltaTime);
	}

	[PunRPC]
	// calculates the angles and direction vectors for projectiles
	void redrawStar(Quaternion q, int numPoints) {
			
		float angle = q.eulerAngles.z;
		float topAngle = angle + 90;

		// clears the lists
		pointAngles.Clear ();
		pointVectList.Clear ();

		// adds values to the angle and direction vector lists
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
		

	[PunRPC]
	// gives points to the player
	public void AddMass(int points){
		starMass += points;
	}

}
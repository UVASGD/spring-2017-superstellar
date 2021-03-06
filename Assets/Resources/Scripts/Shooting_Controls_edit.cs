﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooting_Controls_edit: Photon.MonoBehaviour
{

	//VARIABLES

	private static PhotonView ScenePhotonView;

	//sprites -> projectile and non-projected point
	public GameObject projectile;
	public GameObject starPointSprite;

	//variables
	private float lifetime = 2.0f;
	private float projForce = 500.0f;
	private int starPointNum = 5;
	public int starMass = 0;
	private int maxPointHealth = 10;
	private int maxPointDam = 10;
	public int maxPlayerHealth = 100;
	private int maxPlayerDam = 100;
	private float playerRegen = 1;
	private float reloadTime = 2.0f; //time to regen point
	private float colReload = 0.1f;
	private int currentInd = 0;
	private int oldStarPointNum = 5;


	private List<GameObject> starpoints = new List<GameObject>(); //holds the non-projected points
	private List<SpriteRenderer> spri = new List<SpriteRenderer>(); //holds the non-projected points' spriterenderer after projectiles shot (as a to-do list for the reload function)


	//shooting conditions
	public List<int> canShoot = new List<int> (12); // determines whether a starpoint can be shot
	public List<int> autoShoot = new List<int> (12); // determines whether an individual starpoint will automatically fire after regenerating
	public bool autoShootAll = false; // determines whether all starpoints will automatically fire all at once after they regenerate
	public List<int> shootOnMouse = new List<int> (12); // determines whether an individual starpoint will fire on mouse click or spacebar
	public int preset; // current preset value
	public int presetMax; // maximum preset for current star type
	private int oldPreset; //previous preset value

	//class variables (Refer to Star Manager for details)
	private List<Material> starMats = new List<Material> (13);
	private List<float> projSpeeds = new List<float> (13);
	private List<float> projLife = new List<float> (13);
	private List<float> projRegen = new List<float> (13);
	private List<int> starPtClass = new List<int> (13);
	public List<float> starSizes = new List<float> (13);
	private List<int> starPtHealth = new List<int> (13);
	private List<int> starPtDam = new List<int> (13);
	private List<int> starBodyHealth = new List<int> (13);
	private List<float> starBodyRegen = new List<float> (13);
	private List<int> starBodyDam = new List<int> (13);
	public int starType;

	private Sprite highlightedSprite;


	//Direction Vectors for projectiles

	private List<Vector2> pointVectList = new List<Vector2>(12); // the direction vectors for projectiles
	private List<float> pointAngles = new List<float>(12); // the angles at which starpoints are shot
	//	private List<float> pointAngles2 = new List<float>(12); // the angles at which starpoints are regenerated






	//FUNCTIONS


	void OnEnable()
	{
		if (this.photonView != null && !this.photonView.isMine) {
			this.enabled = false;
			return;
		} 
		//		else {
		//			Debug.Log("I am player "+ this.photonView.ownerId);
		//		}
	}

	void Start() {

		autoShoot = new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		shootOnMouse = new List<int>{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

		starpoints = GetComponent<StarManager>().starpoints;
		starSizes = GetComponent<StarManager>().starSizes;

		ScenePhotonView = this.GetComponent<PhotonView>();

	}


	void Update( )
	{
		if (oldStarPointNum != starPointNum) {
			oldStarPointNum = starPointNum;
			currentInd = 0;
		} 

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			Debug.Log ("ayyy, you clicked one" + currentInd);
			shootOnMouse [currentInd] = 0;
			currentInd = currentInd + 1;
			Debug.Log ("starpointNum: " + starPointNum);
			if (currentInd >= starPointNum) {
				currentInd = 0;
			}
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			Debug.Log ("ayyy, you clicked two" + currentInd);
			shootOnMouse [currentInd] = 0;
			Debug.Log ("spricount: " + spri.Count);
			currentInd = currentInd - 1;
			Debug.Log ("starpointNum: " + starPointNum);
			if (currentInd < 0) {
				currentInd = starPointNum-1;
			}
		}
		starpoints = GetComponent<StarManager>().starpoints;
		starSizes = GetComponent<StarManager>().starSizes;
		starType = GetComponent<StarManager>().starType;
		canShoot = GetComponent<StarManager>().canShoot;
		starPointNum = GetComponent<StarManager>().starPointNum;
		lifetime = GetComponent<StarManager>().lifetime;
		projForce = GetComponent<StarManager>().projForce;
		reloadTime = GetComponent<StarManager>().reloadTime;
		starPtDam = GetComponent<StarManager>().starPtDam;
		starMats = GetComponent<StarManager> ().starMats;
		highlightedSprite = GetComponent<StarManager> ().highlightedSprite;

		ScenePhotonView.RPC("redrawStar", PhotonTargets.All, transform.rotation, starPointNum); // calculate the directions to shoot projectiles at that instant
		// redrawStar(transform.rotation, starPointNum);

		 ScenePhotonView.RPC("healthRegen", PhotonTargets.All, playerRegen); // regenerate player health
		// healthRegen (playerRegen);



		//POSSIBLY USED FOR MODE SWITCHING INSTEAD OF "R"

		// set values for keys that can be used for shooting
		// List<KeyCode> shootKeys = new List<KeyCode> (12);
		// shootKeys = new List<KeyCode>{KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
		// KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, KeyCode.Minus, KeyCode.Equals};

		// toggle autoshooting for all points
		if (Input.GetKeyDown (KeyCode.F))
		{
			if (autoShootAll) {
				autoShootAll = false;
				oldPreset = preset;
				preset = -1;
			} else {
				autoShootAll = true;
				preset = oldPreset;
			}
		}

		if (Input.GetKeyDown (KeyCode.E))
		{
			preset = -1;
			for (int i = 0; i < 12; i++) {
				if (starPointNum > i) {
					shootOnMouse [i] = 1;
					starpoints[i].GetComponent<SpriteRenderer> ().material=Resources.Load<Material>("Materials/Local_Player_Icon");
				} else {
					shootOnMouse [i] = 0;
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.R))
		{
			currentInd = 0;
			if (preset == -1) {
				preset = 1;
			} else {
				preset = preset + 1;
				if (preset > presetMax) {
					preset = 1;
				}
			}
		}

		// check to see if stars can be shot
		for (int i = 0; i < 12; i++) {

			// checks to see if starpoints exist
			if (i< starPointNum) {

				float currAngle = (360 / starPointNum) * i;
//				Debug.Log ("Hallo in case one thank" + i+"vs"+currentInd);

				switch (preset) {

				//mode where you shoot one point
				case 1:
					if (i == currentInd) {
						shootOnMouse [i] = 1;

						starpoints[i].GetComponent<SpriteRenderer> ().material=Resources.Load<Material>("Materials/Local_Player_Icon"); //change starpoint that can shoot color to white
//						starpoints[i].GetComponent<SpriteRenderer> ().sprite = highlightedSprite;
					} else {
						shootOnMouse [i] = 0;
						starpoints [i].GetComponent<SpriteRenderer> ().material = starMats [starType];
					}
					break;
				case 2:
					
					if (i == currentInd) {
						if (starPointNum < 5) {
							shootOnMouse [i] = 0;
							starpoints [i].GetComponent<SpriteRenderer> ().material = starMats [starType];
						} else {
							shootOnMouse [i] = 1;
							starpoints[i].GetComponent<SpriteRenderer> ().material=Resources.Load<Material>("Materials/Local_Player_Icon");
//							starpoints[i].GetComponent<SpriteRenderer> ().sprite = highlightedSprite;
						}
					} else {
						if (currAngle > 90 && currAngle < 270) {
							shootOnMouse [i] = 1;
							starpoints[i].GetComponent<SpriteRenderer> ().material=Resources.Load<Material>("Materials/Local_Player_Icon");
//							starpoints[i].GetComponent<SpriteRenderer> ().sprite = highlightedSprite;
						} else {
							shootOnMouse [i] = 0;
							starpoints [i].GetComponent<SpriteRenderer> ().material = starMats [starType];
						}
					}
					break;
				case 3:
					Debug.Log ("In mode 3");
					if (i == currentInd || i == Mathf.FloorToInt ((float)starPointNum / 2) || i == Mathf.CeilToInt ((float)starPointNum / 2)) {
						shootOnMouse [i] = 0;
						starpoints [i].GetComponent<SpriteRenderer> ().material = starMats [starType];
					} else {
						shootOnMouse [i] = 1;
						starpoints[i].GetComponent<SpriteRenderer> ().material=Resources.Load<Material>("Materials/Local_Player_Icon");
//						starpoints[i].GetComponent<SpriteRenderer> ().sprite = highlightedSprite;
					}
					break;
				case 4:
					if (currAngle > 90 && currAngle < 270 || i == currentInd) {
						shootOnMouse [i] = 0;
						starpoints [i].GetComponent<SpriteRenderer> ().material = starMats [starType];


					} else {
						shootOnMouse [i] = 1;
						starpoints[i].GetComponent<SpriteRenderer> ().material=Resources.Load<Material>("Materials/Local_Player_Icon");
//						starpoints[i].GetComponent<SpriteRenderer> ().sprite = highlightedSprite;

					}
					break;
				}


				// check conditions to see if starpoint can be shot
				if (((autoShoot [i] == 1 || autoShootAll) || ((Input.GetMouseButton (0) || Input.GetKey (KeyCode.Space)) && shootOnMouse [i] == 1)) && canShoot [i] == 1) {
					ScenePhotonView.RPC ("Shoot", PhotonTargets.All, i+1);
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

		//		Debug.Log(PlayerPrefs.GetString("PlayerName"));
		starPointNum = GetComponent<StarManager>().starPointNum;

		//clones existing projectile gameobject
		GameObject proj = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
		proj.GetComponent<Health_Management> ().viewID = this.photonView.viewID;
		// sets projectile size, material, health, damage, and accesses its rigidbody and spriterenderer
		starMats = GetComponent<StarManager> ().starMats;
		starType = GetComponent<StarManager> ().starType;
		starSizes = GetComponent<StarManager> ().starSizes;
		starpoints = GetComponent<StarManager> ().starpoints;
		canShoot = GetComponent<StarManager> ().canShoot;
		projForce = GetComponent<StarManager> ().projForce;
		pointAngles = GetComponent<StarManager> ().pointAngles;
		pointVectList = GetComponent<StarManager> ().pointVectList;
		starPtDam = GetComponent<StarManager> ().starPtDam;

		proj.transform.localScale = new Vector3(starSizes [starType]*0.6f,starSizes [starType]*1f,starSizes [starType]*0.5f);
		proj.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;

		proj.GetComponent<Health_Management> ().Health = maxPointHealth;
		proj.GetComponent<CollisionHandler> ().damage_to_give = starPtDam[starType];
		SpriteRenderer sr = proj.GetComponent<SpriteRenderer> ();
		Rigidbody2D rb = proj.GetComponent<Rigidbody2D> ();

		// designates which starpoints are being shot so that their sprite can be changed and they won't collide anymore
		SpriteRenderer spr = starpoints [point - 1].GetComponent<SpriteRenderer> (); 
		spri.Add(spr);
		spr.GetComponent<Collider2D> ().enabled = false;
		Collider2D projCol = proj.GetComponent<Collider2D> ();
		projCol.enabled = false;

		// makes it so that the projectile can't immediately shoot again
		canShoot [point - 1] = 0;

		// loads proper sprites -> projectile triangle sprite and empty "nub" sprite
		spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
		sr.sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White60");


		// CHECK THIS! PLAYER ANGLES ARE NOT RIGHT
		// shoots projectile at proper rotation, direction, and speed, and gives recoil to player
		proj.transform.RotateAround(transform.position,Vector3.forward, (pointAngles [point-1] + 90));
		rb.AddForce (pointVectList[point - 1]* GetComponent<StarManager> ().projForce);
		GetComponent<Rigidbody2D> ().AddForce (-pointVectList [point - 1] *  GetComponent<StarManager> ().projForce/10f);

		//sound effect
		if (GetComponent<StarManager> ().source) {
			GetComponent<StarManager> ().source.PlayOneShot (GetComponent<StarManager> ().shootSound, 0.75f);
		}
		// tells starpoint regeneration function to run
		StartCoroutine(reload(starpoints [point - 1],spr,reloadTime, point - 1, starType));
		StartCoroutine (collideEnable (proj, colReload, projCol));

		//PolygonCollider2D pc = proj.AddComponent<PolygonCollider2D> ();
		//pc.density = 0;
		//pc.isTrigger = true;

		// destroys projectile
		Destroy(proj, lifetime);

	}


	[PunRPC]
	// re-enable collider on shot starpoints since they initially collide
	IEnumerator collideEnable(GameObject projctl, float timeDelay, Collider2D colliderThing)
	{
		yield return new WaitForSeconds (timeDelay);
		if (colliderThing != null) {
			colliderThing.enabled = true;
		}
		//projctl.GetComponent<Renderer> ().material = Resources.Load<Material> ("Materials/Star_Pulsar");
	}


	[PunRPC]
	// regenerate starpoints after they were shot off or destroyed
	IEnumerator reload(GameObject strPont,SpriteRenderer sprIndex, float delayTime, int strPt, int strClassN)
	{
		yield return new WaitForSeconds (delayTime);
		//Debug.Log ("Delay Time: " + delayTime);

		// checks to make sure the star class is still the same
//		Debug.Log("starClassN" + strClassN);
//		Debug.Log("starType" + starType);
		if (strClassN == starType) {

			// reloads the un-shot starpoint into the proper spriterenderer, and then removes it from the to-do list of spriterenderers
			spri [spri.FindIndex (d => d == sprIndex)].sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White");
			spri.Remove (sprIndex);

			// gives the regenerated point max health and damage
			strPont.GetComponent<Health_Management> ().Health = maxPointHealth;
			strPont.GetComponent<CollisionHandler> ().damage_to_give = maxPointDam;

			// sets the starpoint as able to be shot and able to collide with objects
			canShoot [strPt] = 1;
			if (this.photonView.isMine) {
				strPont.GetComponent<Collider2D> ().enabled = true;
			}

		}
	}

	[PunRPC]
	// kills un-shot starpoints when their health reaches 0
	public void destroyStarPoint(GameObject starIndex){

		// makes sure the starpoint still exists (ie it wasn't just shot)
		Debug.Log("canShoot" + ((canShoot [starpoints.FindIndex(d => d == starIndex)]) == 1));
		if (canShoot [starpoints.FindIndex (d => d == starIndex)] == 1) {
			Debug.Log ("in method");
			if (canShoot [starpoints.FindIndex (d => d == starIndex)] == 1) {
				Debug.Log ("canshoot");
				// turns off collider, disables shooting of that starpoint, adds the spriterenderer to the regen to-do list, and turns the sprite into the "nub"
				starIndex.GetComponent<Collider2D> ().enabled = false;
				canShoot [starpoints.FindIndex (d => d == starIndex)] = 0;
				spri.Add (starIndex.GetComponent<SpriteRenderer> ());
				starIndex.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");

				// tells the point to regenerate
				StartCoroutine (reload (starIndex, starIndex.GetComponent<SpriteRenderer> (), reloadTime, starpoints.FindIndex (d => d == starIndex), starType));

			}
		}

	}

}
﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarManager: Photon.MonoBehaviour
{

	private static PhotonView ScenePhotonView;

	//sprites -> projectile and non-projected point
	public GameObject projectile;
	public GameObject starPointSprite;


	//variables

	public float lifetime = 2.0f; // how long projectiles stay on screen
	public float projForce = 500.0f; // how much force the projectiles are given when shot
	public int starPointNum = 5; // how many points the star has
	public int starMass = 0; // the mass of the star (the score in this game)
	public int maxPointHealth = 10; // how much health the projectiles have
	public int maxPointDam = 10; // how much damage the projectiles give when they collide with something
	public int maxPlayerHealth = 100; // how much health the player has at full health
	public int maxPlayerDam = 100; // how much damage the player gives when they collide with something
	public float playerRegen = 1; // how quickly the player regenerates their health
	public float reloadTime = 2.0f; //time to regen point
	public string playerTag;


	public List<GameObject> starpoints = new List<GameObject>(); //holds the non-projected points
	private List<SpriteRenderer> spri = new List<SpriteRenderer>(); //holds the non-projected points' spriterenderer after projectiles shot (as a to-do list for the reload function)


	//shooting conditions
	public List<int> canShoot = new List<int> (12); // determines whether a starpoint can be shot
	public List<int> autoShoot = new List<int> (12); // determines whether an individual starpoint will automatically fire after regenerating
	private bool autoShootAll = false; // determines whether all starpoints will automatically fire all at once after they regenerate
	public List<int> shootOnMouse = new List<int> (12); // determines whether an individual starpoint will fire on mouse click or spacebar


	//class variables (Initialized in Start for readability)
	public List<Material> starMats = new List<Material> (13); // holds the materials of each star class
	private List<float> projSpeeds = new List<float> (13); // holds the values for the speed of shot starpoints for each star class
	private List<float> projLife = new List<float> (13); // holds the values for lifetime of shot starpoints for each star class
	private List<float> projRegen = new List<float> (13); // holds the values for how fast starpoints regenerate for each star class
	private List<int> starPtClass = new List<int> (13); // holds the values for the number of starpoints for each star class
	public List<float> starSizes = new List<float> (13); // holds the values for the localscale size for each star class
	private List<int> starPtHealth = new List<int> (13); // holds the values for the health of starpoints for each star class
	public List<int> starPtDam = new List<int> (13); // holds the values for the damage inflicted by starpoints for each star class
	private List<int> starBodyHealth = new List<int> (13); // holds the values for the max health of the player for each star class
	private List<float> starBodyRegen = new List<float> (13); // holds the values for the health regeneration of the player for each star class
	private List<int> starBodyDam = new List<int> (13); // holds the values for the damage inflicted by the player starbody for each star class
	public int starType = 1; // the class of the star

	/*Plan:
	    -list of available classes
		-list of available classes
		-method run every frame to see what classes player is eligible for
		-method to update upgrade GUI based on this
		-GUIs to call UpgradeStar*/
	//Player Stats
	//public int playersKilled = 0;
	//public List<bool> availableClasses = new List<bool> (true, false, false, false, false, false, false, false, false, false, false, false, false, false);

	//Direction Vectors for projectiles
	public List<Vector2> pointVectList = new List<Vector2>(12); // the direction vectors for projectiles
	public List<float> pointAngles = new List<float>(12); // the angles at which starpoints are shot
	public List<float> pointAngles2 = new List<float>(12); // the angles at which starpoints are regenerated


	void Start() {

		ScenePhotonView = this.GetComponent<PhotonView>();
		// set initial shooting conditions
		canShoot = new List<int>{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
		autoShoot = new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		shootOnMouse = new List<int>{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

		this.tag = this.GetComponent<PhotonView> ().viewID.ToString();
		playerTag = this.tag;

		// set class variable values
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

		// initialize star to class 1
		if (starSizes.Count > 0) {
			ScenePhotonView.RPC("upgradeStar", PhotonTargets.AllBufferedViaServer, 1); // upgradeStar (1);
		}

	}


	void Update( )
	{
		if (this.GetComponent<Movement_Norm_Star> ().enabled == true) {
			
			ScenePhotonView = this.GetComponent<PhotonView>();

			// calculate the directions to shoot projectiles at that instant
			ScenePhotonView.RPC("redrawStar", PhotonTargets.All, transform.rotation, starPointNum); // redrawStar(transform.rotation, starPointNum);

			// regenerate player health					
			ScenePhotonView.RPC("healthRegen", PhotonTargets.All, playerRegen); // healthRegen (playerRegen);


			// REPLACE WITH CLASS PROGRESSION. BASE ON 'starMass' (equivalent to score)

		
			// downgrade star class (testing purposes)
//			if (Input.GetKeyDown (KeyCode.Y) && starType > 1)
//			{
//				starType = starType - 1;
//				ScenePhotonView.RPC("upgradeStar", PhotonTargets.AllBufferedViaServer, starType); //	upgradeStar(starType);
//			}
//
//			// upgrade star class (testing purposes)
//			if (Input.GetKeyDown (KeyCode.U) && starType < 13)
//			{
//				starType = starType + 1;
//				ScenePhotonView.RPC("upgradeStar", PhotonTargets.AllBufferedViaServer, starType); //	upgradeStar(starType);
//			}

		}
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
		
	// redraws the star with a particular number of points
	void resetShooting(Quaternion q, int numPoints, int starGrade){

		// turns shooting off and clears the reload to-do list
		GetComponent<Shooting_Controls_edit> ().autoShootAll = false;
		spri.Clear ();

		// destroys all the old starpoints and clears the starpoint list
		int oldPt = starpoints.Count;
		for (int i = 0; i < oldPt; i++) {
			Destroy (starpoints [i]);
		}

		starpoints.Clear ();

		// calculates the angles to draw the new starpoints at
		float angle2 = q.eulerAngles.z;
		float topAngle2 = angle2 + 90;


		pointAngles2.Clear ();
		pointAngles2.Add(topAngle2);

		for(int i = 1; i < numPoints; i++)
		{
			pointAngles2.Add(topAngle2 - i * (360 / numPoints));
		}
			
		// instantiates the new starpoints and gives them size, health, and damage, then adds them to the list and makes them shootable
		for(int i = 0; i < numPoints; i++)
		{
			GameObject newPt = Instantiate(starPointSprite, transform.position, Quaternion.identity) as GameObject;
			newPt.tag = playerTag;
			newPt.transform.localScale = new Vector3(starSizes [starGrade - 1]*0.6f,starSizes [starGrade - 1]*1f,starSizes [starGrade - 1]*0.5f);
			newPt.GetComponent<Renderer> ().material = starMats [starGrade - 1];
			newPt.transform.RotateAround(transform.position,Vector3.forward, (pointAngles2 [i] + 90));
			newPt.transform.parent = transform;
			newPt.GetComponent<Health_Management> ().Health = maxPointHealth;
			newPt.GetComponent<CollisionHandler> ().damage_to_give = maxPointDam;
			starpoints.Add (newPt);

			canShoot [i] = 1;
			autoShoot [i] = 0;
			shootOnMouse [i] = 1;
		}

		// Update Shooting Script with new shooting array values
		GetComponent<Shooting_Controls_edit> ().canShoot = canShoot;
		GetComponent<Shooting_Controls_edit> ().autoShoot = autoShoot;
		GetComponent<Shooting_Controls_edit> ().shootOnMouse = shootOnMouse;
	}

	[PunRPC]
	// reloads star under a particular class -> establishes stats and redraws the star
	void upgradeStar(int starGrade){
		
		transform.localScale = new Vector3(starSizes [starGrade - 1]*0.5f,starSizes [starGrade - 1]*0.5f,starSizes [starGrade - 1]*0.5f);
		GetComponent<Renderer> ().material = starMats [starGrade - 1];

		starPointNum = starPtClass [starGrade - 1];
		maxPointHealth = starPtHealth [starGrade - 1];
		maxPointDam = starPtDam [starGrade - 1];
		maxPlayerHealth = starBodyHealth [starGrade - 1];
		maxPlayerDam = starBodyDam [starGrade - 1];
		playerRegen = starBodyRegen [starGrade - 1];

		GetComponent<Health_Management> ().Health = maxPlayerHealth;
		GetComponent<CollisionHandler> ().damage_to_give = maxPlayerDam;

		ScenePhotonView = this.GetComponent<PhotonView> ();
		resetShooting (transform.rotation, starPointNum, starGrade);
		lifetime = projLife [starGrade - 1];
		projForce = projSpeeds [starGrade - 1];
		reloadTime = projRegen [starGrade - 1];

		GetComponent<Shooting_Controls_edit> ().preset = 1;
		if (starPointNum < 4) {
			GetComponent<Shooting_Controls_edit> ().presetMax = 2;
		} else if (starPointNum > 5) {
			GetComponent<Shooting_Controls_edit> ().presetMax = 4;
		} else {
			GetComponent<Shooting_Controls_edit> ().presetMax = 3;
		}
			
//		GameObject.Find ("Minimap").GetComponent<RectTransform> ().sizeDelta = new Vector2 (Camera.main.pixelWidth, Camera.main.pixelHeight);
	}

	[PunRPC]
	// gives points to the player
	public void AddMass(int points){
		starMass += points;
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



}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Shooting : Photon.MonoBehaviour {
	private static PhotonView ScenePhotonView;

	//sprites -> projectile and non-projected point
	public GameObject projectile;
	public GameObject starPointSprite;
	public Collider2D enemyCheck;

	//variables
	public float lifetime = 2.0f; // how long projectiles stay on screen
	public float projForce = 300.0f; // how much force the projectiles are given when shot
	public int starPointNum = 4; // how many points the star has
	public int maxPointHealth = 50; // how much health the projectiles have
	public int maxPointDam = 70; // how much damage the projectiles give when they collide with something
	public int maxPlayerHealth = 5000; // how much health the player has at full health
	public int maxPlayerDam = 300; // how much damage the player gives when they collide with something
	public float playerRegen = 1f; // how quickly the player regenerates their health
	public Material starMat; // the material of the star
	public float starSize = 3f; // how quickly the player regenerates their health
	public float reloadTime = 2f; //time to regen point
	public float colReload = 0.1f;

	private List<GameObject> starpoints = new List<GameObject>(); //holds the non-projected points
	private List<SpriteRenderer> spri = new List<SpriteRenderer>(); //holds the non-projected points' spriterenderer after projectiles shot (as a to-do list for the reload function)

	//shooting conditions
	public List<int> canShoot = new List<int> (); // determines whether a starpoint can be shot
	public bool autoShootAll = false; // determines whether all starpoints will automatically fire all at once after they regenerate

	//Direction Vectors for projectiles
	private List<Vector2> pointVectList = new List<Vector2>(); // the direction vectors for projectiles
	private List<float> pointAngles = new List<float>(); // the angles at which starpoints are shot
	private List<float> pointAngles2 = new List<float>(); // the angles at which starpoints are regenerated
	private float playerSpeed = 2f; //speed player moves
	private Vector2 movTarget; // where the player is to move towards
	private Vector2 dampSpeed = Vector2.zero; // the dampspeed for smoothdamping player movement
	private float smoothTime = 0.5f; // the smoothdamping delay
	private Vector2 velTarget; // the target velocity based on the differenct between player position and movTarget

	private float targetX;
	private float targetY;
	private Vector2 myTarget;
	private Vector3 myTarget3D;
	private bool targeting = false;
	private GameObject targetPlayer;
	private bool randoSelected = false;

	void Start() {

		canShoot = new List<int>{ 0, 0, 0, 0 };
		for (int i = 0; i < starPointNum; i++) {
			canShoot [i] = 1;
		}


		// initialize star to class 1
		//ScenePhotonView.RPC("upgradeStar", PhotonTargets.All);
		upgradeStar ();

	}


	void Update( )
	{
		// calibrate movTarget with player position
		movTarget.x = transform.position.x;
		movTarget.y = transform.position.y;
		selectTarget (targetPlayer);

		moveFunct ();
		rotate ();

		//ScenePhotonView.RPC("redrawStar", PhotonTargets.All, transform.rotation, starPointNum);
		// calculate the directions to shoot projectiles at that instant
		redrawStar(transform.rotation, starPointNum);

		//ScenePhotonView.RPC("healthRegen", PhotonTargets.All, playerRegen);
		// regenerate player health
		healthRegen (playerRegen);



		// toggle autoshooting for all points
		/* if (player within range)
		{
			autoShootAll = true;
			} else {
			autoShootAll = false;
		}*/



		// check to see if stars can be shot
		for (int i = 0; i < starPointNum; i++) {
			// check conditions to see if starpoint can be shot
			if (autoShootAll && canShoot [i] == 1) {
				//ScenePhotonView.RPC ("Shoot", PhotonTargets.All, i + 1);
				Shoot(i+1);
			}
		} 

	}



	//[PunRPC]
	//Creates projectile and shoots it in appropriate direction
	void Shoot(int point) {

		//clones existing projectile gameobject
		GameObject proj = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;

		// sets projectile size, material, health, damage, and accesses its rigidbody and spriterenderer
		proj.transform.localScale = new Vector3(starSize*0.6f,starSize*1f,starSize*0.5f);
		proj.GetComponent<Renderer> ().material = starMat;
		proj.GetComponent<Health_Management> ().Health = maxPointHealth;
		proj.GetComponent<CollisionHandler> ().damage_to_give = maxPointDam;
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

		// shoots projectile at proper rotation, direction, and speed, and gives recoil to player
		proj.transform.RotateAround(transform.position,Vector3.forward, (pointAngles [point-1] + 90));
		Debug.Log (projForce);
		rb.AddForce (pointVectList[point - 1]*projForce);
		GetComponent<Rigidbody2D> ().AddForce (-pointVectList [point - 1] * projForce/10f);


		// tells starpoint regeneration function to run

		//		ScenePhotonView.RPC("reload", PhotonTargets.All, starpoints [point - 1],spr,reloadTime, point - 1, starType);
		StartCoroutine(reload(starpoints [point - 1],spr,reloadTime, point - 1));
		StartCoroutine (collideEnable (proj, colReload, projCol));
		PolygonCollider2D pc = proj.AddComponent<PolygonCollider2D> ();
		pc.density = 0;
		pc.isTrigger = true;

		// destroys projectile
		Destroy(proj, lifetime);


	}

	// re-enable collider on shot starpoints since they initially collide
	IEnumerator collideEnable(GameObject projctl, float timeDelay, Collider2D colliderThing)
	{
		yield return new WaitForSeconds (timeDelay);
		colliderThing.enabled = true;
		//projctl.GetComponent<Renderer> ().material = Resources.Load<Material> ("Materials/Star_Pulsar");
	}

	//[PunRPC]
	// regenerate starpoints after they were shot off or destroyed
	IEnumerator reload(GameObject strPont,SpriteRenderer sprIndex, float delayTime, int strPt)
	{
		yield return new WaitForSeconds (delayTime);



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

	//[PunRPC]
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
			StartCoroutine(reload(starIndex,starIndex.GetComponent<SpriteRenderer>(),reloadTime, starpoints.FindIndex(d => d == starIndex)));

		}
	}

	//[PunRPC]
	// tells the health manager to regenerate player health over time
	public void healthRegen(float starRegen){
		GetComponent<Health_Management> ().Health = Mathf.MoveTowards (GetComponent<Health_Management> ().Health, maxPlayerHealth, starRegen * Time.deltaTime);
	}

	//[PunRPC]
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



	//[PunRPC]
	// redraws the star with a particular number of points
	void resetShooting(Quaternion q, int numPoints){

		// calculates the angles to draw the new starpoints at
		float angle2 = q.eulerAngles.z;
		float topAngle2 = angle2 + 90;
		pointAngles2.Clear ();
		pointAngles2.Add (topAngle2);
		for(int i = 1; i < numPoints; i++)
		{
			pointAngles2.Add(topAngle2 - i * (360 / numPoints));
		}

		// instantiates the new starpoints and gives them size, health, and damage, then adds them to the list and makes them shootable
		for(int i = 0; i < numPoints; i++)
		{
			GameObject newPt = Instantiate(starPointSprite, transform.position, Quaternion.identity) as GameObject;

			newPt.transform.localScale = new Vector3(starSize*0.6f,starSize*1f,starSize*0.5f);
			newPt.GetComponent<Renderer> ().material = starMat;
			newPt.transform.RotateAround(transform.position,Vector3.forward, (pointAngles2 [i] + 90));
			newPt.transform.parent = transform;
			newPt.GetComponent<Health_Management> ().Health = maxPointHealth;
			newPt.GetComponent<CollisionHandler> ().damage_to_give = maxPointDam;
			starpoints.Add (newPt);
			canShoot [i] = 1;

		}
	}


	//[PunRPC]
	// reloads star under a particular class -> establishes stats and redraws the star
	void upgradeStar(){

		transform.localScale = new Vector3(starSize*0.5f,starSize*0.5f,starSize*0.5f);
		GetComponent<Renderer> ().material = starMat;
		GetComponent<Health_Management> ().Health = maxPlayerHealth;
		GetComponent<CollisionHandler> ().damage_to_give = maxPlayerDam;
		//ScenePhotonView.RPC("resetShooting", PhotonTargets.All, transform.rotation, 4);
		resetShooting (transform.rotation, starPointNum);


	}


	public void selectTarget(GameObject enemyPl) {
		if (enemyPl != null) {
			if (randoSelected) {
				targetPlayer = enemyPl;
			}
			targetX = enemyPl.transform.position.x;
			targetY = enemyPl.transform.position.y;
			autoShootAll = true;
			randoSelected = false;

		} else if (!randoSelected) {
			targetX = Random.Range (-100f, 100f);
			targetY = Random.Range (-100f, 100f);
			autoShootAll = false;
			randoSelected = true;
			enemyCheck.GetComponent<AI_Detect_Player> ().inPursuit = false;
		}
		myTarget = new Vector2 (targetX, targetY);
		myTarget3D = new Vector3 (targetX, targetY, 0f);
	}

	//you will need to change this to point towards the player
	void rotate() {
		//Vector3 pos = player.transform.position;
		//Vector3 dir = pos - transform.position;

		//Test position to point at
		Vector3 pos = myTarget3D;
		Vector3 dir = pos - transform.position;


		//Test with pointing towards mouse
		//Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		//Vector3 dir = Input.mousePosition - pos;


		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
	}

	void moveFunct()
	{
		// add to movTarget based on player location
		/*if (player in range) {//Press left arrow key to move backward on the X AXIS

			Vector2 playerPosition = player.transform.position;
			Vector2 deltaPosition = playerPosition - movTarget; 
			movTarget += deltaPosition.normalized * (playerSpeed * Time.deltaTime / (transform.localScale.x));
			
		}*/ 

		// if no movement input, set velocity target to zero
		/*if (player not in range){
			velTarget = new Vector2 (0f, 0f);
		}*/

		//Test position to go to
		Vector2 playerPosition = myTarget; // NEED TO MAKE VARIABLE!!!
		Vector2 deltaPosition = playerPosition - movTarget; 
		movTarget += deltaPosition.normalized * (playerSpeed * Time.deltaTime / (transform.localScale.x));

		// set target velocity
		velTarget.x = (movTarget.x - transform.position.x)/Time.deltaTime;
		velTarget.y = (movTarget.y - transform.position.y)/Time.deltaTime;

		// accelerate the player to the target velocity with smoothdamp
		//GetComponent<Rigidbody2D> ().AddForce(velTarget);
		GetComponent<Rigidbody2D> ().velocity = Vector2.SmoothDamp (GetComponent<Rigidbody2D> ().velocity, velTarget, ref dampSpeed, smoothTime,playerSpeed,Time.deltaTime);

	}



}

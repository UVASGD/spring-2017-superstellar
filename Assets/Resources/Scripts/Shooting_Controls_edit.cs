using UnityEngine;
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
	public string playerTag;


	private List<GameObject> starpoints = new List<GameObject>(); //holds the non-projected points
	private List<SpriteRenderer> spri = new List<SpriteRenderer>(); //holds the non-projected points' spriterenderer after projectiles shot (as a to-do list for the reload function)


	//shooting conditions
	public List<int> canShoot = new List<int> (12); // determines whether a starpoint can be shot
	public List<int> autoShoot = new List<int> (12); // determines whether an individual starpoint will automatically fire after regenerating
	public bool autoShootAll = false; // determines whether all starpoints will automatically fire all at once after they regenerate
	public List<int> shootOnMouse = new List<int> (12); // determines whether an individual starpoint will fire on mouse click or spacebar
	public int preset; // current preset value
	public int presetMax; // maximum preset for current star type

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
		starpoints = GetComponent<StarManager>().starpoints;
		starSizes = GetComponent<StarManager>().starSizes;
		starType = GetComponent<StarManager>().starType;
		canShoot = GetComponent<StarManager>().canShoot;
		starPointNum = GetComponent<StarManager>().starPointNum;
		lifetime = GetComponent<StarManager>().lifetime;
		projForce = GetComponent<StarManager>().projForce;
		reloadTime = GetComponent<StarManager>().reloadTime;
		starPtDam = GetComponent<StarManager>().starPtDam;

		//ScenePhotonView.RPC("redrawStar", PhotonTargets.All, transform.rotation, starPointNum); // calculate the directions to shoot projectiles at that instant
																								// redrawStar(transform.rotation, starPointNum);

		// ScenePhotonView.RPC("healthRegen", PhotonTargets.All, playerRegen); // regenerate player health
																			   // healthRegen (playerRegen);



		//POSSIBLY USED FOR MODE SWITCHING INSTEAD OF "R"

		// set values for keys that can be used for shooting
		// List<KeyCode> shootKeys = new List<KeyCode> (12);
		// shootKeys = new List<KeyCode>{KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
		// KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, KeyCode.Minus, KeyCode.Equals};

		// toggle autoshooting for all points
		if (Input.GetKeyDown (KeyCode.F))
		{
			preset = -1;
			if (autoShootAll)
				autoShootAll = false;
			else {
				autoShootAll = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.E))
		{
			preset = -1;
			for (int i = 0; i < 12; i++) {
				if (starPointNum > i) {
					shootOnMouse [i] = 1;
				} else {
					shootOnMouse [i] = 0;
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.R))
		{
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
			if (starPointNum > i) {

				float currAngle = (360 / starPointNum) * i;
				switch (preset) {
					case 1:
						if (i == 0) {
							shootOnMouse [i] = 1;
						} else {
							shootOnMouse [i] = 0;
						}
						break;
					case 2:
						if (i == 0) {
							if (starPointNum < 5) {
								shootOnMouse [i] = 0;
							} else {
								shootOnMouse [i] = 1;
							}
						} else {
							if (currAngle > 90 && currAngle < 270) {
								shootOnMouse [i] = 1;
							} else {
								shootOnMouse [i] = 0;
							}
						}
						break;
					case 3:
						if (i == 0 || i == Mathf.FloorToInt ((float)starPointNum / 2) || i == Mathf.CeilToInt ((float)starPointNum / 2)) {
							shootOnMouse [i] = 0;
						} else {
							shootOnMouse [i] = 1;
						}
						break;
					case 4:
					if (currAngle > 90 && currAngle < 270 || i == 0) {
							shootOnMouse [i] = 0;
						} else {
							shootOnMouse [i] = 1;
						}
						break;
				}


				// check conditions to see if starpoint can be shot
				if (((autoShoot [i] == 1 || autoShootAll) || ((Input.GetMouseButton (0) || Input.GetKey (KeyCode.Space)) && shootOnMouse [i] == 1)) && canShoot [i] == 1) {
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
		Debug.Log(PlayerPrefs.GetString("PlayerName"));
		starPointNum = GetComponent<StarManager>().starPointNum;

		//clones existing projectile gameobject
		GameObject proj = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
		proj.tag = this.tag;

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

		proj.transform.localScale = new Vector3(starSizes [starType - 1]*0.6f,starSizes [starType - 1]*1f,starSizes [starType - 1]*0.5f);
		proj.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;

		proj.GetComponent<Health_Management> ().Health = maxPointHealth;
		proj.GetComponent<CollisionHandler> ().damage_to_give = starPtDam[starType - 1];
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


		// CHECK THIS! PLAYER ANGLES ARE NOT RIGHT
		// shoots projectile at proper rotation, direction, and speed, and gives recoil to player
		proj.transform.RotateAround(transform.position,Vector3.forward, (pointAngles [point-1] + 90));
		rb.AddForce (pointVectList[point - 1]* GetComponent<StarManager> ().projForce);
		GetComponent<Rigidbody2D> ().AddForce (-pointVectList [point - 1] *  GetComponent<StarManager> ().projForce/10f);

		// tells starpoint regeneration function to run
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

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarManager: Photon.MonoBehaviour
{

	private static PhotonView ScenePhotonView;

	//sprites -> projectile and non-projected point
	public GameObject projectile;
	public GameObject starPointSprite;

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

	public int playerTag = 0;



	private List<GameObject> starpoints = new List<GameObject>();
	//holds the non-projected points

	private List<SpriteRenderer> spri = new List<SpriteRenderer>();
	//holds the non-projected points' spriterenderer after projectiles shot (as a to-do list for the reload function)



	//class variables
	private List<Material> starMats = new List<Material> (13);
	// holds the materials of each star class

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

	void OnEnable()
	{
//		if (this.photonView != null && !this.photonView.isMine) {
//			Debug.Log ("disabled controls : " + this.photonView.ownerId);
//			this.enabled = false;
//			return;
//		} else {
//			Debug.Log("I am player "+ this.photonView.ownerId);
//		}
	}

	void Start() {

		ScenePhotonView = this.GetComponent<PhotonView>();
		// set initial shooting conditions

		playerTag = GetComponent<Tag_Manager> ().tag;

		// set class values
		starMats = new List<Material>{ Resources.Load<Material> ("Materials/Normal_Star_Yellow"), Resources.Load<Material> ("Materials/Star_D_Red"),
			Resources.Load<Material> ("Materials/Star_G_Red"),Resources.Load<Material> ("Materials/Star_SG_Red"),Resources.Load<Material> ("Materials/Star_D_White"),
			Resources.Load<Material> ("Materials/Star_SG_Blue"),Resources.Load<Material> ("Materials/Star_S_Nova"),Resources.Load<Material> ("Materials/Star_HG_Blue"),
			Resources.Load<Material> ("Materials/Star_Neutron"),Resources.Load<Material> ("Materials/Star_H_Nova"),Resources.Load<Material> ("Materials/Star_B_Hole"),
			Resources.Load<Material> ("Materials/Star_Quasar"),Resources.Load<Material> ("Materials/Star_Pulsar")};
		starPtClass = new List<int>{ 5, 4, 6, 8, 4, 10, 6, 12, 3, 7, 11, 9, 2 };
		starPtHealth = new List<int>{ 10, 15, 20, 30, 20, 50, 20, 70, 10, 30, 20, 30, 5 };
		starSizes = new List<float>{ 1f, 0.95f, 1.22f, 1.58f, 0.87f, 2f, 1.12f, 2.25f, 0.77f, 1.18f, 0.89f, 0.79f, 0.71f };
		starPtDam = new List<int>{ 10, 8, 15, 30, 15, 40, 30, 60, 8, 40, 30, 40, 5 };
		starBodyHealth = new List<int>{ 100, 150, 200, 300, 100, 350, 75, 500, 100, 50, 150, 200, 50 };
		starBodyRegen = new List<float>{ 0.1f, 0.2f, 0.05f, 0.03f, 0.3f, 0.02f, 0.3f, 0.01f, 0.4f, 0.4f, 0.3f, 0.2f, 0.5f };
		starBodyDam = new List<int>{ 20, 15, 40, 60, 15, 70, 30, 100, 10, 30, 40, 50, 5 };

		// initialize star to class 1
		ScenePhotonView.RPC("upgradeStar", PhotonTargets.All, 1);
		//		upgradeStar (1);

		if (this.photonView != null && !this.photonView.isMine) {
			Debug.Log ("disabled controls : " + this.photonView.ownerId);
			this.enabled = false;
			return;
		} else {
			Debug.Log("I am player "+ this.photonView.ownerId);
		}

	}


	void Update( )
	{
		ScenePhotonView.RPC("redrawStar", PhotonTargets.All, transform.rotation, starPointNum);
		// calculate the directions to shoot projectiles at that instant
		//		redrawStar(transform.rotation, starPointNum);

		ScenePhotonView.RPC("healthRegen", PhotonTargets.All, playerRegen);
		// regenerate player health
		//		healthRegen (playerRegen);

		// downgrade star class (testing purposes)
		if (Input.GetKeyDown (KeyCode.Y) && starType > 1)
		{
			starType = starType - 1;
			//			upgradeStar(starType);
			ScenePhotonView.RPC("upgradeStar", PhotonTargets.All, starType);
		}

		// upgrade star class (testing purposes)
		if (Input.GetKeyDown (KeyCode.U) && starType < 13)
		{
			starType = starType + 1;
			//			upgradeStar(starType);
			ScenePhotonView.RPC("upgradeStar", PhotonTargets.All, starType);
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



	}

	[PunRPC]
	// gives points to the player
	public void AddMass(int points){
		starMass += points;
	}

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionHandler : Photon.MonoBehaviour {

	// damage that the object gives to objects that collide with it
	public int damage_to_give;
	private static PhotonView p;
	// amount of health object has

	// amount of points to give when object dies
	public int pointsToGive;
	public GameObject killer;
	public bool check = false;

	public GameObject[] playerTags;
	void Start(){
		if (this.photonView != null && !this.photonView.isMine) {
			this.enabled = false;
			return;
		} 


	}


	void OnTriggerEnter2D(Collider2D coll)
		{
			Debug.Log(coll.gameObject.name);
		}
		
	void OnCollisionStay2D(Collision2D other)
	{

		string targetID;

		if (other.gameObject.GetComponent<PhotonView> () != null) {
			PhotonView pv = other.gameObject.GetComponent<PhotonView> ();
			targetID = pv.viewID.ToString();
		} else {
			other.gameObject.name = "target";
			targetID = other.gameObject.name;
		}

				int pvID = int.Parse(gameObject.tag);
				Debug.Log (pvID);
				PhotonView starpv = PhotonView.Find (pvID);
				Debug.Log (starpv);
				starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, targetID, gameObject.tag);
				Debug.Log ("new score: "+starpv.gameObject.GetComponent<Score_Manager> ().score);


	}

	public GameObject findStar(){
		Debug.Log (gameObject.tag);

		GameObject[] starproj = GameObject.FindGameObjectsWithTag (gameObject.tag);
		GameObject star = starproj[0];
		Debug.Log ("length"+starproj.Length);
		Debug.Log ("first thing: "+starproj[0]);

		for (int i = 0; i < starproj.Length; i++) {
			Debug.Log ("star[i]: "+starproj[i].name);

			if (starproj [i].name == "Star") {
				p = starproj [i].GetComponent<PhotonView> ();
				Debug.Log (p);
				star= starproj [i];
			

			}

		}
		check = true;
		Debug.Log ("Set star to: " + star+"which is of type: "+ star.name);
		return star;

	}


	[PunRPC]
	public void giveDamage(int damage, string targetID, string starTag){

		GameObject target;
		if (targetID == "target") { 
			target = GameObject.Find (targetID);
		} else {
			PhotonView targetpv = PhotonView.Find(int.Parse(targetID));
			target = targetpv.gameObject;
		}

		GameObject star = PhotonView.Find(int.Parse(starTag)).gameObject;

		Debug.Log ("starTag: "+starTag);


		target.GetComponent<Health_Management> ().Health -= damage;


		// kill the object when its health is depleted
		if (target.GetComponent<Health_Management> ().Health <= 0) {

			// if the object is an un-shot starpoint, destroy it using the shooting controls script
			if (target.tag == "Star_Point") {
				GetComponentInParent<Shooting_Controls_edit> ().destroyStarPoint (target);

				// if the object is a player, kill it (not yet...)
			} else if (target.tag == "Player_Star") {

				// if the object is anything else, destroy it and give the player points
			} else {
				Destroy (target);
				Debug.Log ("enter");
				star.GetComponent<Score_Manager> ().score += 5;
			}
		}
	}
		
	
}
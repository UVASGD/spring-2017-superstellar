using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionHandler : Photon.MonoBehaviour {

	// damage that the object gives to objects that collide with it
	public int damage_to_give;
	private static PhotonView p;
	// amount of health object has
	public float Health;

	// amount of points to give when object dies
	public int pointsToGive;

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


		GameObject[] starproj = GameObject.FindGameObjectsWithTag (other.gameObject.GetComponent<Tag_Manager> ().tag);
		for (int i = 0; i < starproj.Length; i++) {
			if (starproj [i].name == "Star") {

				p = starproj [i].GetComponent<PhotonView> ();
			}

		}

//			other.GetComponent<Tag_Manager>().tag.GetComponent<PhotonView>();
//		Debug.Log(p);

			// give damage to background stars
			if (other.gameObject.tag == "BG_Stars") {
				if (gameObject.tag != "BG_Stars") {
				p.RPC ("giveDamage", PhotonTargets.All, damage_to_give, gameObject);

				}

			}

			// give damage to starpoints (shot and un-shot)
			if (other.gameObject.tag == "Star_Proj" || other.gameObject.tag == "Star_Point") {
			p.RPC ("giveDamage", PhotonTargets.All, damage_to_give, gameObject);
			}

			// give damage to players and add recoil force
			if (other.gameObject.tag == "Player_Star") {
				p.RPC ("giveDamage", PhotonTargets.All, damage_to_give, gameObject);
				other.gameObject.GetComponent<Rigidbody2D> ().AddForce (other.gameObject.GetComponent<Rigidbody2D> ().velocity.normalized * -100);

			}

		}


	[PunRPC]
	public void giveDamage(int damage, GameObject killer){
		Health -= damage;

		// kill the object when its health is depleted
		if (Health <= 0) {

			// if the object is an un-shot starpoint, destroy it using the shooting controls script
			if (gameObject.tag == "Star_Point") {
				GetComponentInParent<Shooting_Controls_edit> ().destroyStarPoint (gameObject);

				// if the object is a player, kill it (not yet...)
			} else if (gameObject.tag == "Player_Star") {

				// if the object is anything else, destroy it and give the player points
			} else {
				Destroy (gameObject);

				// player to give points to
				string killerTag = killer.GetComponent<Tag_Manager>().tag;

				//if (playerTags == null){
				//playerTags = GameObject.FindGameObjectsWithTag ("Player_Star");
				//}
				playerTags = GameObject.FindGameObjectsWithTag ("Player_Star");

				for (int i = 0; i < playerTags.GetLength (0); i++) {
					if (playerTags [i].GetComponent<Tag_Manager> ().tag == killerTag) {


						playerTags[i].GetComponent<StarManager> ().AddMass (pointsToGive);
					}
				}
			}
		}
	}
		
	
}
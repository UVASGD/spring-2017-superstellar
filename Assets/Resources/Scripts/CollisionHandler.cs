using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionHandler : Photon.MonoBehaviour {

	// damage that the object gives to objects that collide with it
	public int damage_to_give;

	// amount of points to give when object dies
//	public int pointsToGive;


	void Start(){
		if (this.photonView != null && !this.photonView.isMine) {
			this.enabled = false;
			return;
		} 
	}
		
	void OnCollisionEnter2D(Collision2D other)
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
		PhotonView starpv = PhotonView.Find (pvID);



		starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, targetID);

		if (this.gameObject.name != "Projectile(Clone)") {
			starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, pvID.ToString ());
		}

	}


	[PunRPC]
	public void giveDamage(int damage, string targetID){

		GameObject target;
		if (targetID == "target") {
			target = GameObject.Find (targetID);
		} else {
			PhotonView targetpv = PhotonView.Find(int.Parse(targetID));
			target = targetpv.gameObject;
		}
			
	


		target.GetComponent<Health_Management> ().Health -= damage;





		// kill the object when its health is depleted
		if (target.GetComponent<Health_Management> ().Health <= 0) {

			if (target.name == "Star_Point") { // if the object is an un-shot starpoint, destroy it using the shooting controls script
				GetComponentInParent<Shooting_Controls_edit> ().destroyStarPoint (target);

			} else if (target.name == "Player(Clone)"){	// if the object is a player, kill it (not yet...)


			} else if (target.name == "BG_Star(Clone)") { // if the object is anything else, destroy it and give the player points
				Destroy (target);
				GameObject star = PhotonView.Find(int.Parse(gameObject.tag)).gameObject;
				star.GetComponent<Score_Manager> ().score += 5;
			}
		}
	}




}
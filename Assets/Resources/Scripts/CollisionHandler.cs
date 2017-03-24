using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionHandler : Photon.MonoBehaviour {

	// damage that the object gives to objects that collide with it
	public int damage_to_give;

	// amount of points to give when object dies
//	public int pointsToGive;

	void OnTriggerEnter2D(Collider2D other)
	{
		string targetID;
		//bool isSelf = false;
		if (other.gameObject.GetComponent<PhotonView> () != null) {
			PhotonView pv = other.gameObject.GetComponent<PhotonView> ();
			targetID = pv.viewID.ToString ();
		} else if (other.gameObject.name == "Star_Point(Clone)" || other.gameObject.name == "Projectile(Clone)") {
			targetID = other.gameObject.tag;
			/*string myID = gameObject.tag;
			if (myID == targetID) {
				isSelf = true;
			}*/
		} else {
			other.gameObject.name = "target";
			targetID = other.gameObject.name;
		}

		int damage_to_receive = other.gameObject.GetComponent<CollisionHandler> ().damage_to_give;

		int pvID = int.Parse(gameObject.tag);
		PhotonView starpv = PhotonView.Find (pvID);

		if (!targetID.Equals(pvID.ToString())) {
			starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, targetID);
			if (this.gameObject.name == "Star_Point(Clone)") {
				this.GetComponent<Health_Management> ().Health -= damage_to_receive;
				/*if (this.GetComponent<Health_Management> ().Health <= 0) {
					this.GetComponentInParent<Shooting_Controls_edit> ().destroyStarPoint (this.gameObject);
				}*/
			}
			else if (this.gameObject.name != "Projectile(Clone)") {
				starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_receive, pvID.ToString ());
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		string targetID;
		if (other.gameObject.GetComponent<PhotonView> () != null) {
			PhotonView pv = other.gameObject.GetComponent<PhotonView> ();
			targetID = pv.viewID.ToString();
		} else if (other.gameObject.name == "Star_Point(Clone)" || other.gameObject.name == "Projectile(Clone)") {
			targetID = other.gameObject.tag;
		} else {
			other.gameObject.name = "target";
			targetID = other.gameObject.name;
		}

		int damage_to_receive = other.gameObject.GetComponent<CollisionHandler> ().damage_to_give;

		int pvID = int.Parse(gameObject.tag);
		PhotonView starpv = PhotonView.Find (pvID);

		if (!targetID.Equals(pvID.ToString())) {
			starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, targetID);
			if (this.gameObject.name != "Projectile(Clone)") {
				starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_receive, pvID.ToString ());
			} else {
				Destroy (this.gameObject);
			}
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
			// if the object is an un-shot starpoint, destroy it using the shooting controls script
			/*if (target.name == "Star_Point(Clone)") {
				GetComponentInParent<Shooting_Controls_edit> ().destroyStarPoint (target);
				// if the object is anything else, destroy it and give the player points
			} else {*/
				//Destroy (target);
				int scoreGive = target.GetComponent<Health_Management>().scoreToGive;
				gameObject.GetComponent<Score_Manager> ().score += scoreGive;
			//}
		}
	}



}
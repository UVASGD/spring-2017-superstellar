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

		Debug.Log ("IS THIS REAL?!");
		string targetID;
		if (other.gameObject.GetComponent<PhotonView> () != null) {
			PhotonView pv = other.gameObject.GetComponent<PhotonView> ();
			targetID = pv.viewID.ToString ();
		} else if (other.gameObject.name == "Star_Point(Clone)" || other.gameObject.name == "Projectile(Clone)") {
			targetID = gameObject.tag;
		} else {
			other.gameObject.name = "target";
			targetID = other.gameObject.name;
		}

		int pvID = int.Parse(gameObject.tag);
		PhotonView starpv = PhotonView.Find (pvID);

		if (!targetID.Equals(pvID.ToString())) {
			starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, targetID);
			starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, pvID.ToString ());
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{

		Debug.Log ("IS THIS REAL?!");
		string targetID;
		if (other.gameObject.GetComponent<PhotonView> () != null) {
			PhotonView pv = other.gameObject.GetComponent<PhotonView> ();
			targetID = pv.viewID.ToString();
		} else if (other.gameObject.name == "Star_Point(Clone)" || other.gameObject.name == "Projectile(Clone)") {
			targetID = gameObject.tag;
		} else {
			other.gameObject.name = "target";
			targetID = other.gameObject.name;
		}

		int pvID = int.Parse(gameObject.tag);
		PhotonView starpv = PhotonView.Find (pvID);

		if (!targetID.Equals(pvID.ToString())) {
			starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, targetID);
			if (this.gameObject.name != "Projectile(Clone)") {
				starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, pvID.ToString ());
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
			if (target.tag == "Star_Point") {
				GetComponentInParent<Shooting_Controls_edit> ().destroyStarPoint (target);
				// if the object is anything else, destroy it and give the player points
			} else {
				Destroy (target);
				gameObject.GetComponent<Score_Manager> ().score += 5;
			}
		}
	}



}
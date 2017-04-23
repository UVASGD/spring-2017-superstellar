﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionHandler : Photon.MonoBehaviour {

	// damage that the object gives to objects that collide with it
	public int damage_to_give;
	public AudioClip damageSound;
	private AudioSource source;
	private List<string> ignoreObjects = new List<string>{"Background","AI_Collider","Top","Bottom","Left","Right"};
	public int pointsToGive; // amount of points to give when object dies

	void Start()
	{
		source = GameObject.Find("Background").GetComponent<AudioSource> ();
	}	


	void OnTriggerEnter2D(Collider2D other)
	{
		
		handleCollision (other.gameObject);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		handleCollision (other.gameObject);
	}

	void handleCollision(GameObject other) {
		
		if (!ignoreObjects.Contains(other.name)) {
			int viewID = other.GetComponent<Health_Management> ().viewID;

			int pvID = gameObject.GetComponent<Health_Management> ().viewID;
			PhotonView starpv = PhotonView.Find (pvID);

			//check to not kill self
			if (viewID != pvID && other.name != "Projectile(Clone)") {
				starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, viewID); // give damage to target
				if (this.gameObject.name != "Projectile(Clone)") {
					starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, pvID);
				} else {
					starpv.RPC ("destroyObj", PhotonTargets.All);
				}
			}
		}

	}
		

	[PunRPC]
	public void giveDamage(int damage, int targetID){
		if (PhotonView.Find (targetID)) {
			GameObject target = PhotonView.Find (targetID).gameObject;
			target.GetComponent<Health_Management> ().Health -= damage;

			//Play damage sound if target health is > 0 and target is not an AI
			if (target.GetComponent<Health_Management> ().Health > 0) {
				source.PlayOneShot (damageSound, .5f);
			}
			// kill the object when its health is depleted
			if (target.GetComponent<Health_Management> ().Health <= 0) {
				if (target.tag == "Star_Point") { 
					GetComponentInParent<Shooting_Controls_edit> ().destroyStarPoint (target); // if the object is an un-shot starpoint, destroy it using the shooting controls script
				} else if (gameObject.name == "Star") { 
					PhotonView pv = PhotonView.Find (this.GetComponent<Health_Management> ().viewID);
					pv.RPC ("updateScore", PhotonTargets.AllBuffered, target.GetComponent<Health_Management> ().scoreToGive); // if the object is anything else, destroy it and give the player points
				}
			}
		}
	}

	[PunRPC]
	public void updateScore(int score) {
		gameObject.GetComponent<Score_Manager> ().score += score;
	}
		
	[PunRPC]
	public void destroyObj() {
		if (this.gameObject.transform.parent) {
			Destroy (this.gameObject.transform.parent.gameObject);
		} else {
			Destroy (this.gameObject);
		}
	}

}
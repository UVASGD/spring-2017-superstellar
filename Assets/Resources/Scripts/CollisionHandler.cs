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
		string targetTag;
		string targetOldName;

		targetTag = other.gameObject.tag;
		targetOldName = other.gameObject.name;

		if (other.gameObject.GetComponent<PhotonView> () != null) {
			PhotonView pv = other.gameObject.GetComponent<PhotonView> ();
			targetID = pv.viewID.ToString ();

		} else if (other.gameObject.name == "Star_Point(Clone)" || other.gameObject.name == "Projectile(Clone)" || other.gameObject.name == "AI_Collider") {
			targetID = other.gameObject.tag;

		} else {
			other.gameObject.name = "target";
			targetID = other.gameObject.name;
		}

		int damage_to_receive = 0;
		if (other.gameObject.GetComponent<CollisionHandler> () != null) {

			damage_to_receive = other.gameObject.GetComponent<CollisionHandler> ().damage_to_give;
		}

		int pvID = 0;
		if (this.gameObject.GetComponentInParent<StarManager> () != null ||
			this.gameObject.GetComponentInParent<AI_Shooting> () != null || 
			this.gameObject.name == "Projectile(Clone)" ||
			this.gameObject.GetComponent<AI_Detect_Player>() != null) {
			//Debug.Log (this.gameObject.tag);
			int.TryParse (this.gameObject.tag, out pvID);
		} else if (this.gameObject.GetComponent<PhotonView>() != null) {
			pvID = this.gameObject.GetComponent<PhotonView> ().viewID;
		}

		PhotonView starpv;
		if (pvID != 0) {
			starpv = PhotonView.Find (pvID);
		

		if (this.gameObject.tag != other.gameObject.tag && starpv != null && this.gameObject.GetComponent<Health_Management>() != null && other.gameObject.GetComponent<Health_Management>() != null) {
				//Debug.Log (this.gameObject.tag + " " + other.gameObject.tag);
				if (other.gameObject.name == "Star_Point(Clone)" || other.gameObject.name == "Projectile(Clone)") {
					other.gameObject.GetComponent<Health_Management> ().Health -= damage_to_give;

				} else if (damage_to_give != 0) {
					starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, targetID, targetTag, targetOldName);
					Debug.Log (this.gameObject.name + " tag: " + this.gameObject.tag + " giving T damage to " + other.gameObject.name + " tag: " + this.gameObject.tag);
				}
			
			
				if (this.gameObject.name == "Star_Point(Clone)" || this.gameObject.name == "Projectile(Clone)") {
					if (damage_to_receive != 0) {
						this.gameObject.GetComponent<Health_Management> ().Health -= damage_to_receive;
						Debug.Log (this.gameObject.tag + " starpoint was hit by " + other.gameObject.name);
					}

				} else if (damage_to_receive != 0) {
					starpv.RPC ("receiveDamage", PhotonTargets.All, damage_to_receive, other.gameObject.tag);
					Debug.Log (this.gameObject.name + " tag: " + this.gameObject.tag + " receiving T damage from " + other.gameObject.name + " tag: " + this.gameObject.tag);

				}
		}
		} else {
			Debug.Log (this.gameObject.name + " returned null photonview ID");
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{

		string targetID;
		string targetTag;
		string targetOldName;

		targetTag = other.gameObject.tag;
		targetOldName = other.gameObject.name;

		if (other.gameObject.GetComponent<PhotonView> () != null) {
			PhotonView pv = other.gameObject.GetComponent<PhotonView> ();
			targetID = pv.viewID.ToString ();

		} else if (other.gameObject.name == "Star_Point(Clone)" || other.gameObject.name == "Projectile(Clone)" || other.gameObject.name == "AI_Collider") {
			targetID = other.gameObject.tag;

		} else {
			other.gameObject.name = "target";
			targetID = other.gameObject.name;
		}

		int damage_to_receive = 0;
		if (other.gameObject.GetComponent<CollisionHandler> () != null) {

			damage_to_receive = other.gameObject.GetComponent<CollisionHandler> ().damage_to_give;
		}


		int pvID = 0;
		if (this.gameObject.GetComponentInParent<StarManager> () != null ||
			this.gameObject.GetComponentInParent<AI_Shooting> () != null || 
			this.gameObject.name == "Projectile(Clone)" ||
			this.gameObject.GetComponent<AI_Detect_Player>() != null) {
			//Debug.Log (this.gameObject.tag);
			int.TryParse (this.gameObject.tag, out pvID);
		} else if (this.gameObject.GetComponent<PhotonView>() != null) {
			pvID = this.gameObject.GetComponent<PhotonView> ().viewID;
		}

		PhotonView starpv;
		if (pvID != 0) {
			starpv = PhotonView.Find (pvID);
		



		if (this.gameObject.tag != other.gameObject.tag && starpv != null && this.gameObject.GetComponent<Health_Management>() != null && other.gameObject.GetComponent<Health_Management>() != null) {
				//Debug.Log (this.gameObject.tag + " " + other.gameObject.tag);
				if (other.gameObject.name == "Star_Point(Clone)" || other.gameObject.name == "Projectile(Clone)") {
					other.gameObject.GetComponent<Health_Management> ().Health -= damage_to_give;

				} else if (damage_to_give != 0) {
					starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, targetID, targetTag, targetOldName);
					Debug.Log (this.gameObject.name + " tag: " + this.gameObject.tag + " giving damage to " + other.gameObject.name + " tag: " + this.gameObject.tag);
				}
				if (this.gameObject.name == "Star_Point(Clone)" || this.gameObject.name == "Projectile(Clone)") {
					if (damage_to_receive != 0) {
						this.gameObject.GetComponent<Health_Management> ().Health -= damage_to_receive;
						Debug.Log (this.gameObject.tag + " starpoint was hit by " + other.gameObject.name);
					}

				} else if (damage_to_receive != 0) {
					starpv.RPC ("receiveDamage", PhotonTargets.All, damage_to_receive, other.gameObject.tag);
					Debug.Log (this.gameObject.name + " tag: " + this.gameObject.tag + " receiving damage from " + other.gameObject.name + " tag: " + this.gameObject.tag);

				}
		}
		} else {
			Debug.Log (this.gameObject.name + " returned null photonview ID");
		}
	}



	[PunRPC]
	public void giveDamage(int damage, string targetName, string targetTagg, string targetOldeName){

		GameObject target;

		if (targetName == "target") {
			foreach (GameObject foundObj in GameObject.FindGameObjectsWithTag(targetTagg)) {
				if (foundObj.name == "target") {
					target = foundObj;
					//Debug.Log (target.name);
					target.name = targetOldeName;

					Health_Management healthManager = target.GetComponent<Health_Management> ();
					healthManager.Health -= damage;

					// kill the object when its health is depleted
					if (healthManager.Health <= 0) {
						int scoreGive = healthManager.scoreToGive;
						foreach (GameObject foundSelf in GameObject.FindGameObjectsWithTag(this.gameObject.tag)) {
							if (foundSelf.GetComponent<Score_Manager> () != null) {
								foundSelf.GetComponent<Score_Manager> ().score += scoreGive;
							}
						}
					}
				}
			}
		} else {
			int pvID2 = 0;
			int.TryParse (targetName, out pvID2);
			if (pvID2 != 0) {
				PhotonView targetpv = PhotonView.Find (pvID2);
				target = targetpv.gameObject;
				//Debug.Log (target.name);
				//target.gameObject.name = targetOldeName;
				Health_Management healthManager = target.GetComponent<Health_Management> ();
				healthManager.Health -= damage;

				// kill the object when its health is depleted
				if (healthManager.Health <= 0) {
					int scoreGive = healthManager.scoreToGive;
					foreach (GameObject foundSelf in GameObject.FindGameObjectsWithTag(this.gameObject.tag)) {
						if (foundSelf.GetComponent<Score_Manager> () != null) {
							foundSelf.GetComponent<Score_Manager> ().score += scoreGive;
						}
					}
				}
			} else {
				Debug.Log ("Could not damage " + targetName + " with tag: " + targetTagg);
			}
		}

			
		
	}

	[PunRPC]
	public void receiveDamage(int damage, string killerTag){

		Health_Management healthManager = this.gameObject.GetComponent<Health_Management> ();
		healthManager.Health -= damage;

		// kill the object when its health is depleted
		if (healthManager.Health <= 0) {
			int scoreGive = healthManager.scoreToGive;
			foreach (GameObject foundSelf in GameObject.FindGameObjectsWithTag(killerTag)) {
				if (foundSelf.GetComponent<Score_Manager> () != null) {
					foundSelf.GetComponent<Score_Manager> ().score += scoreGive;
				}
			}
		}

	}



	/*void OnTriggerEnter2D(Collider2D other)
	{
		string targetID;
		//Debug.Log (this.gameObject.name + " was hit by " + other.gameObject.name);

		if (other.gameObject.GetComponent<PhotonView> () != null) {
			PhotonView pv = other.gameObject.GetComponent<PhotonView> ();
			targetID = pv.viewID.ToString ();
		} else if (other.gameObject.name == "Star_Point(Clone)" || other.gameObject.name == "Projectile(Clone)" || other.gameObject.name == "AI_Collider") {
			targetID = other.gameObject.tag;

		} else {
			other.gameObject.name = "target";
			targetID = other.gameObject.name;
		}

		int damage_to_receive = other.gameObject.GetComponent<CollisionHandler> ().damage_to_give;

		int pvID = 0;
		if (this.gameObject.GetComponentInParent<StarManager> () != null ||
			this.gameObject.GetComponentInParent<AI_Shooting> () != null || 
			this.gameObject.name == "Projectile(Clone)" ||
			this.gameObject.GetComponent<AI_Detect_Player>() != null) {
			Debug.Log (this.gameObject.tag);
			pvID = int.Parse (this.gameObject.tag);
		} else if (this.gameObject.GetComponent<PhotonView>() != null) {
			pvID = this.gameObject.GetComponent<PhotonView> ().viewID;
		}

		PhotonView starpv = PhotonView.Find (pvID);

		if (!targetID.Equals(pvID.ToString()) && starpv != null) {
			if (other.gameObject.GetComponent<AI_Detect_Player>() == null) {
				starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_give, targetID);
				Debug.Log (this.gameObject.name + "tag: " + this.gameObject.tag + " giving damage to " + other.gameObject.name + "tag: " + this.gameObject.tag);
			}
			if (this.gameObject.name == "Star_Point(Clone)" && !other.gameObject.CompareTag(this.gameObject.tag)) {
				this.GetComponent<Health_Management> ().Health -= damage_to_receive;
				Debug.Log (this.gameObject.tag + " starpoint was hit by " + other.gameObject.name);

			}
			else if (this.gameObject.name != "Projectile(Clone)" && other.gameObject.GetComponent<AI_Detect_Player>() == null) {
				starpv.RPC ("giveDamage", PhotonTargets.All, damage_to_receive, pvID.ToString ());
				Debug.Log (this.gameObject.name + "tag: " + this.gameObject.tag + " receiving damage from " + other.gameObject.name + "tag: " + this.gameObject.tag);
			
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

		int pvID = 0;
		if (this.gameObject.GetComponentInParent<StarManager> () != null ||
			this.gameObject.GetComponentInParent<AI_Shooting> () != null || 
			this.gameObject.name == "Projectile(Clone)" ||
			this.gameObject.GetComponent<AI_Detect_Player>() != null) {
			Debug.Log (this.gameObject.tag);
			pvID = int.Parse (this.gameObject.tag);
		} else if (this.gameObject.GetComponent<PhotonView>() != null) {
			pvID = this.gameObject.GetComponent<PhotonView> ().viewID;
		}
		PhotonView starpv = PhotonView.Find (pvID);

		if (!targetID.Equals(pvID.ToString()) && starpv != null) {
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

		if (target.GetComponent<Health_Management> () != null) {
			target.GetComponent<Health_Management> ().Health -= damage;

			// kill the object when its health is depleted
			if (target.GetComponent<Health_Management> ().Health <= 0) {
				// if the object is an un-shot starpoint, destroy it using the shooting controls script

				//Destroy (target);
				int scoreGive = target.GetComponent<Health_Management> ().scoreToGive;
				if (this.gameObject.GetComponent<Score_Manager> () != null) {
					gameObject.GetComponent<Score_Manager> ().score += scoreGive;
				}
				//}
			}
		}
	}*/



}
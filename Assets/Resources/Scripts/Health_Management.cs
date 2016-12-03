using UnityEngine;
using System.Collections;

public class Health_Management : Photon.MonoBehaviour {



	// amount of health object has
	public float Health;

	// amount of points to give when object dies
	public int pointsToGive;

	public GameObject[] playerTags;

	private static PhotonView ScenePhotonView;


	void Start () {

		Debug.Log ("HEALTH MULTI");
		if (this.photonView != null && !this.photonView.isMine) {
			this.enabled = false;
			return;
		} 

		ScenePhotonView = this.GetComponent<PhotonView>();


	}
	

	void Update () {
		
	
	}

	// give damage to the object

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

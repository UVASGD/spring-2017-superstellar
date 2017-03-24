﻿using UnityEngine;
using System.Collections;

public class Health_Management : Photon.MonoBehaviour {



	// amount of health object has
	public float Health;
	public int scoreToGive;

	private static PhotonView ScenePhotonView;


	void Start () {

		if (this.photonView != null && !this.photonView.isMine) {
			this.enabled = false;
			return;
		} 
		Debug.Log (this.gameObject.name);
		ScenePhotonView = this.GetComponent<PhotonView>();
	}

	void Update () {
		if (this.gameObject.name == "Star") {
			scoreToGive = GetComponent<Score_Manager> ().score;
		}
		if (Health <= 0) {

			if (this.gameObject.name == "Star") {
				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (2);
				PhotonNetwork.Disconnect();
			} else if(this.gameObject.name == "Star_Point(Clone)"){
				this.GetComponentInParent<Shooting_Controls_edit> ().destroyStarPoint (this.gameObject);
			} else {
				Debug.Log ("Destroyed gameobject");
				Destroy (this.gameObject);
			}
		}
	}


}

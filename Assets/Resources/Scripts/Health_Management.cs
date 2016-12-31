using UnityEngine;
using System.Collections;

public class Health_Management : Photon.MonoBehaviour {



	// amount of health object has
	public float Health;

	private static PhotonView ScenePhotonView;


	void Start () {

		if (this.photonView != null && !this.photonView.isMine) {
			this.enabled = false;
			return;
		} 

		ScenePhotonView = this.GetComponent<PhotonView>();
	}
	


}

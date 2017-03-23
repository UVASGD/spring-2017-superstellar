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
		Debug.Log (this.gameObject.name);
		ScenePhotonView = this.GetComponent<PhotonView>();
	}

	void Update () {
		if (Health <= 0) {

			if (this.gameObject.name == "Star") {
				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (2);
				PhotonNetwork.Disconnect();
			} else {
				Debug.Log ("Destroyed gameobject");
				Destroy (this.gameObject);
			}
		}
	}


}

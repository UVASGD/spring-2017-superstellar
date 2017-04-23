using UnityEngine;
using System.Collections;

public class Health_Management : Photon.MonoBehaviour {



	// amount of health object has
	public float Health;
	public int scoreToGive;
	public int viewID;

	void Start () {
		if (this.gameObject.name == "BG_Star(Clone)") {
			viewID = this.photonView.viewID;
		}
	}

	void Update () {
		if (this.gameObject.name == "Star") {
			scoreToGive = GetComponent<Score_Manager> ().score;
		}
		if (Health <= 0) {
			Debug.Log ("Health is 0");
			if (this.gameObject.name == "Star" && this.photonView.isMine) {
				Debug.Log ("Sepukku");
				int score = this.GetComponent<Score_Manager> ().score;
				string name = this.GetComponent<Score_Manager> ().playerNameForDeath;
				int classNum = this.GetComponent<StarManager> ().starType;
				string className = this.GetComponent<StarManager> ().starClassNames [classNum];

				PlayerPrefs.SetInt ("dscore", score);
				PlayerPrefs.SetString ("dname", name);
				PlayerPrefs.SetString ("dclass", className);

				//load death scene
				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (2);
				PhotonNetwork.Disconnect ();
			} else if (this.gameObject.name == "Star_Point(Clone)") {
				Debug.Log ("Ayyyy, mah triggers");
				if (this.GetComponentInParent<Shooting_Controls_edit> () != null) {
					Debug.Log ("destroy mah star points");
					this.GetComponentInParent<Shooting_Controls_edit> ().destroyStarPoint (this.gameObject);
				} else if (this.GetComponentInParent<AI_Shooting> () != null) {
					Debug.Log ("destroy da AI points");
					this.GetComponentInParent<AI_Shooting> ().destroyStarPoint (this.gameObject);
				}
			} else {
				Debug.Log ("Destroys background stars for everyone ayyy");
				this.GetComponent<PhotonView> ().RPC ("destroyObj", PhotonTargets.All);
			}
		}
	}

	[PunRPC]
	void destroyObj() {
		if (this.gameObject.name == "Body") {
			Debug.Log ("Destroyed AI");
			Destroy (this.gameObject.transform.parent.gameObject);
		} else {
			Debug.Log ("Destroyed gameobject");
			Destroy (this.gameObject);
		}
	}


}

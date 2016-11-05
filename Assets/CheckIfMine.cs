using UnityEngine;
using System.Collections;

public class CheckIfMine : Photon.MonoBehaviour {

	void OnEnable()
	{
		if (this.photonView != null && !this.photonView.isMine) {
			Debug.Log("entered disable");
			this.enabled = false;
			return;
		}
	}
}

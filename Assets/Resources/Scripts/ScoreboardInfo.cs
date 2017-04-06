using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardInfo : MonoBehaviour {

	public GameObject leaderboardGUI;

	SortedDictionary<int, string> psn = new SortedDictionary<int, string>();
	string leaderboardInfo;

	// Photon Checks to only show local player minimap
	void OnEnable()
	{
		PhotonView pv = this.transform.parent.gameObject.GetPhotonView ();
		if (pv != null && !pv.isMine) {
			this.enabled = false;
			for (int i = 0; i < this.transform.childCount; ++i) {
				this.transform.GetChild (i).gameObject.SetActiveRecursively (false);
			}
		} 
	}
	
	// Update is called once per frame
	void Update () {
		
		Score_Manager[] smList = GameObject.FindObjectsOfType<Score_Manager> ();
		foreach (Score_Manager sm in smList) {
			psn.Add (sm.score, sm.playerNameForDeath);
		}

		foreach (int score in psn.Keys) {
			leaderboardInfo += psn[score] + ": "+ score + "\n";
		}

		psn = new SortedDictionary<int, string>();
		leaderboardGUI.GetComponent<Text> ().text = "LEADERBOARD\n" + leaderboardInfo;
		leaderboardInfo = "";
	}


}

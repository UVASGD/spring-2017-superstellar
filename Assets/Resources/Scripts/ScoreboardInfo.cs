using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardInfo : MonoBehaviour {

	public GameObject leaderboardGUI;

	List<KeyValuePair<int,string>> psn = new List<KeyValuePair<int,string>>();
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
			psn.Add(new KeyValuePair<int, string>(sm.score, sm.playerNameForDeath));
		}


		psn.Sort((x,y)=>y.Key.CompareTo(x.Key));


//		List<int> theValues = new List<int> (psn.Values);
//		for(int j = theValues.Count - 1; j >= 0; j--) {
//			int score = theValues[j];
//			leaderboardInfo += psn[score] + ": "+ score + "\n";
//		}
		for (int i = 0; i < psn.Count; i++) {
			leaderboardInfo += psn[i].Value + ": "+ psn[i].Key + "\n";
		}
		psn = new List<KeyValuePair<int,string>>();
		leaderboardGUI.GetComponent<Text> ().text = "LEADERBOARD\n" + leaderboardInfo;
		leaderboardInfo = "";
	}


}

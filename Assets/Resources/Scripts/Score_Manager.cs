using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score_Manager : MonoBehaviour {
	
	private GameObject player;
	public int score;
	public string playerNameForDeath;

	public GameObject word1;
	public GameObject word2;
	public GameObject word3;

	Text text1;
	public Text text2;
	Text text3;


	// Use this for initialization
	void Start () {

		player = this.gameObject.transform.parent.FindChild("Star").gameObject;
		text1 = word1.GetComponent<Text>();
		text2 = word2.GetComponent<Text>();
		text3 = word3.GetComponent<Text>();

		if (player.GetPhotonView ().isMine) {
			string name = PlayerPrefs.GetString ("PlayerName");
			player.GetPhotonView ().RPC("displayName", PhotonTargets.AllBufferedViaServer, name); // displayName(name);
			playerNameForDeath = PlayerPrefs.GetString ("PlayerName");
			PlayerPrefs.DeleteKey ("PlayerName");
			score = 0;
		}
	}

	// Update is called once per frame
	void Update () {
		Debug.Log (text3.text);
		playerNameForDeath = text3.text;

//		score = player.GetComponent<StarManager>().starMass;
		word1.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, 0);
		word2.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, 0);
		word3.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + 0.8f, 0);

		text1.text = "" + score;
		text2.text = "" + score;
	}


	[PunRPC]
	void displayName(string name) {
		text3.text = name;
	}
}

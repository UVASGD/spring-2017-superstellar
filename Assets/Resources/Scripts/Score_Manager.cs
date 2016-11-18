using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score_Manager : MonoBehaviour {

	public static int score;

	public GameObject word1;

	public GameObject word2;

	Text text1;

	Text text2;

	private GameObject player;

	// Use this for initialization
	void Start () {

		player = this.gameObject.transform.parent.FindChild("Star").gameObject;

		text1 = word1.GetComponent<Text>();
		text2 = word2.GetComponent<Text>();

		score = 0;

	}
	
	// Update is called once per frame
	void Update () {

		float x = player.transform.position.x;
		float y = player.transform.position.y - 0.16f*player.transform.localScale.x;

		transform.position = new Vector3 (x, y, 0);

		transform.localScale = player.transform.localScale;

		score = player.GetComponent<Shooting_Controls_edit>().starMass;
		text1.text = "" + score;
		text2.text = "" + score;
	}
}

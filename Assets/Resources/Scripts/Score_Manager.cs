using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score_Manager : MonoBehaviour {

	public static int score;

	Text text;

	public GameObject player;

	// Use this for initialization
	void Start () {

		text = GetComponent<Text>();

		score = 0;


	
	}
	
	// Update is called once per frame
	void Update () {

		score = player.GetComponent<Shooting_Controls_edit>().starMass;
		text.text = "" + score;
	}
}

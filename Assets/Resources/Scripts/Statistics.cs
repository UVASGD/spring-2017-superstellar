//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Statistics : MonoBehaviour {



	public GameObject word1;
	public GameObject word2;
	public GameObject word3;

//	public GameObject word1;
//	public GameObject word2;
//	public GameObject word3;

	// Use this for initialization
	void Start () {
//		Debug.Log ("hello entered the start");
//		Debug.Log ("dscore "+PlayerPrefs.GetInt("dscore"));
//		Debug.Log ("dname"+PlayerPrefs.GetString("dname"));
//		Debug.Log ("dclass"+PlayerPrefs.GetString("dclass"));
		string dscore = (PlayerPrefs.GetInt ("dscore")).ToString();
		string dname = (PlayerPrefs.GetString("dname")).ToString();
		string dclass = (PlayerPrefs.GetString("dclass")).ToString();


		PlayerPrefs.DeleteKey ("dscore");
		Debug.Log ("score: " + dscore);
//		this.gameObject.GetComponent<Text>.text="hello";
		word2.GetComponent<Text>().text = "Score: "+dscore;
		word3.GetComponent<Text>().text = "Name: "+dname;
		word1.GetComponent<Text>().text = "Class: "+dclass;


//		PlayerPrefs.DeleteKey ("dscore");
		PlayerPrefs.DeleteKey ("dname");
		PlayerPrefs.DeleteKey ("dclass");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

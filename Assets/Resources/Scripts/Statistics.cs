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
	public string replayName;

	void Start () {
		
		string dscore = (PlayerPrefs.GetInt ("dscore")).ToString();
		string dname = (PlayerPrefs.GetString("dname")).ToString();
		string dclass = (PlayerPrefs.GetString("dclass")).ToString();

		word2.GetComponent<Text>().text = "Score: "+dscore;
		word3.GetComponent<Text>().text = "Name: "+dname;
		word1.GetComponent<Text>().text = "Class: "+dclass;

		PlayerPrefs.DeleteKey ("dscore");
		PlayerPrefs.DeleteKey ("dclass");
		PlayerPrefs.DeleteKey ("dname");

		replayName = dname;

	}
}

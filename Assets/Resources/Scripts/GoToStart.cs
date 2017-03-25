using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		

	}

	public void GoToMain() {
		PlayerPrefs.DeleteKey ("dname");
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
	}

	public void ReplayGame () {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
	}

	// Update is called once per frame
	void Update () {
		
	}
}

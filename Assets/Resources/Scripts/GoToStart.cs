using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("hello entered the start");

	}

	public void OnStartGame () {
		Debug.Log ("hello entered the thing");
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
	}

	// Update is called once per frame
	void Update () {
		
	}
}

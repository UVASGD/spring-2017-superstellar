using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		

	}

	public void OnStartGame () {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
	}

	// Update is called once per frame
	void Update () {
		
	}
}

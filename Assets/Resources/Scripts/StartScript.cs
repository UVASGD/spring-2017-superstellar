using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {

	public GameObject Instructions;

	void Start() {
		Instructions.SetActive (false);
	}

	// Use this for initialization
	public void OnStartGame () {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
	}

	public void showControls() {
		GameObject.Find ("Title").SetActive (false);
		GameObject.Find ("Controls").SetActive (false);
		Instructions.SetActive (true);
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScript : MonoBehaviour {

	public AudioClip blowupSound;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	    source.PlayOneShot(blowupSound,.5f);
	}

	public void GoToMain() {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
	}

	public void ReplayGame () {
		PlayerPrefs.SetString ("PlayerName", GameObject.FindObjectOfType<Statistics> ().replayName);
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
	}
		
}

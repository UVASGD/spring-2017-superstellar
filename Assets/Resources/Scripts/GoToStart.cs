using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToStart : MonoBehaviour {

    // Use this for initialization
    public AudioClip blowupSound;
    private AudioSource source;

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

	// Update is called once per frame
	void Update () {
		
	}
}

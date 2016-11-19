using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {

	// Use this for initialization
	public void OnStartGame () {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
	}

}
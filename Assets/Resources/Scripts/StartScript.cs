using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScript : MonoBehaviour {

	public GameObject Instructions;
	public GameObject nameInputField;
	public GameObject Controls;
	public GameObject Play;
	public GameObject Title;

	void Start() {
		PlayerPrefs.SetString ("PlayerName","");
		Instructions.SetActive (false);
	}

	// Use this for initialization
	public void OnStartGame () {
		if (PlayerPrefs.GetString ("PlayerName") != "") {
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (1);
		}
	}

	public void showControls() {
		Title.SetActive (false);
		nameInputField.SetActive (false);
		Controls.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/mainmenu");
		Controls.GetComponent<Button> ().onClick.AddListener (delegate { BackToMain (); });
		Instructions.SetActive (true);
	}

	public void BackToMain() {
		Title.SetActive (true);
		nameInputField.SetActive (true);
		Controls.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/controls");
		Controls.GetComponent<Button> ().onClick.AddListener (delegate { showControls(); });
		Instructions.SetActive (false);
	}

}
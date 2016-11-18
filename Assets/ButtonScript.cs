using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	public void OnStartGame(){
		Debug.Log ("hello");
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
}
}

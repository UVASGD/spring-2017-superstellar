using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Follow_Player : MonoBehaviour {

	public GameObject player;

	public Image bar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float x = player.transform.position.x;
		float y = player.transform.position.y - 1.5f*player.transform.localScale.x;

		transform.position = new Vector3 (x, y, 0);

		transform.localScale = player.transform.localScale;

		float currentHealth = (int)player.GetComponent<Health_Management> ().Health;
		float maxHealth = (int)player.GetComponent<Shooting_Controls_edit> ().maxPlayerHealth;

		bar.fillAmount = currentHealth / maxHealth;
	}
}

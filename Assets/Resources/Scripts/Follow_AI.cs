using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Follow_AI : MonoBehaviour {
	private GameObject player;

	private Image bar;

	private float maxHealth;
	public Collider2D playerCheck;


	// Use this for initialization
	void Start () {

		player = this.gameObject.transform.parent.FindChild ("Body").gameObject;
		//Debug.Log (this.gameObject.transform.parent.GetComponentsInChildren<Transform> ().Length);
		Transform barParent = this.gameObject.transform.Find("Health Bar");
		bar = barParent.FindChild ("Health_Bar").GetComponent<Image>();
		maxHealth = (int)player.GetComponent<AI_Shooting> ().maxPlayerHealth;

	}

	// Update is called once per frame
	void Update () {

		float x = player.transform.position.x;
		float y = player.transform.position.y - 1.5f*player.transform.localScale.x;

		transform.position = new Vector3 (x, y, 0);
		playerCheck.transform.position = new Vector3 (x, player.transform.position.y, 0);


		transform.localScale = player.transform.localScale;

		float currentHealth = (int)player.GetComponent<Health_Management> ().Health;

		bar.fillAmount = currentHealth / maxHealth;
	}
}

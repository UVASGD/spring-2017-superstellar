using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Follow_Blazar : MonoBehaviour {
	private GameObject player;

	private Image bar;

	// Use this for initialization
	void Start () {

		player = this.gameObject.transform.parent.FindChild("B_Body").gameObject;

		//		Debug.Log (this.gameObject.transform.parent.GetComponentsInChildren<Transform> ().Length);

		Transform barParent = this.gameObject.transform.Find("Health Bar");
		bar = barParent.FindChild ("Health_Bar_BG").GetComponent<Image>();
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

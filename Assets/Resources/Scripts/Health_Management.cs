using UnityEngine;
using System.Collections;

public class Health_Management : MonoBehaviour {
	public GameObject player;

	public float Health;
	public int pointsToGive;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	public void giveDamage(int damage){
		Health -= damage;
		if (Health <= 0) {
			if (gameObject.tag == "Star_Point") {
				GetComponentInParent<Shooting_Controls_edit> ().destroyStarPoint (gameObject);
			} else if (gameObject.tag == "Player_Star") {
			} else {
				Destroy (gameObject);
				player.GetComponent<Shooting_Controls_edit> ().AddMass (pointsToGive);
			}
		}
	}
}

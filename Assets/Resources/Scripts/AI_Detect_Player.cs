using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Detect_Player : MonoBehaviour {


	public GameObject body;
	public bool inPursuit = false;

	void Start() {
		this.tag = body.tag;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		GameObject player;
		if (other.gameObject.GetComponent<Shooting_Controls_edit> () != null && !inPursuit) {
			player = other.gameObject;
			inPursuit = true;
			body.GetComponent<AI_Shooting> ().selectTarget (player);
		}
	}


}

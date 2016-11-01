﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionHandler : MonoBehaviour {

	public int damage_to_give;
		

	void OnTriggerEnter2D(Collider2D coll)
		{
			Debug.Log(coll.gameObject.name);
		}
		
	void OnCollisionStay2D(Collision2D other)
		{
			Debug.Log(other.gameObject.name);

		if (other.gameObject.tag == "BG_Stars") {
			if (gameObject.tag != "BG_Stars") {
				other.gameObject.GetComponent<Health_Management> ().giveDamage (damage_to_give);

			}

		}

		if (other.gameObject.tag == "Star_Proj" || other.gameObject.tag == "Star_Point") {
			other.gameObject.GetComponent<Health_Management>().giveDamage(damage_to_give);

		}

		if (other.gameObject.tag == "Player_Star") {
			other.gameObject.GetComponent<Health_Management>().giveDamage(damage_to_give);
			other.gameObject.GetComponent<Rigidbody2D> ().AddForce (other.gameObject.GetComponent<Rigidbody2D> ().velocity.normalized * -100);

		}

		}

	void giveMoreDamage(GameObject stubborn){
		stubborn.GetComponent<Health_Management> ().giveDamage (damage_to_give);
	}
	
}
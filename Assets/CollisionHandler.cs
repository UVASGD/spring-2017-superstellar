using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log(coll.gameObject.name);
	}

	void OnCollisionEnter(Collision coll)
	{
		Debug.Log(coll.gameObject.name);
	}

}

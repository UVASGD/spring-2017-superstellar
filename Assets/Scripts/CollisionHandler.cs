using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour {

		void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.gameObject.tag != "Player") {
//				Debug.Log (coll.gameObject.name);
				Destroy (coll.gameObject);
			}
		}
	
}
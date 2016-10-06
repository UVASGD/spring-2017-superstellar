using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour {

		void OnTriggerEnter2D(Collider2D coll)
		{
			Debug.Log(coll.gameObject.name);
		}
	
}
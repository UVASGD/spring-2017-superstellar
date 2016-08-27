using UnityEngine;

public class Controls: MonoBehaviour
{
	public GameObject projectile;
	public Vector2 mProjectileSpeed = new Vector2 (10f, 10f);
	public Vector2 mSpeed = new Vector2(15, 15);
	private Vector2 mMovement;

	void Update( )
	{
		if (Input.GetKeyDown (KeyCode.Alpha1))

			Shoot (1);

		if( Input.GetKeyDown( KeyCode.Alpha2 ) )

			Shoot (2);

		if (Input.GetKeyDown (KeyCode.Alpha3))

			Shoot (3);

		if( Input.GetKeyDown( KeyCode.Alpha4 ) )

			Shoot (4);

		if( Input.GetKeyDown( KeyCode.Alpha5 ) )

			Shoot (5);

//		if( Input.GetKeyUp( KeyCode.Space ) )
//			Debug.Log( "Space key was released." );
	}

	void Shoot(int point) {
//		SpriteRenderer sr = projectile.GetComponent<SpriteRenderer> (); sr.enabled = true;
		GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		SpriteRenderer sr = proj.GetComponent<SpriteRenderer> (); 
		sr.enabled = true;
		Rigidbody rb = proj.GetComponent<Rigidbody> ();
		rb.AddForce(Vector3.up * 100f);

//		cooldown = Time.time + attackSpeed;

	}
}
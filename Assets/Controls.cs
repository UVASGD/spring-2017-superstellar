using UnityEngine;

public class Controls: MonoBehaviour
{
	public GameObject projectile;

	private Vector3 righttop = new Vector3(0.95105651629f,0.30901699437f,0);
	private Vector3 rightbot = new Vector3(0.58778525229f,-0.80901699437f,0);
	private Vector3 leftbot = new Vector3(-0.58778525229f,-0.80901699437f,0);
	private Vector3 lefttop = new Vector3(-0.95105651629f,0.30901699437f,0);

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

		GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer> (); 
		SpriteRenderer sr = proj.GetComponent<SpriteRenderer> (); 
		Rigidbody rb = proj.GetComponent<Rigidbody> ();

		switch (point) {

			case 1:
				spr.sprite = Resources.Load<Sprite>("star_removed");
				sr.sprite = Resources.Load<Sprite>("star_top");
				rb.AddForce(Vector3.up * 100f);
				break;
			case 2:
				spr.sprite = Resources.Load<Sprite>("star_removed_right_top");
				sr.sprite = Resources.Load<Sprite>("star_righttop");
				rb.AddForce(righttop * 100f);
				break;
			case 3:
				spr.sprite = Resources.Load<Sprite>("star_removed_right_bottom");
				sr.sprite = Resources.Load<Sprite>("star_rightbot");
				rb.AddForce(rightbot * 100f);
				break;
			case 4:
				spr.sprite = Resources.Load<Sprite>("star_removed_left_bottom");
				sr.sprite = Resources.Load<Sprite>("star_leftbot");
				rb.AddForce(leftbot * 100f);
				break;
			case 5:
				spr.sprite = Resources.Load<Sprite>("star_removed_left_top");
				sr.sprite = Resources.Load<Sprite>("star_lefttop");
				rb.AddForce(lefttop * 100f);
				break;
		}
		sr.enabled = true;
	}
}
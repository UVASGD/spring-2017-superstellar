using UnityEngine;
using System.Collections;

public class Movement_Norm_Star : MonoBehaviour {
	
	//Inspector Variables
	private float playerSpeed = 2f; //speed player moves
	//private float turnSpeed  = 100; // speed player turns
	private Vector2 movTarget;
	private Vector2 dampSpeed = Vector2.zero;
	private float smoothTime = 0.5f;
	private Vector2 velTarget;

	public Vector2 velo = Vector2.zero;

	void Start()
	{
		
	}

	void Update () 
	{
		movTarget.x = transform.position.x;
		movTarget.y = transform.position.y;
		moveFunct(); // Player Movement
		rotate();//Player Turning
		//transform.localScale += new Vector3(Time.deltaTime / 20,Time.deltaTime / 20,0);

		velo = GetComponent<Rigidbody2D> ().velocity;

	}

	void moveFunct()
	{

		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {//Press up arrow key to move forward on the Y AXIS
			movTarget += new Vector2(0f, playerSpeed * Time.deltaTime / (transform.localScale.x));
			//velTarget = new Vector2 (0, playerSpeed / transform.localScale.x);
		} 
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {//Press up arrow key to move forward on the Y AXIS
			movTarget += new Vector2(0f, -playerSpeed * Time.deltaTime / (transform.localScale.x));
			//velTarget = new Vector2 (0, -playerSpeed / transform.localScale.x);
		} 
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {//Press up arrow key to move forward on the Y AXIS
			movTarget += new Vector2(playerSpeed * Time.deltaTime / (transform.localScale.x), 0f);
			//velTarget = new Vector2 (playerSpeed / transform.localScale.x,0);
		} 
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {//Press up arrow key to move forward on the Y AXIS
			movTarget += new Vector2(-playerSpeed * Time.deltaTime / (transform.localScale.x), 0f);
			//velTarget = new Vector2 (-playerSpeed / transform.localScale.x,0);
		} 
		if (! Input.GetKey(KeyCode.UpArrow) && ! Input.GetKey(KeyCode.DownArrow) && ! Input.GetKey(KeyCode.RightArrow) && ! Input.GetKey(KeyCode.LeftArrow)
			&& ! Input.GetKey(KeyCode.W) && ! Input.GetKey(KeyCode.A) && ! Input.GetKey(KeyCode.S) && ! Input.GetKey(KeyCode.D)){
			velTarget = new Vector2 (0f, 0f);
		}

		velTarget.x = (movTarget.x - transform.position.x)/Time.deltaTime;
		velTarget.y = (movTarget.y - transform.position.y)/Time.deltaTime;

		GetComponent<Rigidbody2D> ().velocity = Vector2.SmoothDamp (GetComponent<Rigidbody2D> ().velocity, velTarget, ref dampSpeed, smoothTime);
		//transform.position = Vector3.SmoothDamp (transform.position, movTarget, ref dampSpeed, smoothTime, playerSpeed / (transform.localScale.x));

	}

	void rotate() {
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = Input.mousePosition - pos;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
	}
}

using UnityEngine;
using System.Collections;

public class Movement_Norm_Star : Photon.MonoBehaviour {
	
	//Inspector Variables
	public float playerSpeed = 100; //speed player moves
	public float turnSpeed  = 10; // speed player turns
	private Vector3 movTarget;
	private Vector3 dampSpeed = Vector3.zero;
	public float smoothTime = 0.5f;

	public bool isControllable = false;
	public bool AssignAsTagObject = true;

	void Awake()
	{
		// PUN: automatically determine isControllable, if this GO has a PhotonView
		PhotonView pv = this.gameObject.GetComponent<PhotonView> ();
		if (pv != null) {
			isControllable = pv.isMine;
//			Debug.Log ("made mine");

			// The pickup demo assigns this GameObject as the PhotonPlayer.TagObject. This way, we can access this character (controller, position, etc) easily
			if (this.AssignAsTagObject) {
				pv.owner.TagObject = this.gameObject;
//				Debug.Log ("tagged mine");
			}
		}
	}

	void Start()
	{
		
	}

	void Update () 
	{
		if (this.photonView.isMine) {
			movTarget = transform.position;
			moveFunct(); // Player Movement
			rotate();//Player Turning
			//transform.localScale += new Vector3(Time.deltaTime / 20,Time.deltaTime / 20,0);
		}
	}

	void moveFunct()
	{

		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))//Press up arrow key to move forward on the Y AXIS
		{
			movTarget += new Vector3(0f, playerSpeed * Time.deltaTime / (transform.localScale.x * transform.localScale.x), 0f);


		}
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))//Press up arrow key to move forward on the Y AXIS
		{
			movTarget += new Vector3(0f, -playerSpeed * Time.deltaTime / (transform.localScale.x * transform.localScale.x), 0f);

		}
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))//Press up arrow key to move forward on the Y AXIS
		{
			movTarget += new Vector3(playerSpeed * Time.deltaTime / (transform.localScale.x * transform.localScale.x), 0f,0f);

		}
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))//Press up arrow key to move forward on the Y AXIS
		{
			movTarget += new Vector3(-playerSpeed * Time.deltaTime / (transform.localScale.x * transform.localScale.x), 0f,0f);

		}
		transform.position = Vector3.SmoothDamp (transform.position, movTarget, ref dampSpeed, smoothTime, playerSpeed / (transform.localScale.x * transform.localScale.x));

	}

	void rotate() {
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = Input.mousePosition - pos;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
	}
}

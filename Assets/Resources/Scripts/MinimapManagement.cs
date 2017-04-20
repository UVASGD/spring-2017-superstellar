using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManagement : MonoBehaviour {

	public GameObject PlayerPosIcon; //Prefab of MiniMap Player Icon
	public GameObject MiniMapGUI; // MiniMap Box
	public GameObject PlayerCamera; // PlayerCamera for size/position scaling

	private Transform starBody;
	private float playerScale;

	List<int> viewIDList = new List<int>(); //List of all players in room (get position)
	List<GameObject> playerList = new List<GameObject>(); //List of all players in room (get position)
	List<GameObject> iconList = new List<GameObject>(); // List containing all icons (set position)
	List<GameObject> bgStarList = new List<GameObject>(); // List of all BG Stars in room (get position)

	// Scaling factor for Actual -> MiniMap positions
	float scaledX;
	float scaledY;

	// Center of MiniMap
	Vector3 origin;


	// Photon Checks to only show local player minimap
	void OnEnable()
	{
		PhotonView pv = this.transform.parent.gameObject.GetPhotonView ();
		if (pv != null && !pv.isMine) {
			this.enabled = false;
			for (int i = 0; i < this.transform.childCount; ++i) {
				this.transform.GetChild (i).gameObject.SetActiveRecursively (false);
			}
		} 
	}
		
	void Start () {

		// Get all Players in room and initialize on minimap
		StarManager[] smList = GameObject.FindObjectsOfType<StarManager> ();
		foreach (StarManager sm in smList) {
			playerList.Add (sm.gameObject);
			viewIDList.Add (sm.gameObject.GetComponent<PhotonView>().viewID);
			GameObject icon = Instantiate (PlayerPosIcon, scaledPosition (sm.gameObject.transform.position), Quaternion.identity);
			icon.transform.SetParent (this.transform);
			icon.transform.position = scaledPosition (sm.gameObject.transform.position);
			if (sm.gameObject.GetPhotonView ().isMine) { 
				icon.GetComponent<SpriteRenderer> ().material = Resources.Load<Material> ("Materials/Local_Player_Icon");
			}
			iconList.Add (icon);
		}

		// Actual Map Bounds
		float mapX = GameObject.Find ("Background").transform.localScale.x;
		float mapY = GameObject.Find ("Background").transform.localScale.y;

		// MiniMap Bounds
		float minimapX = MiniMapGUI.GetComponent<RectTransform> ().rect.width  *  this.GetComponent<RectTransform>().localScale.x;
		float minimapY = MiniMapGUI.GetComponent<RectTransform> ().rect.height  *  this.GetComponent<RectTransform>().localScale.y;


		// Calculate Scaling Factor (NEED TO CHECK MATH...Maybe don't hardcode it...)
		scaledX = minimapX/mapX;
		scaledY = minimapY/mapY;

		starBody = this.transform.parent.Find("Star");
		if (starBody == null) {
			Debug.Log ("No star body");
		}

	}
	
	// Update is called once per frame
	void Update () {

		// check if players have entered or left game
		updateNumPlayers ();
//		updateNumBGStars ();
			
		// update MiniMap position
		origin = MiniMapGUI.transform.position;
		playerScale = starBody.transform.localScale.x * 2.4f;


		// 
		foreach (GameObject p in playerList) {
			GameObject icon = iconList [playerList.IndexOf (p)];
			icon.transform.position = scaledPosition (p.transform.position);
		}
			
		// show BG Stars on MiniMap (removed because of lag issues)
//		displayBGStars ();

	}
		


	void displayBGStars() {


		GameObject[] bgsList = GameObject.FindGameObjectsWithTag ("BG_Stars");
		foreach (GameObject bgs in bgsList) {
			GameObject icon = Instantiate (PlayerPosIcon, scaledPosition (bgs.gameObject.transform.position), Quaternion.identity);
			icon.transform.SetParent (this.transform);
			icon.transform.position = scaledPosition (bgs.gameObject.transform.position);
			icon.GetComponent<SpriteRenderer> ().material = Resources.Load<Material> ("Materials/BG_Star_Icon");
			bgStarList.Add (icon);
		}

	}

	Vector3 scaledPosition(Vector3 original) {
		float x = (original.x * scaledX * playerScale) + origin.x;
		float y = (original.y * scaledY * playerScale) + origin.y;
		//float x = (original.x * scaledX) + origin.x;
		//float y = (original.y * scaledY) + origin.y;
		Vector3 res = new Vector3 (x,y,0);
		return res;
	}


	void updateNumPlayers() {
		if (GameObject.FindObjectsOfType<StarManager> ().Length > playerList.Count) {
			StarManager[] smList = GameObject.FindObjectsOfType<StarManager> ();
			foreach (StarManager sm in smList) {
				bool oldplayer = false;
				foreach (GameObject g in playerList) {
					if (g.GetPhotonView ().ownerId == sm.photonView.ownerId) {
						oldplayer = true;
					}
				}
				if (!oldplayer) {
					playerList.Add (sm.gameObject);
					viewIDList.Add (sm.gameObject.GetComponent<PhotonView>().viewID);
					GameObject icon = Instantiate (PlayerPosIcon, scaledPosition (sm.gameObject.transform.position), Quaternion.identity);
					icon.transform.SetParent (this.transform);
					icon.transform.position = scaledPosition (sm.gameObject.transform.position);
					if (sm.gameObject.GetPhotonView ().isMine) { 
						icon.GetComponent<SpriteRenderer> ().material = Resources.Load<Material> ("Materials/Local_Player_Icon");
					}
					iconList.Add (icon);
				}

			}
		} else if (GameObject.FindObjectsOfType<StarManager> ().Length < playerList.Count) {
			StarManager[] smList = GameObject.FindObjectsOfType<StarManager> ();
			foreach (int viewID in viewIDList) {
				bool playerAlive = false;
				foreach (StarManager sm in smList) {
					if (viewID == sm.photonView.viewID) {
						playerAlive = true;
					}
				}
				if (!playerAlive) {
					GameObject player = playerList[viewIDList.IndexOf (viewID)];
					GameObject icon = iconList[viewIDList.IndexOf (viewID)];
					Destroy (icon);
					iconList.Remove (icon);
					playerList.Remove (player);
					viewIDList.Remove (viewID);
				}
			}
		}
	}



//	void updateNumBGStars() {
//		if (GameObject.FindGameObjectsWithTag ("BG_Stars").Length > bgStarList.Count) {
//			GameObject[] bgsList = GameObject.FindGameObjectsWithTag ("BG_Stars");
//			foreach (GameObject bgs in bgsList) {
//				bool oldplayer = false;
//				foreach (GameObject g in bgsList) {
//					if (g.GetPhotonView ().ownerId == bgs.GetPhotonView().ownerId) {
//						oldplayer = true;
//					}
//				}
//				if (!oldplayer) {
//					bgStarList.Add (bgs);
//					GameObject icon = Instantiate (PlayerPosIcon, scaledPosition (bgs.transform.position), Quaternion.identity);
//					icon.transform.SetParent (this.transform);
//					icon.transform.position = scaledPosition (bgs.transform.position);
//					if (bgs.GetPhotonView ().isMine) {
//						icon.GetComponent<SpriteRenderer> ().material = Resources.Load<Material> ("Materials/BG_Star_Icon");
//					}
//					iconList.Add (icon);
//				}
//
//			}
//		} else if (GameObject.FindGameObjectsWithTag ("BG_Stars").Length < bgStarList.Count) {
//			GameObject[] bgsList = GameObject.FindGameObjectsWithTag ("BG_Stars");
//			foreach (GameObject bgs in bgsList) {
//				bool oldplayer = false;
//				foreach (GameObject g in playerList) {
//					if (g.GetPhotonView ().ownerId == bgs.GetPhotonView().ownerId) {
//						oldplayer = true;
//					}
//				}
//				if (!oldplayer) {
//					GameObject icon = iconList[bgStarList.IndexOf (bgs)];
//					Destroy (icon);
//					bgStarList.Remove (bgs);
//				}
//
//			}
//		}
//
//	}

}

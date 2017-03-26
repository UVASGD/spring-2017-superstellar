using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManagement : MonoBehaviour {

	public GameObject PlayerPosIcon;
	private GameObject miniMap;
	List<GameObject> playerList = new List<GameObject>();
	List<GameObject> iconList = new List<GameObject>();

	List<GameObject> bgStarList = new List<GameObject>();

	float scaledX;
	float scaledY;

	Vector3 origin;

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

	// Use this for initialization
	void Start () {
		StarManager[] smList = GameObject.FindObjectsOfType<StarManager> ();
		foreach (StarManager sm in smList) {
			playerList.Add(sm.gameObject);
			Vector3 playerPosition = scaledPosition (sm.gameObject.transform.position);
			GameObject icon = Instantiate (PlayerPosIcon, playerPosition , Quaternion.identity) as GameObject;
			icon.transform.SetParent (this.transform);
			icon.transform.position = scaledPosition (sm.gameObject.transform.position);
			if (sm.gameObject.GetPhotonView ().isMine) { 
				icon.GetComponent<SpriteRenderer> ().material = Resources.Load<Material> ("Materials/Local_Player_Icon");
			}
			iconList.Add (icon);
		}

		miniMap = GameObject.Find("minimap_gui");
		miniMap.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";
		float mapX = GameObject.Find ("Background").transform.localScale.x;
		float mapY = GameObject.Find ("Background").transform.localScale.y;

		float minimapX = GameObject.Find ("minimap_gui").transform.localScale.x;
		float minimapY = GameObject.Find("minimap_gui").transform.localScale.y;

//		scaledX = mapX / minimapX;
//		scaledY = mapY / minimapY;

		scaledX = mapX;
		scaledY = mapY;

	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log (GameObject.FindObjectsOfType<StarManager> ().Length);
		//Debug.Log (playerList.Count);
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
					GameObject icon = Instantiate (PlayerPosIcon, scaledPosition (sm.gameObject.transform.position), Quaternion.identity);
					icon.transform.SetParent (this.transform);
					icon.transform.position = scaledPosition (sm.gameObject.transform.position);
					if (sm.gameObject.GetPhotonView ().isMine) {
						icon.GetComponent<SpriteRenderer> ().material = Resources.Load<Material> ("Materials/Local_Player_Icon");
					}
					iconList.Add (icon);
				}

			}
		}
			
		origin = GameObject.Find("minimap_gui").transform.position;
//		Debug.Log (origin);

		foreach (GameObject p in playerList) {
//			Debug.Log (p.name + ": " +p.transform.position);
			GameObject icon = iconList [playerList.IndexOf (p)];
			icon.transform.position = scaledPosition (p.transform.position);
//			Debug.Log (icon.transform.position);
		}

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
		float x = (3*original.x/scaledX) + origin.x;
		float y = (3*original.y/scaledY) + origin.y;
		Vector3 res = new Vector3 (x,y,0);
		return res;
	}


}

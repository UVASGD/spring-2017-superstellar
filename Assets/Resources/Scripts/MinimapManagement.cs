using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManagement : MonoBehaviour {

	public GameObject PlayerPosIcon;
	List<GameObject> playerList = new List<GameObject>();
	List<GameObject> iconList = new List<GameObject>();
	float mapX;
	float mapY;
	Vector3 origin;

	void OnEnable()
	{
		PhotonView pv = this.transform.parent.gameObject.GetPhotonView ();
		if (pv != null && !pv.isMine) {
			this.enabled = false;
			for( int i = 0; i < this.transform.childCount; ++i )
			{
				this.transform.GetChild(i).gameObject.SetActiveRecursively(false);
			}
		} 
	}

	// Use this for initialization
	void Start () {
		StarManager[] smList = GameObject.FindObjectsOfType<StarManager> ();
		foreach (StarManager sm in smList) {
			playerList.Add(sm.gameObject);
			GameObject icon = Instantiate (PlayerPosIcon, scaledPosition (sm.gameObject.transform.position), Quaternion.identity);
			icon.transform.SetParent (this.transform);
			icon.transform.position = scaledPosition (sm.gameObject.transform.position);
			iconList.Add (icon);
//			Debug.Log (sm.gameObject.name);
		}

		mapX = GameObject.Find ("Background").transform.localScale.x;
		mapY = GameObject.Find ("Background").transform.localScale.y;

	}
	
	// Update is called once per frame
	void Update () {
		origin = GameObject.Find("minimap_gui").transform.position;
		Debug.Log (origin);

		foreach (GameObject p in playerList) {
			Debug.Log (p.name + ": " +p.transform.position);
			GameObject icon = iconList [playerList.IndexOf (p)];
			icon.transform.position = scaledPosition (p.transform.position);
			Debug.Log (icon.transform.position);
		}

	}
		

	Vector3 scaledPosition(Vector3 original) {
		float x = (original.x/mapX) + origin.x;
		float y = (original.y/mapY) + origin.y;
		Vector3 res = new Vector3 (x,y,0);
		return res;
	}


}

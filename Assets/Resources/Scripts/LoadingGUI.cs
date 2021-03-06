﻿using UnityEngine;
using System.Collections;

public class LoadingGUI : MonoBehaviour 
{
	public GUISkin Skin;

	void OnGUI()
	{
		if( Skin != null )
		{
			GUI.skin = Skin;
		}

		float width = 400;
		float height = 100;

		Rect centeredRect = new Rect( ( Screen.width - width ) / 2, ( Screen.height - height ) / 2, width, height );

		GUILayout.BeginArea( centeredRect, GUI.skin.box );
		{
			GUILayout.Label( "Entering the Galaxy " + GetConnectingDots());
		}
		GUILayout.EndArea();

		if( PhotonNetwork.inRoom )
		{
			enabled = false;
			Renderer bg = GameObject.Find ("Background").GetComponent<Renderer> ();
			bg.material.mainTextureScale = new Vector2 (15,15);
		}
	}

	string GetConnectingDots()
	{
		string str = "";
		int numberOfDots = Mathf.FloorToInt( Time.timeSinceLevelLoad * 3f % 4 );

		for( int i = 0; i < numberOfDots; ++i )
		{
			str += " .";
		}

		return str;
	}
}

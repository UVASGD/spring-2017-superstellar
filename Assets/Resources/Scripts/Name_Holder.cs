using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name_Holder : MonoBehaviour {
	public void CharacterField(string inputFieldString){
		PlayerPrefs.SetString ("PlayerName", inputFieldString);
	}
}

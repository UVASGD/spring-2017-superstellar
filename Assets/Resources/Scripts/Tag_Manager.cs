using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tag_Manager : MonoBehaviour {

	private SerializedObject myTagManager;
	private SerializedProperty tagsProp;
	private SerializedProperty layersProp;

	void Start() {
		myTagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		tagsProp = myTagManager.FindProperty("tags");
		layersProp = myTagManager.FindProperty("layers");
	}


	public void addTag(string myNewTag, string myLayer = "Default", int layerIndex = 0, bool addLayer = false) {
		bool found = false;
		for (int i = 0; i < tagsProp.arraySize; i++) {
			SerializedProperty t = tagsProp.GetArrayElementAtIndex (i);
			if (t.stringValue.Equals (myNewTag)) {
				found = true;
				break;
			}
		
		}

		if (!found) {
			tagsProp.InsertArrayElementAtIndex (0);
			SerializedProperty n = tagsProp.GetArrayElementAtIndex (0);
			n.stringValue = myNewTag;
		}

		if (addLayer) {
			SerializedProperty sp = layersProp.GetArrayElementAtIndex (layerIndex);
			if (sp != null) {
				sp.stringValue = myLayer;
			}
	
		}

		myTagManager.ApplyModifiedProperties ();
	}

}

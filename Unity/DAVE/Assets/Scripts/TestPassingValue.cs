using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPassingValue : MonoBehaviour {

	//List<GameObject> systemBoxes;
	public GameObject systemName;
	ChangeTextMeshText systemBox_script;
	string i = "yeah!!!";
	void Start () {
		//foreach (Transform child in transform) {
		//	systemBoxes.Add (child.gameObject);
		//}

		systemBox_script = systemName.GetComponent<ChangeTextMeshText>();

	}

	// Update is called once per frame
	void Update () {
		systemBox_script.className = i;
		systemBox_script.display();
	}
}

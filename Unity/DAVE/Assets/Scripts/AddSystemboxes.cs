using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSystemboxes : MonoBehaviour {
	public GameObject systemBox;
	public GameObject systemName;

	public ChangeTextMeshText text;

	public int p = 0;
	string[] nameList = {"a", "b", "c", "d", "e"};
	// Use this for initialization
	void Start () {
		CreateBoxes ();
	}

	void CreateBoxes(){
		foreach (string names in nameList) {

			Vector3 v = new Vector3 (p, 0, 0);
			GameObject tmp = (GameObject) Instantiate(
				systemBox, 
				v, 
				this.transform.rotation
			);

			GameObject tmpName = (GameObject)Instantiate (systemName, v, this.transform.rotation);
			text = GetComponentInChildren<ChangeTextMeshText> ();
			tmp.name = names;
			text.className = names;
			p = p + 2;
		
		}
	}
	
	// Update is called once per frame
	void Update () {
		

	}
}

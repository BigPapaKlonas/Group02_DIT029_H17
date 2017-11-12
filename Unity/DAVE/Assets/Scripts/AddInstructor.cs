using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddInstructor : MonoBehaviour {

	public Button button;
	// Use this for initialization
	void Start () {
		button.onClick.AddListener (OnClick);
	}

	void OnClick()
	{
		Coordinator.R.Db ("root").Table ("instructors").Insert (Coordinator.R.Array(
			Coordinator.R.HashMap("name", "Leona")
		)).Run(Coordinator.conn);
	}

}

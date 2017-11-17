using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayDiagram : MonoBehaviour {

	private Button button;

	// Use this for initialization
	void Start () 
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);

		if (Coordinator.coordinator.GetInstructorBool ()) {
			button.gameObject.SetActive (false);
		}
	}

	void OnClick() 
	{
		Coordinator.coordinator.Publish (
			"root/" + 
			Coordinator.coordinator.GetInstructor () + "/" + 
			Coordinator.coordinator.GetDiagram (),
			Coordinator.coordinator.GetSessionJson (),
			true
		);

		Coordinator.coordinator.Publish (
			"root/" + 
			Coordinator.coordinator.GetInstructor () + "/" + 
			Coordinator.coordinator.GetDiagram () + "/nodes",
			"init_diagram",
			true
		);
	}
}

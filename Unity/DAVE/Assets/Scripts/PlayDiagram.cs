using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayDiagram : MonoBehaviour {

	private Button button;
    private ConnectionManager coordinator = ConnectionManager.coordinator;

    // Use this for initialization
    void Start () 
	{
        button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);

		if (ConnectionManager.coordinator.GetInstructorBool () == false) {
			button.gameObject.SetActive (false);
		}
	}

     void OnClick() 
	{
        ConnectionManager.coordinator.Publish (
			"root/" + 
			ConnectionManager.coordinator.GetInstructor () + "/" + 
			ConnectionManager.coordinator.GetRoom (),
			ConnectionManager.coordinator.GetSessionJson (),
			true
		);

		ConnectionManager.coordinator.Publish (
			"root/" + 
			ConnectionManager.coordinator.GetInstructor () + "/" + 
			ConnectionManager.coordinator.GetRoom () + "/nodes",
			"init_diagram",
			true
		);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayDiagram : MonoBehaviour {

	private Button button;
    private SubscribingStudents subscribingStudents;
    private ConnectionManager coordinator = ConnectionManager.coordinator;

    // Use this for initialization
    void Start () 
	{
        subscribingStudents = GetComponentInParent<SubscribingStudents>();

        button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);

		if (ConnectionManager.coordinator.GetInstructorBool () == false) {
			button.gameObject.SetActive (false);
		}
	}

    void DisableSubscribingStudents()
    {
        coordinator.Unsubscribe("root/" + coordinator.GetInstructor() + "/" +
            coordinator.GetDiagram() + "/students");
        subscribingStudents.enabled = false;
    }


    void OnClick() 
	{

        DisableSubscribingStudents();

        ConnectionManager.coordinator.Publish (
			"root/" + 
			ConnectionManager.coordinator.GetInstructor () + "/" + 
			ConnectionManager.coordinator.GetDiagram (),
			ConnectionManager.coordinator.GetSessionJson (),
			true
		);

		ConnectionManager.coordinator.Publish (
			"root/" + 
			ConnectionManager.coordinator.GetInstructor () + "/" + 
			ConnectionManager.coordinator.GetDiagram () + "/nodes",
			"init_diagram",
			true
		);
	}
}

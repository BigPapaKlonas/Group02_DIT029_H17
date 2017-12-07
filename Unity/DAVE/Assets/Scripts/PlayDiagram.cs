using UnityEngine;
using UnityEngine.UI;

public class PlayDiagram : MonoBehaviour {

	private Button button;
    ConnectionManager coordinator = ConnectionManager.coordinator;

    // Use this for initialization
    void Start () 
	{
        button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);

		if (coordinator.GetInstructorBool () == false) {
			button.gameObject.SetActive (false);
		}
	}

     void OnClick() 
	{
        // Iterates through the selected JSONS and publishes them to the MQTT broker
        while (coordinator.GetSelectedJsons().Count > 0)
        {
            var jsonStruct = coordinator.GetSelectedJsons().Dequeue();
            
            coordinator.Publish(
            "root/" + coordinator.GetInstructor() + "/" +
            coordinator.GetRoom() + "/" + jsonStruct.diagramType,
            jsonStruct.json,
            true
        );
        }
        
		coordinator.Publish (
			"root/" + coordinator.GetInstructor () + "/" + 
			coordinator.GetRoom () + "/nodes",
			"init_diagram",
			true
		);

        gameObject.SetActive(false);
	}
}

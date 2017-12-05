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
        // Iterates through the selected JSONS and publishes them to the MQTT broker
        while (ConnectionManager.coordinator.GetSelectedJsons().Count > 0)
        {
            var jsonStruct = ConnectionManager.coordinator.GetSelectedJsons().Dequeue();
            
            ConnectionManager.coordinator.Publish(
            "root/" +
            ConnectionManager.coordinator.GetInstructor() + "/" +
            ConnectionManager.coordinator.GetRoom() + "/" +
            jsonStruct.diagramType,
            jsonStruct.json,
            true
        );
        }
        
		ConnectionManager.coordinator.Publish (
			"root/" + 
			ConnectionManager.coordinator.GetInstructor () + "/" + 
			ConnectionManager.coordinator.GetRoom () + "/nodes",
			"init_diagram",
			true
		);

        gameObject.SetActive(false);
	}
}

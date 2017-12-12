using UnityEngine;
using UnityEngine.UI;

public class PublishDiagram : MonoBehaviour
{

    public GameObject player;
    private Button button;
    ConnectionManager coordinator = ConnectionManager.coordinator;

	private GameObject uploadBtn;
	private bool maxUploads;

    // Use this for initialization
    void Start()
    {
		uploadBtn = GameObject.Find ("UploadBtn");
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        if (ConnectionManager.auth == false)
        {
            button.gameObject.SetActive(false);
        }
    }

    void OnClick()
    {
        // Iterates through the selected JSONS and publishes them to the MQTT broker
        while (coordinator.GetSelectedJsons().Count > 0)
        {
            var jsonStruct = coordinator.GetSelectedJsons().Dequeue();

            // If the diagram is a sequence diagram, it is retained for the simulation to work
            if (jsonStruct.diagramType == "sequence_diagram")
            {

                string ssdRoom = "root/" + coordinator.GetInstructor() + "/" +
                    coordinator.GetRoom() + "/sequence_diagram";
                coordinator.Publish(
                    "root/newdiagram",
                    ssdRoom.ToLower(),
                    false
                );

                coordinator.Publish(
                "root/" + coordinator.GetInstructor() + "/" +
                coordinator.GetRoom() + "/" + jsonStruct.diagramType + "/diagram",
                jsonStruct.json,
                true);
            }

            coordinator.Publish(
            "root/" + coordinator.GetInstructor() + "/" +
            coordinator.GetRoom() + "/" + jsonStruct.diagramType,
            jsonStruct.json,
            false
            );
        }

		if (maxUploads == false) 
		{
			uploadBtn.GetComponent<Button>().interactable = true;
		}

        gameObject.SetActive(false);
    }

	public void SetMaxUploads(bool max)
	{
		maxUploads = max;
	}
}

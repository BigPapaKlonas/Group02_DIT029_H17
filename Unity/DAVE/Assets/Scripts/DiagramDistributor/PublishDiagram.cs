using UnityEngine;
using UnityEngine.UI;

public class PublishDiagram : MonoBehaviour
{

    public GameObject player;
    private Button button;
    ConnectionManager coordinator = ConnectionManager.coordinator;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        if (coordinator.GetInstructorBool() == false)
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

            if (jsonStruct.diagramType == "sequence_diagram")
            {

                string ssdRoom = "root/" + coordinator.GetInstructor() + "/" +
                    coordinator.GetRoom() + "/sequence_diagram";
                coordinator.Publish(
                    "root/newdiagram",
                    ssdRoom,
                    false
                );
            }

            // If the diagram is a sequence diagram, it is retained for the simulation to work
            if (jsonStruct.diagramType == "sequence_diagram")
            {
                coordinator.Publish(
                "root/" + coordinator.GetInstructor() + "/" +
                coordinator.GetRoom() + "/" + jsonStruct.diagramType,
                jsonStruct.json,
                true);
            }
            else
            {
                coordinator.Publish(
                "root/" + coordinator.GetInstructor() + "/" +
                coordinator.GetRoom() + "/" + jsonStruct.diagramType,
                jsonStruct.json,
                false
                );
            }
        }

        gameObject.SetActive(false);
    }
}

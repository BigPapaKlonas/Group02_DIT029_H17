using System;
using UnityEngine;
using UnityEngine.UI;

public class DiagramBroker : MonoBehaviour
{
    private Button uplButton;
    ConnectionManager coordinator = ConnectionManager.coordinator;
    float houseOffset = -50;                         // offset used to position house "districts"

    private void Start()
    {
        uplButton = GameObject.Find("Upload").GetComponent<Button>();

        coordinator.Subscribe(
            "root/" + coordinator.GetInstructor() + "/" +
            coordinator.GetRoom() + "/#"
        );
    }

    private void Update()
    {
        if (ConnectionManager.classDiagramQueue.Count > 0)
        {
            Debug.Log("Dequeued: " + ConnectionManager.classDiagramQueue.Peek());
            JsonParser parser = new JsonParser(ConnectionManager.classDiagramQueue.Dequeue());
            RenderClassDiagram(parser.ParseClass(), houseOffset);
            houseOffset += 40;
        }
        else if (ConnectionManager.deploymentDiagramQueue.Count > 0)
        {
            Debug.Log("Dequeued: " + ConnectionManager.deploymentDiagramQueue.Peek());
            JsonParser parser = new JsonParser(ConnectionManager.deploymentDiagramQueue.Dequeue());
        }
    }

    /*
        switch (jsonParser.GetDiagramType())
        {
            case "sequence_diagram":
                Debug.Log("Sequence");
                RenderSequence(jsonParser.ParseSequence());
                break;
            case "class_diagram":
                Debug.Log("Class");
                RenderClassDiagram(jsonParser.ParseClass(), houseOffset);
                break;
            case "deployment_diagram":
                Debug.Log("deployment");
                RenderDeployment(jsonParser.ParseDeployment());
                break;
            default:
                Debug.Log("Invalid diagram type");
                break;
        }
    **/

    public void RenderSequence(JSONSequence JSONSequence)
    {
        RenderSystemBoxes(JSONSequence);
        RenderMessages(JSONSequence);
    }

    public void RenderClassDiagram(JSONClass JSONClass, float houseOffset)
    {
        string id = Guid.NewGuid().ToString("N");
        RenderClasses(JSONClass, id, houseOffset);
        RenderRelationships(JSONClass, id);
    }

    public void RenderDeployment(JSONDeployment JSONDeployment)
    {
        //Placeholder
    }

    private void RenderSystemBoxes(JSONSequence JSONSequence)
    {
        uplButton.GetComponent<RenderSystemBoxes>().CreateSystemBoxes(JSONSequence);
    }

    private void RenderMessages(JSONSequence JSONSequence)
    {
        uplButton.GetComponent<StartMessages>().NewMessage(JSONSequence);
    }

    public void RenderClasses(JSONClass JSONClass, string id, float offset)
    {
        uplButton.GetComponent<RenderClasses>().AddHouse(JSONClass, id, offset);
    }

    public void RenderRelationships(JSONClass JSONClass, string id)
    {
        uplButton.GetComponent<RenderClassRelationship>().AddRelationship(JSONClass, id);
    }
}


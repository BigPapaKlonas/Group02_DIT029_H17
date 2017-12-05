using System;
using UnityEngine;

public class DiagramBroker : MonoBehaviour
{
    ConnectionManager coordinator = ConnectionManager.coordinator;
    float houseOffset = -50;                         // offset used to position house "districts"

    private void Start()
    {
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
        gameObject.GetComponent<RenderSystemBoxes>().CreateSystemBoxes(JSONSequence);
    }

    private void RenderMessages(JSONSequence JSONSequence)
    {
        gameObject.GetComponent<StartMessages>().NewMessage(JSONSequence);
    }

    public void RenderClasses(JSONClass JSONClass, string id, float offset)
    {
        gameObject.GetComponent<RenderClasses>().AddHouse(JSONClass, id, offset);
    }

    public void RenderRelationships(JSONClass JSONClass, string id)
    {
        gameObject.GetComponent<RenderClassRelationship>().AddRelationship(JSONClass, id);
    }
}


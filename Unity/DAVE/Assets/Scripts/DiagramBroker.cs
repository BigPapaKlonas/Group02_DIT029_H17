using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

public class DiagramBroker : MonoBehaviour
{
    ConnectionManager coordinator = ConnectionManager.coordinator;
    float houseOffset = -50;                         // offset used to position house "districts"

    //Queues of received JSON strings.
    public static Queue<String> classDiagramQueue = new Queue<String>();
    public static Queue<String> deploymentDiagramQueue = new Queue<String>();
    public static Queue<String> sequenceDiagramQueue = new Queue<String>();


    public GameObject ssdSpawnerSpawner;
    private SSDInit ssdInit;

    private void Start()
    {
        // Assign handler for handling the receiving messages
        coordinator.GetMqttClient().MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

        // Subscribes to choosen intructor's room 
        coordinator.Subscribe(
            "root/" + coordinator.GetInstructor() + "/" +
            coordinator.GetRoom() + "/class_diagram"
        );

        // Subscribes to choosen intructor's room 
        coordinator.Subscribe(
            "root/" + coordinator.GetInstructor() + "/" +
            coordinator.GetRoom() + "/deployment_diagram"
        );

        // Subscribes to choosen intructor's room 
        coordinator.Subscribe(
            "root/" + coordinator.GetInstructor() + "/" +
            coordinator.GetRoom() + "/sequence_diagram"
        );

        ssdInit = ssdSpawnerSpawner.GetComponent<SSDInit>();

    }

    private void Update()
    {   //Dequeues and render the diagrams
        if (classDiagramQueue.Count > 0)
        {
            Debug.Log("Dequeued: " + classDiagramQueue.Peek());
            JsonParser parser = new JsonParser(classDiagramQueue.Dequeue());
            RenderClassDiagram(parser.ParseClass(), houseOffset);
            houseOffset += 40;
        }
        else if (deploymentDiagramQueue.Count > 0)
        {
            Debug.Log("Dequeued: " + deploymentDiagramQueue.Peek());
            JsonParser parser = new JsonParser(deploymentDiagramQueue.Dequeue());
            RenderDeployment(parser.ParseDeployment());
        }
        else if (sequenceDiagramQueue.Count > 0)
        {
            JsonParser parser = new JsonParser(sequenceDiagramQueue.Dequeue());
            StartCoroutine(RenderDeploymentConnections(parser.ParseSequence()));
        }
    }


    // Handler that gets received messages from subscribed topics
    void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received\r\n" + "Topic: " + e.Topic + "\r\n" + "Message: " +
            System.Text.Encoding.UTF8.GetString(e.Message));

        CheckReceived(e);
    }

    // Method checks if received message should be added to the queue that will be animated
    private void CheckReceived(MqttMsgPublishEventArgs e)
    {
        if (e.Topic == coordinator.GetParentTopic() + "/class_diagram")          // Checks topic
        {
            String payload = System.Text.Encoding.UTF8.GetString(e.Message);

            // Checks if payload is a valid JSON and of valid type
            if (IsValidJson(payload) && IsValidDiagramType(payload))
            {
                Debug.Log("Class diagram JSON received, verified and queued");
                classDiagramQueue.Enqueue(payload);             // Adds payload (JSON) to the queue
            }
        }
        else if (e.Topic == coordinator.GetParentTopic() + "/deployment_diagram")
        {
            String payload = System.Text.Encoding.UTF8.GetString(e.Message);
            Debug.Log("Deployment diagram JSON received, verified and queued");
            if (IsValidJson(payload) && IsValidDiagramType(payload))
            {
                deploymentDiagramQueue.Enqueue(payload);
            }
        }
        else if(e.Topic == coordinator.GetParentTopic() + "/sequence_diagram")
        {
            String payload = System.Text.Encoding.UTF8.GetString(e.Message);
            string[] array = payload.Split(' ');
            if(array[1] == "size")
            {
                string ssdRoom = "root/" + coordinator.GetInstructor() + "/" +
                    coordinator.GetRoom() + "/sequence_diagram";

                ssdInit.room = ssdRoom;
                ssdInit.size = int.Parse(array[0]);
                ssdInit.spawnSpawner = true;

                coordinator.Unsubscribe(ssdRoom);
            }

            if (IsValidJson(payload) && IsValidDiagramType(payload))
            {
                sequenceDiagramQueue.Enqueue(payload);
            }
        }
    }

    public void RenderClassDiagram(JSONClass JSONClass, float houseOffset)
    {
        string id = Guid.NewGuid().ToString("N");
        RenderClasses(JSONClass, id, houseOffset);
        RenderRelationships(JSONClass, id);
    }

    public void RenderDeployment(JSONDeployment JSONDeployment)
    {
        RenderDevices(JSONDeployment);
    }

    private void RenderSystemBoxes(JSONSequence JSONSequence)
    {
        gameObject.GetComponent<RenderSystemBoxes>().CreateSystemBoxes(JSONSequence);
    }

    public void RenderClasses(JSONClass JSONClass, string id, float offset)
    {
        gameObject.GetComponent<RenderClasses>().AddHouse(JSONClass, id, offset);
    }

    public void RenderRelationships(JSONClass JSONClass, string id)
    {
        gameObject.GetComponent<RenderClassRelationship>().AddRelationship(JSONClass, id);
    }

    private void RenderDevices(JSONDeployment JSONDeployment)
    {
        gameObject.GetComponent<RenderDevices>().CreateDevices(JSONDeployment);
    }

    private IEnumerator RenderDeploymentConnections(JSONSequence JSONSequence)
    {
        print(Time.time);
        yield return new WaitForSeconds(1.5f);
        print(Time.time);
        gameObject.GetComponent<FindDeploymentConnections>().NewMessage(JSONSequence);
    }


    // Source: https://goo.gl/n89LoF
    private bool IsValidJson(string strInput)
    {
        strInput = strInput.Trim();
        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
            (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(strInput);
                obj.Equals(obj); //Could not suppress warning so this prevents the error message
                return true;
            }
            catch (JsonReaderException jex) //Exception in parsing json
            {
                Debug.Log(jex.Message);
                return false;
            }
            catch (Exception ex) //some other exception
            {
                Debug.Log(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // Checks if the json is either a sequence, class or a deployment diagram
    private bool IsValidDiagramType(string json)
    {
        string[] allowedDiagramTypes = { "sequence_diagram", "class_diagram", "deployment_diagram" };
        string diagramType = new JsonParser(json).GetDiagramType(); // Gets the diagram type
        // Checks if allowedDiagramTypes contains diagram type of the string json
        if ((((IList<string>)allowedDiagramTypes).Contains(diagramType)))
            return true;
        return false;
    }
}
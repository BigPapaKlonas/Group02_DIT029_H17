using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt.Messages;

public class Coordinator
{
    private Button uplButton;
    private string parentTopic;

    //Constructor for instructor
    public Coordinator(string nJson, string instructorName, string diagramName)
    {
        MqttClientDAVE MqttClientDAVE = EstablishConnection();
        parentTopic = instructorName + "/" + diagramName;

        //Publushes the instructor's name
        MqttClientDAVE.Publish("instructors", instructorName, true);

        //Sets the state to pause
        MqttClientDAVE.Publish(parentTopic + "/state", "pause", true);

        //The parsing should be called here//
        //MqttClientDAVE.Publish(parentTopic, "PLACEHOLDER_DIAGRAM", true);
        MqttClientDAVE.Publish(parentTopic, nJson, true); //dummy data

        //For development 
        MqttClientDAVE.Subscribe(parentTopic + "/#");
    }

    //Constructor for student
    public Coordinator(string subscribeTopic, string studentName)
    {
        MqttClientDAVE MqttClientDAVE = EstablishConnection();
        parentTopic = subscribeTopic;

        //Subscribes to the choosen topic
        MqttClientDAVE.Subscribe(parentTopic);

        //Publishes the students name to the 'classroom'
        MqttClientDAVE.Publish(parentTopic + "/students", studentName, true);
    }

    private MqttClientDAVE EstablishConnection()
    {
        // Creates a MqttClientDAVE with the following credentials
        MqttClientDAVE MqttClientDAVE = new MqttClientDAVE("127.0.0.1", 1883, "Unity1");

        // Assign handler for handling the receiving messages 
        MqttClientDAVE.GetMqttClient().MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

        return MqttClientDAVE;
    }

    void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received\r\n" + "Topic: " + e.Topic + "\r\n" + "Message: " +
            System.Text.Encoding.UTF8.GetString(e.Message));

        CheckReceived(e);
    }

    // Calls helper methods to check the received message
    private void CheckReceived(MqttMsgPublishEventArgs e)
    {
        State(e);
        Processes(e);
        Diagram(e);
        Students(e);
    }

    void State(MqttMsgPublishEventArgs e)
    {
        if (e.Topic == parentTopic + "/state" && System.Text.Encoding.UTF8.GetString(e.Message) == "start")
            Debug.Log("State: " + System.Text.Encoding.UTF8.GetString(e.Message));
            //Pause animation and simulation

        else if (e.Topic == parentTopic + "/state" && System.Text.Encoding.UTF8.GetString(e.Message) == "pause")
            Debug.Log("State: " + System.Text.Encoding.UTF8.GetString(e.Message));
            //Pause animation and simulation

    }

    void Processes(MqttMsgPublishEventArgs e)
    {
        if (e.Topic == parentTopic + "/processes")
        {
            Debug.Log("Processes: " + System.Text.Encoding.UTF8.GetString(e.Message));
            //Render systemboxes
        }
    }

    void Diagram(MqttMsgPublishEventArgs e)
    {
        if (e.Topic == parentTopic + "/diagram")
        {
            Debug.Log("Messages: " + System.Text.Encoding.UTF8.GetString(e.Message));
            //Render message
        }
    }

    void Students(MqttMsgPublishEventArgs e)
    {
        if (e.Topic == parentTopic + "/students")
        {
            Debug.Log("Students: " + System.Text.Encoding.UTF8.GetString(e.Message));
            //Add student name to scene
        }
    }

    private void RenderSystemBoxes(JSONSequence JSONSequence)
    {
        uplButton.GetComponent<RenderSystemBoxes>().CreateSystemBoxes(JSONSequence);
    }

    private void RenderMessages(JSONSequence JSONSequence)
    {
        uplButton.GetComponent<StartMessages>().NewMessage(JSONSequence);
    }

}

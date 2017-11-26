using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

public class SubscribingStudents : MonoBehaviour
{
    public GUISkin customSkin;

    private ConnectionManager coordinator = ConnectionManager.coordinator;  // Used for publish/subscribe
    private List<string> studentList = new List<string>();
    private Vector2 scrollPosition;                                         // Used to place ScrollView
    // Creates the rectangle for the log
    private Rect windowRect = new Rect(0, 0, Screen.width, Screen.height * 0.12f);

    private void Start()
    {
        //Subscribes to the current session's student topic
        coordinator.Subscribe("root/" + coordinator.GetInstructor() + "/" +
                 coordinator.GetDiagram() + "/students");
    }

    private void OnEnable()
    {
        //Called when the object becomes enabled and active.
        coordinator.GetMqttClient().MqttMsgPublishReceived += SubscribingStudents_Handler;
    }

    private void OnDisable()
    {
        coordinator.GetMqttClient().MqttMsgPublishReceived += null;
    }

    void OnGUI()
    {
        if (studentList.Count > 0)  //If the list contains at least one student
        {
            /*
	        * Creates a window with id 11111 based on windowRect dimensions with StudentListWindow
            * and with the title of the numbers of subscribing students
	        */
            GUILayout.Window(11111, windowRect, StudentListWindow,
                "Subscribed students: " + studentList.Count.ToString());
        }
    }

    // Handler that receives and handles messages from the subscribed topic
    void SubscribingStudents_Handler(object sender, MqttMsgPublishEventArgs e)
    {
        // Verifies the message's topic
        if (e.Topic == "root/" + coordinator.GetInstructor() + "/" +
                 coordinator.GetDiagram() + "/students")
        {
            studentList.Add(System.Text.Encoding.UTF8.GetString(e.Message)); // Adds message to list
        }
    }

    // GUI window that houses the student names
    void StudentListWindow(int windowID)
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition); // Starts/adds the scroll view
        GUILayout.BeginHorizontal();                                // Starts/adds horizontal area

        foreach (string student in studentList)                     // Iterates through studentList
            GUILayout.Button(student, customSkin.button);           // Adds student as button title
                                                                    // .. and applies a custom skin
        GUILayout.EndHorizontal();                                  
        GUILayout.EndScrollView();                                  
    }
}
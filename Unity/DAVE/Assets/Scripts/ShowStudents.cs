using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt.Messages;

public class ShowStudents : MonoBehaviour
{
    private Image Arrow;                                                    // Arrow image on button
    public Sprite downArrow;
    public Sprite upArrow;
    public GUISkin customSkin;                                              // Skin for the labels

    ConnectionManager coordinator = ConnectionManager.coordinator;  // Used for publish/subscribe
    private List<string> studentList = new List<string>();
    private Vector2 scrollPosition;                                         // Used to place ScrollView
    // Creates and positions the rectangle for the log
    private Rect windowRect = new Rect(Screen.width - 360f, 30, 179f, Screen.height);
    private bool showStudentsWindow = true;

    // Gets objects and sets click listener
    private void Start()
    {
        //Subscribes to the current session's student topic
        coordinator.Subscribe("root/" + coordinator.GetInstructor() + "/" +
            coordinator.GetRoom() + "/students");
        GetComponent<Button>().onClick.AddListener(OnClick);
        Arrow = GameObject.FindGameObjectWithTag("arrow_students").GetComponent<Image>();
    }

    private void OnClick()
    {
        Debug.Log("logmsg" + "*" + "43" + "*" + "34" + "*" + "mmmmmmm miami"); // For debug
        showStudentsWindow = !showStudentsWindow; // // Disables/enables the students window

        if (showStudentsWindow)
            Arrow.sprite = downArrow; // Make drop down arrow point downwards
        else
            Arrow.sprite = upArrow;
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

    private void OnGUI()
    {
        if (!showStudentsWindow)
            return; // Returns if log is disabled

        /*
        * Creates a window with id 11111 based on windowRect dimensions with StudentListWindow
        * and with the title of the numbers of subscribing students
        */
        GUILayout.Window(11111, windowRect, StudentListWindow, studentList.Count.ToString());
    }

    // Handler that receives and handles messages from the subscribed topic
    private void SubscribingStudents_Handler(object sender, MqttMsgPublishEventArgs e)
    {
        // Verifies the message's topic
        if (e.Topic == "root/" + coordinator.GetInstructor() + "/" +
                 coordinator.GetRoom() + "/students")
        {
            studentList.Add(System.Text.Encoding.UTF8.GetString(e.Message)); // Adds message to list
        }
    }

    // GUI window that houses the student names
    private void StudentListWindow(int windowID)
    {
        
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (string student in studentList)          // Iterates through studentList
        { 
            GUILayout.Label(student, customSkin.label);  // Adds student name and applies a custom style
            GUILayout.Space(6);                          // Adds space between the student names
        }
        GUILayout.EndScrollView();
    }
}

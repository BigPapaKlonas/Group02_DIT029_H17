using System.Collections.Generic;
using UnityEngine;

public class SubscribingStudents : MonoBehaviour
{
    private ConnectionManager coordinator = ConnectionManager.coordinator;
    private List<string> studentList = new List<string>();                  // List of Log structures
    private Vector2 scrollPosition;                                         // Used to place ScrollView
    private Rect windowRect = new Rect(0, 0, Screen.width, Screen.height * 0.1f);

    private void Start()
    {
        coordinator.Subscribe("root/" + coordinator.GetInstructor() + "/" +
                coordinator.GetDiagram() + "/students");
    }

    private void OnEnable()
    {
        coordinator.GetMqttClient().MqttMsgPublishReceived += SubscribingStudents_Handler;
    }

    private void OnDisable()
    {
        coordinator.GetMqttClient().MqttMsgPublishReceived += null;
    }

    void OnGUI()
    {
        GUILayout.Window(123456, windowRect, LogWindow, "", new GUIStyle());
    }


    void SubscribingStudents_Handler(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
    {
        if (e.Topic == "root/" + coordinator.GetInstructor() + "/" +
                 coordinator.GetDiagram() + "/students")
        {
            studentList.Add(System.Text.Encoding.UTF8.GetString(e.Message));
        }
    }

    void LogWindow(int windowID)
    {
        if (studentList.Count > 0)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            GUILayout.BeginHorizontal();

            foreach (string student in studentList)
            {
                GUILayout.Label(student);
                //new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter});
                GUILayout.Space(30);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }
    }
}
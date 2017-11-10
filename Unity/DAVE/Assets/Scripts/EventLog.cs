using System.Collections.Generic;
using UnityEngine;

/*
 * Log to display what is going on in the diagram 'in-game'.
 * Calling Debug.log("logmsg" + "*"+ placeholder.y + "*" + placeholder.z + "*" + "zzz") 
 * will add "zzz" as a button to the log and onClick will take camera to placeholder position
 * Based on: https://gist.github.com/mminer/975374
 **/

public class EventLog : MonoBehaviour
{
    // Structure used for each log item 
    struct Log
    {
        public string message;
        public Vector3 targetPosition;
        public string stackTrace;
        public LogType type;
    }

    public KeyCode toggleKey = KeyCode.L;   // The hotkey to show and hide the log window
    List<Log> logs = new List<Log>();       // List of Log structures
    Vector2 scrollPosition;                 // Used to place ScrollView
    public bool showLogWindow = true;       // True on start

    // Creates the rectangle for the log
    Rect windowRect = new Rect(Screen.width * 0.8f, 0, Screen.width * 0.2f, Screen.height);
    // Label for clear button
    GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
    // Allows for repositioning the camera
    CameraOrbit cameraOrbitScript;

    void OnEnable() //Called when the object becomes enabled and active.
    {
        // Assigns HandleLog function to handle log messages received
        Application.logMessageReceived += HandleLog;
        // Gets the cameraOrbit script
        cameraOrbitScript = (CameraOrbit)Camera.main.GetComponent(typeof(CameraOrbit));
    }

    void OnDisable()
    {
        Application.logMessageReceived += null;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))    // Disable/enables the log on toggle key pressed
        {
            showLogWindow = !showLogWindow;
        }
    }

    void OnGUI()
    {
        if (!showLogWindow)
        {
            return; // Returns if log is disabled
        }

        // Creates a window with id 123456 based on windowRect dimensions with LogWindow and title "Log"
        GUILayout.Window(123456, windowRect, LogWindow, "Log");
    }


    // GUI window that houses the log items
    void LogWindow(int windowID)
    {
        //Create GUIStyles for the different buttons that will be added to the window
        GUIStyle logBtnStyle = new GUIStyle(GUI.skin.button)
        {
            wordWrap = true,
            alignment = TextAnchor.UpperLeft
        };
        GUIStyle clearBtnStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleCenter
        };

        scrollPosition = GUILayout.BeginScrollView(scrollPosition); // Starts the scroll view

        // Iterate through the logs.
        foreach (Log log in logs)
        {
            // Creates button that executes the if statement on click
            if (GUILayout.Button(log.message, logBtnStyle))
            {
                // Sets camera position to targetPosition
                cameraOrbitScript.SetPosition(log.targetPosition);
            }
        }

        GUILayout.EndScrollView();  // Ends the scroll view


        if (GUILayout.Button(clearLabel, clearBtnStyle))
        {
            logs.Clear();   //Clears the log when the button is clicked
        }
    }


    // Decodes and records a log from the log callback.
    void HandleLog(string message, string stackTrace, LogType type)
    {
        // Checks type and if message starts with logmsg
        if (type.Equals(LogType.Log) && message.StartsWith("logmsg"))
        {
            message = message.Remove(0, 6);             // Removes 'logmsg'
            var y = float.Parse(message.Split('*')[1]); // Get coordinates from string array
            var z = float.Parse(message.Split('*')[2]); // and parses to floats

            logs.Add(new Log()                          // Adds a new Log to the list of logs
            {
                message = message.Split('*')[3],        // Assigns the Log structure's message 
                targetPosition = new Vector3(0, y, z)   // Creates and assigns a Vector3 based on y and z
            });
        }
    }
}
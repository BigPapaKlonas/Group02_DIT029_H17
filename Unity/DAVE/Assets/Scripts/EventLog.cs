using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Log to display what is going on in the diagram 'in-game'.
/// Calling Debug.log("logmsg" + "*"+ placeholder.y + "*" + placeholder.z + "*" + "zzz") 
/// will add "zzz" as a button to the log and onClick will take camera to placeholder position
/// Based on: https://gist.github.com/mminer/975374
/// </summary>

public class EventLog : MonoBehaviour
{
    // Structure used for each log item 
    struct Log
    {
        public string message;
        public string stackTrace;
        public LogType type;
    }

    public KeyCode toggleKey = KeyCode.L;   // The hotkey to show and hide the log window
    List<Log> logs = new List<Log>();       // List of Log structures
    Vector2 scrollPosition;                 // Used to place ScrollView
    public bool showLogWindow = true;       // True on start

    Rect windowRect = new Rect(Screen.width * 4 / 5f, 0, Screen.width * 1 / 5f, Screen.height);     // Creates the rectangle for the log
    GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");          // Label for clear button
    CameraOrbit cameraOrbitScript;                                                                  // Allows for repositioning the camera

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;                                    // Assigned when EventLog object is enabled
        cameraOrbitScript = (CameraOrbit)Camera.main.GetComponent(typeof(CameraOrbit)); // Gets the cameraOrbit script
    }

    void OnDisable()
    {
        Application.logMessageReceived += null;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))    //Disable the log if key pressed
        {
            showLogWindow = !showLogWindow;
        }
    }

    void OnGUI()
    {
        if (!showLogWindow)
        {
            return;
        }

        windowRect = GUILayout.Window(123456, windowRect, LogWindow, "Log"); //Creates a window with LogWindow and title
    }


    /// <summary>
    /// GUI window that displays the log items
    /// </summary>
    /// <param name="windowID">Window ID.</param>
    void LogWindow(int windowID)
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        // Iterate through the logs.
        for (int i = 0; i < logs.Count; i++)
        {
            var log = logs[i];

            // Creates Vector3 from MessageText position to where the log item's corresponding MessageText is
            var y = float.Parse(log.message.Split('*')[1]);
            var z = float.Parse(log.message.Split('*')[2]);
            Vector3 messageTextPosition = new Vector3(0, y, z);
        
            if (GUILayout.Button(log.message = log.message.Split('*')[3], new GUIStyle(GUI.skin.button) { wordWrap = true, alignment = TextAnchor.UpperLeft })) // Creates button
            {
                cameraOrbitScript.SetPosition(messageTextPosition); //OnClick the camera's position is set to where the messageText is
            }
        }

        GUILayout.EndScrollView();


        GUILayout.BeginHorizontal();

        if (GUILayout.Button(clearLabel, new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter }))
        {
            logs.Clear();   //Clears the log OnClick
        }

        GUILayout.EndHorizontal();
    }

    /// <summary>
	/// Records a log from the log callback.
	/// </summary>
	void HandleLog(string message, string stackTrace, LogType type)
    {
        if (type.Equals(LogType.Log) && message.StartsWith("logmsg"))   // Checks type and if message starts with logmsg
        {
            logs.Add(new Log()
            {
                message = message.Remove(0, 6), // Removes logmsg
            });
        }
    }
}
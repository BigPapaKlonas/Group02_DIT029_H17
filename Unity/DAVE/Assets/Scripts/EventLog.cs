using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Log to display what is going on in the diagram in-game.
/// Calling Debug.log("logmsg" + "*"+ placeholder.y + "*" + placeholder.z + "*" + "zzz") 
/// will add "zzz" as a button to the log and onClick will take camera to placeholder position
/// Based on: https://gist.github.com/mminer/975374
/// </summary>
/// 

public class EventLog : MonoBehaviour
{
    // Structure used for each log item 
    struct Log
    {
        public string message;
        public string stackTrace;
        public LogType type;
    }

    public KeyCode toggleKey = KeyCode.L;    // The hotkey to show and hide the console window.
    List<Log> logs = new List<Log>();
    Vector2 scrollPosition;
    public bool showLogs = true;                    // Show on start

    Rect windowRect = new Rect(Screen.width * 4 / 5f, 0, Screen.width * 1 / 5f, Screen.height);     // Creates the rectangle for the log
    GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");          //Label for clear button
    CameraOrbit cameraOrbitScript;

    void OnEnable()
    {
        cameraOrbitScript = (CameraOrbit)Camera.main.GetComponent(typeof(CameraOrbit));    // Gets the cameraOrbit script
        Application.logMessageReceived += HandleLog; // Assigned when EventLog object is enabled
    }

    void OnDisable()
    {
        Application.logMessageReceived += null;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))    //Disable the log if key pressed
        {
            showLogs = !showLogs;
        }
    }

    void OnGUI()
    {
        if (!showLogs)
        {
            return;
        }

        windowRect = GUILayout.Window(123456, windowRect, LogWindow, "Log"); //Creates window with the log in the windowRect
    }
    

    /// <summary>
    /// A window that displayss the recorded logs.
    /// </summary>
    /// <param name="windowID">Window ID.</param>
    void LogWindow(int windowID)
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        float y = 0;
        float z = 0;
        Vector3 messageTextPosition = new Vector3();

        // Iterate through the logs.
        for (int i = 0; i < logs.Count; i++)
        {
            var log = logs[i];

            y = float.Parse(log.message.Split('*')[1]);
            z = float.Parse(log.message.Split('*')[2]);
            messageTextPosition = new Vector3(0, y, z);
            log.message = log.message.Split('*')[3];


            if (GUILayout.Button(log.message, new GUIStyle(GUI.skin.button){wordWrap = true, alignment = TextAnchor.UpperLeft}))
            {
                cameraOrbitScript.SetPosition(messageTextPosition);
            }
        }

        GUILayout.EndScrollView();


        GUILayout.BeginHorizontal();


        if (GUILayout.Button(clearLabel, new GUIStyle(GUI.skin.button){alignment = TextAnchor.MiddleCenter}))
        {
            logs.Clear();
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
                type = type,
            });
        }
    }
}
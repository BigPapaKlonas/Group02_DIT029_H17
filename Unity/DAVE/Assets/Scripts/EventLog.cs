using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Log to display what is going on in the diagram in-game.
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

    Rect windowRect = new Rect(Screen.width * 4 / 5f, 0, Screen.width * 1 / 5f, Screen.height); // Creates the rectangle for the log
    GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");      //Label for clear button

    void OnEnable()
    {
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

        // Iterate through the logs.
        for (int i = 0; i < logs.Count; i++)
        {
            var log = logs[i];

            GUILayout.Label(log.message);
        }

        GUILayout.EndScrollView();


        GUILayout.BeginHorizontal();

        if (GUILayout.Button(clearLabel))
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
                message = message.Remove(0, 7), // Removes logmsg
                type = type,
            });
        }
    }
}
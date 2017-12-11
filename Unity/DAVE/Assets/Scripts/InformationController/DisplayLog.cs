using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Window with a log displaying what is going on in the diagram 'in-game'. Clicking on one of the
 * generated buttons will zoom into the event it corresponds to.
 * Calling Debug.log("logmsg" + "*"+ placeholder.y + "*" + placeholder.z + "*" + "zzz") 
 * will add "zzz" as a button to the log and onClick will take camera to placeholder position
 * Based on: https://gist.github.com/mminer/975374
 **/

public class DisplayLog : MonoBehaviour
{
    // Structure used for each log item 
    private struct Log
    {
        public string message;
        public Vector3 targetPosition;
        public string stackTrace;
        public LogType type;
    }

    private Image Arrow;                            // Arrow image on button
    public Sprite downArrow;                
    public Sprite upArrow;                  

    private List<Log> logs = new List<Log>();       // List of Log structures
    private Vector2 scrollPosition;                 // Used to place ScrollView
    private bool showLogWindow = true;              // True on start

    // Creates and positions the rectangle for the log
    private Rect windowRect = new Rect(Screen.width - 179f, 30, 179f, Screen.height);
    // Label for clear button
    private GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
    // Allows for repositioning the camera
    private CameraMover cameraOrbitScript;
    GameObject player;

    // Gets objects and sets click listener
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Button>().onClick.AddListener(OnClick);
        Arrow = GameObject.FindGameObjectWithTag("arrow_log").GetComponent<Image>();
    }

    private void OnEnable() //Called when the object becomes enabled and active.
    {
        // Assigns HandleLog function to handle log messages received
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived += null;
    }

    private void OnClick()
    {
        showLogWindow = !showLogWindow; // Disables the log window

        if (showLogWindow)
            Arrow.sprite = downArrow;   // Make drop down arrow point downwards
        else
            Arrow.sprite = upArrow;
    }

    private void OnGUI()
    {
        if (!showLogWindow)
            return; // Returns if log is disabled

        // Creates a window with id 123456 based on windowRect dimensions with LogWindow and title "Log"
        GUILayout.Window(123456, windowRect, LogWindow, "");
    }


    // GUI window that houses the log items
    private void LogWindow(int windowID)
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
                player.GetComponent<CameraModeController>().SetPosition(log.targetPosition);
            }
        }

        GUILayout.EndScrollView();  // Ends the scroll view


        if (GUILayout.Button(clearLabel, clearBtnStyle))
        {
            logs.Clear();   //Clears the log when the button is clicked
        }
    }


    // Decodes and records a log from the log callback.
    private void HandleLog(string message, string stackTrace, LogType type)
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
                targetPosition = new Vector3(4, y, z)   // Creates and assigns a Vector3 based on y and z
            });
        }
    }
}
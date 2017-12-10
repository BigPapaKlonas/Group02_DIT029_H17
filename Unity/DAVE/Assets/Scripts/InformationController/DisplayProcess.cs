using System;
using UnityEngine;

public class DisplayProcess : MonoBehaviour {

    private bool processesVisible;
    private TextMesh text;
    private Device device;
    private Vector2 scrollPosition;
    private void OnMouseDown()
    {
            processesVisible = true;
        
    }

    void OnGUI()
    {
        if (processesVisible)
        {
            Rect windowRect = new Rect(UnityEngine.Screen.width/2, UnityEngine.Screen.height/2-180, 
                150, 300);
            GUILayout.Window(5, windowRect, ProcessWindow, "Processes");
        }
    }

    private void ProcessWindow(int id)
    {
        scrollPosition= GUILayout.BeginScrollView(scrollPosition); 

        foreach (String proc in device.GetProcesses())
        {

            GUILayout.Label(proc);
            GUILayout.Space(5);
        }

        GUILayout.EndScrollView();
        if (GUILayout.Button("Close"))
        {
            processesVisible = false;
        }
    }

    public void GetNameText(TextMesh nameText)
    {
        text = nameText;
    }
    public void GetDevice(Device d)
    {
        device = d;
    }
}

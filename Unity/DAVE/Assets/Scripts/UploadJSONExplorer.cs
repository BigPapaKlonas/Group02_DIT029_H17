using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using SimpleJSON;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class UploadJSONExplorer : MonoBehaviour, IPointerDownHandler
{
    // The following variables dictates the file explorer's behaviour and looks
    public string Title = "Select the your diagram file..";
    public string FileName = "";
    public string Directory = "";
    public string Extension = "json";
    public bool Multiselect = false;

      public void OnPointerDown(PointerEventData eventData) { }

    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel(Title, Directory, Extension, Multiselect);
        if (paths.Length > 0)
        {
            // Starts a new routine with the path to the selected file as argument
            StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
        }
    }

    private IEnumerator OutputRoutine(string url)
    {
        // Debug: file's path
        Debug.Log("Path: " + url);

        var loader = new WWW(url);
        yield return loader;

        // Read the json from the file into a string
        string output = loader.text;

        // Debug: Json text
        Debug.Log("JSON: " + output);

        ParseToJSONClass(output);
        //ParseToJSONObjects(output);

    }

    public void ParseToJSONClass(string jsonString)
    {
        var N = JSON.Parse(jsonString);
        string diagram_type = N["type"].Value;

        if (diagram_type.Equals("sequence_diagram"))
        {
            // To get a JSON object
            var JSONObject = QuickType.JsonHelper.ParseToClass(jsonString);

            // Example how to get informatzione
            Debug.Log("We have got: " + JSONObject.Processes.Count + " processes");

            foreach (var process in JSONObject.Processes)
            {
                Debug.LogFormat("Class: {0}" + "\r\n" + "Name: {1}", process.Class, process.Name);
            }
        }
    }

    public void ParseToJSONObjects(string jsonString)
    {
        var jSONNode = JSON.Parse(jsonString);
        string diagram_type = jSONNode["type"].Value;
        var meta = jSONNode["meta"].AsObject;          // .."["version"].Value" to get version

        Debug.Log("Meta: " + meta.ToString());
        Debug.Log("Type: " + diagram_type);

        if (diagram_type.Equals("sequence_diagram"))
        {
            var process = jSONNode["processes"].AsArray;   // gets all processes in a JSONArray, jSONNode["processes"][0].AsObject to get the first process
            var diagram = jSONNode["diagram"].AsObject;    //gets diagram as JSONObject, jSONNode["diagram"]["content"][0]["content"][0].AsObject to get the first
                                                           //                              message from the first parallelism
            Debug.Log("Process: " + process.ToString());
            Debug.Log("Process: " + process.ToString());
        }
    }
}
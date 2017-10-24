using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using SimpleJSON;
using System.Linq;
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
        //JSONParser.parse(output);
        Parse(output);
    }

    public void Parse(string jsonString)
    {
        var N = JSON.Parse(jsonString);

        var meta = N["meta"].AsObject;          // .."["version"].Value" to get version 
        string diagram_type = N["type"].Value;  //
        var process = N["processes"].AsArray;   // gets all processes in a JSONArray, N["processes"][0].AsObject to get the first process
        var diagram = N["diagram"].AsObject;    //gets diagram as JSONObject, N["diagram"]["content"][0]["content"][0].AsObject to get the first
                                                //                              message from the first parallelism  

        Debug.Log("Meta: " + meta.ToString());
        Debug.Log("Type: " + diagram_type);
        Debug.Log("Process: " + process.ToString());
        Debug.Log("Diagram: " + diagram.ToString());
    }

    public static void parse(string jsonString)
    {
        JSONCLASS myJSONDiagram = new JSONCLASS();
        JsonUtility.FromJsonOverwrite(jsonString, myJSONDiagram);
        Debug.Log("JSON: " + myJSONDiagram.Type);
        Debug.Log("JSON: " + myJSONDiagram.Meta.getFormat());
    }

    public class JSONCLASS
    {
        public Meta Meta { get; set; }
        public Diagram Diagram { get; set; }
        public List<Process> Processes { get; set; }
        public string Type { get; set; }
    }



    public class Meta
    {
        public string Format { get; set; }
        public List<object> Extensions { get; set; }
        public string Version { get; set; }

        public string getFormat()
        {
            return Format;
        }
    }

    public class Diagram
    {
        public List<Content> Content { get; set; }
        public string Node { get; set; }
    }

    public class Content
    {
        public List<OtherContent> OtherContent { get; set; }
        public string Node { get; set; }
    }

    public class OtherContent
    {
        public List<string> Message { get; set; }
        public string From { get; set; }
        public string Node { get; set; }
        public string To { get; set; }
    }

    public class Process
    {
        public string Class { get; set; }
        public string Name { get; set; }
    }
}
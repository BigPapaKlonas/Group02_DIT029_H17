using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UploadJSONExplorer : MonoBehaviour
{
    // The following variables dictates the file explorer's behaviour and looks
    public string Title = "Select the your diagram file..";
    public string FileName = "";
    public string Directory = "";
    public string Extension = "json";
    public bool Multiselect = true;
    private Button button;
    private int offset;

    public GameObject playBtnPrefab;
    private int fileCounter = 0;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        offset = -110;
        if(SceneManager.GetActiveScene().name == "Main")
        {
            offset += 35;
        }
        Debug.Log("im start " + offset);
    }

    private void OnClick()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel(Title, Directory, Extension, Multiselect);
         
        foreach (var file in paths)
        {
            if (file.Length > 0)
            {
                fileCounter++;
                // Starts a new routine with the path to the selected file as argument
                AddJson(new Uri(file).AbsoluteUri);
            }
            
        }
        Debug.Log("on click before adding " + offset);
        offset += 35;
        Debug.Log("on click after adding " + offset);

        // Only loads the "Main" scene in case file counter is not zero and active scene is "Start"
        if (SceneManager.GetActiveScene().name == "Start" && fileCounter != 0)
        {
            SceneManager.LoadScene("Main");
        }

        // Instantiates and activates the playBtn
        else if (SceneManager.GetActiveScene().name == "Main" && fileCounter != 0)
        {
            GameObject playBtn = Instantiate(playBtnPrefab);
            playBtn.SetActive(true);
            GameObject canvas = GameObject.Find("Canvas_Show_Reset_Upload_Play");
            playBtn.transform.SetParent(canvas.transform, false);
        }
    }

    // Adds JSON from the path (url) to queue with valid JSONS prepared for uploading
    private void AddJson(string url)
    {
        var loader = new WWW(url);              // Retrieves content from url 
        string output = loader.text;            // Read the json from the file into a string

        if (IsValidJson(output))                // Checks if output is a valid JSON
        {
            if (IsValidDiagramType(output))     // Checks if the json, output, is of a valid type

            {
                JsonParser parser = new JsonParser(output);
                output = parser.AddMetaToSequence(offset.ToString());
                //parser = new JsonParser(output);
                Debug.Log(output.ToString());
                // Adds the JSON diagram to the queue of strings to be ready to be uploaded
                ConnectionManager.coordinator.AddSelectedJson(output);
                // Coroutine for uploading data to Database
                StartCoroutine(Insert());
            }
            else
                Debug.Log("Invalid type! Not a sequence, class or sequence diagram");
        }
        else
            Debug.Log("JSON not valid! Check JSON structure");
    }

    /* 
	 * Insert to the Database.
	 * when ran in a Coroutine The full ConnectionManager.<variable> needs to be present.
	 */
    IEnumerator Insert()
    {

        string instructor = ConnectionManager.coordinator.GetInstructor();
        string room = ConnectionManager.coordinator.GetRoom();
        string diagramType = ConnectionManager.coordinator.GetDiagramType();

        /* 
		 * If database contains the instructor name: 
		 * Update instructors.diagrams
		 * and add the diagram to diagrams table.
		 * Else:
		 * Add both to instructors and diagrams tables.
		*/

        var update = ConnectionManager.R.Db("root")
            .Table("diagrams").Insert(ConnectionManager.R.Array(
                ConnectionManager.R.HashMap("name", room)
                .With("type", diagramType)
                .With("instructor", instructor)
            ))
            .Run(ConnectionManager.conn);

        yield return update;
        Debug.Log("Successful insert and update of the " + room + " table, for instructor: "
            + instructor);
    }

    // Checks if the json is either a sequence, class or a deployment diagram
    private bool IsValidDiagramType(string json)
    {
        string[] allowedDiagramTypes = {"sequence_diagram", "class_diagram", "deployment_diagram"};
        string diagramType = new JsonParser(json).GetDiagramType(); // Gets the diagram type
        // Checks if allowedDiagramTypes contains diagram type of the string json
        if ((((IList<string>)allowedDiagramTypes).Contains(diagramType)))
            return true;
        return false;
    }

    // Source: https://goo.gl/n89LoF
    private bool IsValidJson(string strInput)
    {
        strInput = strInput.Trim();
        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
            (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(strInput);
                obj.Equals(obj); //Could not suppress warning 'value is assigned but never use' so this prevents the error message
                return true;
            }
            catch (JsonReaderException jex) //Exception in parsing json
            {
                Debug.Log(jex.Message);
                return false;
            }
            catch (Exception ex) //some other exception
            {
                Debug.Log(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
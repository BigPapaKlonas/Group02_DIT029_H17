using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using SFB;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[RequireComponent(typeof(Button))]
public class UploadJSONExplorer : MonoBehaviour, IPointerDownHandler
{
    // The following variables dictates the file explorer's behaviour and looks
    public string Title = "Select the your diagram file..";
    public string FileName = "";
    public string Directory = "";
    public string Extension = "json";
    public bool Multiselect = true;
    private Button button;
    
    // offset used to position house "districts"
    float positionOffset = -50;


    public void OnPointerDown(PointerEventData eventData) { }

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel(Title, Directory, Extension, Multiselect);
        foreach(var file in paths)
        {
            if (file.Length > 0)
            {
                // Starts a new routine with the path to the selected file as argument
                StartCoroutine(OutputRoutine(new System.Uri(file).AbsoluteUri));
            }
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
        // Debug.Log("Raw JSON: " + output);

		JsonParser parser = new JsonParser (output);

		Debug.Log ("Diagram type: " + parser.GetDiagramType ());

		ConnectionManager.coordinator.SetDiagramType (parser.GetDiagramType ());

		output = parser.AddMetaToSequence ("root/" + 
			ConnectionManager.coordinator.GetInstructor ().Replace (" ", "").ToLower () + "/" + 
			ConnectionManager.coordinator.GetRoom ().Replace (" ", "").ToLower ()
		);

		Debug.Log (output);
		ConnectionManager.coordinator.SetSessionJson (output);
	
        // Check if output is valid
        if (!IsValidJson(output))
        {
            yield return "JSON not valid, check JSON structure";
        }

        DiagramBroker broker = new DiagramBroker(output, positionOffset);
        Debug.Log(positionOffset);
        positionOffset += 40;
        StartCoroutine(Insert ());
    }

	/* 
	 * Insert to the Database.
	 * when ran in a Coroutine The full ConnectionManager.<variable> needs to be present.
	 */
	IEnumerator Insert()
	{

		string instructor = ConnectionManager.coordinator.GetInstructor ();
		string diagram = ConnectionManager.coordinator.GetRoom ();
		string diagramType = ConnectionManager.coordinator.GetDiagramType ();

        /* 
		 * If database contains the instructor name: 
		 * Update instructors.diagrams
		 * and add the diagram to diagrams table.
		 * Else:
		 * Add both to instructors and diagrams tables.
		*/

		var update = ConnectionManager.R.Db("root")
			.Table("diagrams").Insert(ConnectionManager.R.Array(
				ConnectionManager.R.HashMap("name", diagram)
				.With("type", diagramType)
				.With("instructor", instructor)
			))
			.Run(ConnectionManager.conn);

        yield return update;
            Debug.Log("Successful insert and update of the " + diagram + " table, for instructor: " + instructor);
            SceneManager.LoadScene(ConnectionManager.coordinator.GetDiagramType());
	}

    private bool IsValidJson(string strInput)
    //https://stackoverflow.com/questions/14977848/how-to-make-sure-that-string-is-valid-json-using-json-net
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
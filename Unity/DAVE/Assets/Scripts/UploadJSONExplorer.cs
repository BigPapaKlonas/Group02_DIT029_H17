using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using SFB;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;


[RequireComponent(typeof(Button))]
public class UploadJSONExplorer : MonoBehaviour, IPointerDownHandler
{
    // The following variables dictates the file explorer's behaviour and looks
    public string Title = "Select the your diagram file..";
    public string FileName = "";
    public string Directory = "";
    public string Extension = "json";
    public bool Multiselect = false;
    private Button button;

    public void OnPointerDown(PointerEventData eventData) { }

    void Start()
    {
        button = GetComponent<Button>();
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
        // Debug.Log("Raw JSON: " + output);

		JsonParser parser = new JsonParser (output);

		Debug.Log ("Diagram type: " + parser.GetDiagramType ());

		Coordinator.coordinator.SetDiagramType (parser.GetDiagramType ());

		output = parser.AddMetaToSequence ("root/" + 
			Coordinator.coordinator.GetInstructor ().Replace (" ", "").ToLower () + "/" + 
			Coordinator.coordinator.GetDiagram ().Replace (" ", "").ToLower ()
		);

		Debug.Log (output);

		Coordinator.coordinator.SetSessionJson (output);
	
		Insert ();

		SceneManager.LoadScene (Coordinator.coordinator.GetDiagramType ());

    }

	// Insert to the Database.
	void Insert()
	{

		string instructor = Coordinator.coordinator.GetInstructor ();
		string diagram = Coordinator.coordinator.GetDiagram ();
		string diagramType = Coordinator.coordinator.GetDiagramType ();

		/* 
		 * If database contains the instructor name: 
		 * Update instructors.diagrams
		 * and add the diagram to diagrams table.
		 * Else:
		 * Add both to instructors and diagrams tables.
		*/
		if (Coordinator.R.Db ("root").Table ("instructors").GetField ("name")
			.Contains (instructor).Run (Coordinator.conn)) 
		{
			Coordinator.R.Db("root")
				.Table("diagrams").Insert(Coordinator.R.Array(
					Coordinator.R.HashMap("name", diagram)
					.With("type", diagramType)
					.With("instructor", instructor)
				))
				.Run(Coordinator.conn);

			Coordinator.R.Db ("root")
				.Table ("instructors")
				.Filter (row => row.G ("name").Eq (instructor))
				.Update (Coordinator.R.HashMap("diagrams", Coordinator.R.Array(diagram)))
				.Run(Coordinator.conn);

		} else {

			Coordinator.R.Db("root")
				.Table("diagrams").Insert(Coordinator.R.Array(
					Coordinator.R.HashMap("name", diagram)
					.With("type", diagramType)
					.With("instructor", instructor)
				))
				.Run(Coordinator.conn);

			Coordinator.R.Db("root")
				.Table("instructors").Insert(Coordinator.R.Array(
					Coordinator.R.HashMap("name", instructor)
					.With("diagrams", Coordinator.R.Array())
				)
				)
				.Run(Coordinator.conn);
		}
	}
}
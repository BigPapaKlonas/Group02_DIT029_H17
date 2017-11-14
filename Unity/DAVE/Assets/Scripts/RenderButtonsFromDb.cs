using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RethinkDb.Driver.Net;
using RethinkDb.Driver.Model;
using UnityEngine.SceneManagement;

public class RenderButtonsFromDb : MonoBehaviour {

	public Button buttonPrefab;
	public ScrollRect scroll;
	public RectTransform parentPanel;

	// Add string from unity
	public string table;
	public string row;

	private Vector3 position;

	float y = 90f;
	float x = -160f;

	void Start () {

		RenderButtons (table, row);
		//ObserveChange (table, row);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// Use this to set up an observer for the selected table, the return is the changes made to the selected row. 
	void ObserveChange (string table, string selectedRow)
	{
		Cursor<string> cursor = Coordinator.R.Db ("root")
			.Table (table)
			.Filter (row => 
				row.G ("new_val").G (selectedRow)
				.Gt (row.G ("old_val").G (selectedRow))
			).G ("new_val").RunCursorAsync<string>(Coordinator.conn);
		
		foreach (var i in cursor) {
			Debug.Log("Changes: " + i);
			var btn = Instantiate (buttonPrefab) as Button;
			btn.transform.SetParent (parentPanel, false);
			btn.transform.localScale = new Vector3 (1, 1, 1);
			btn.GetComponentInChildren<Text>().text = i;
			btn.onClick.AddListener(() => ButtonCallBack(btn));
		}
	}

	//This method renders buttons according to our database, use on Start ().
	void RenderButtons (string table, string selectedRow)
	{
        Cursor<string> result;
        switch (table)
        { 
            case "instructors":
                Debug.Log("Case instructors");
                result = Coordinator.R
                .Db("root").Table(table).GetField(selectedRow)
                .RunCursor<string>(Coordinator.conn);
                break;
            case "diagrams":
                Debug.Log("Case diagrams");
                result = Coordinator.R
               .Db("root").Table(table).Filter(row => row.G("instructor").Eq(Coordinator.coordinator.GetInstructor())).GetField(selectedRow)
               .RunCursor<string>(Coordinator.conn);
                break;
            default:
                result = Coordinator.R
                .Db("root").Table(table).GetField(selectedRow)
                .RunCursor<string>(Coordinator.conn);
                break;
        }
			

		foreach(var i in result){
			Debug.Log("Result: " + i);
			var btn = Instantiate (buttonPrefab) as Button;

			btn.transform.SetParent (parentPanel, false);
			btn.transform.localScale = new Vector3 (1, 1, 1);
			btn.GetComponentInChildren<Text>().text = i;
			btn.onClick.AddListener(() => ButtonCallBack(btn));
		}
	}

	private void ButtonCallBack (Button buttonPressed)
	{

        string name = buttonPressed.GetComponentInChildren<Text>().text;

        switch (this.table) 
		{
		case "instructors":
			Debug.Log ("table: " + table + " name: " + name);
			Coordinator.coordinator.SetInstructor (name);
			SceneManager.LoadScene ("DiagramChoice");
			break;
		case "diagrams":
			Debug.Log ("table: " + table + " name: " + name);
			Coordinator.coordinator.SetDiagram (name);
			Coordinator.coordinator.Subscribe (
				"root/" + Coordinator.coordinator.GetInstructor() + "/" + 
				Coordinator.coordinator.GetDiagram()
			);
            SceneManager.LoadScene ("Diagram");
			break;
		default:
			Debug.Log ("Error in table name OnClick");
			break;
		}
	}
		
}
